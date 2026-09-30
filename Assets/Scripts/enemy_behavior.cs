using UnityEngine;

public class enemy_behavior : MonoBehaviour
{
    public int enemy_health = 10;
    public float enemy_speed = 5f;

    public float patrol_time = 3f;
    public int direction = 1;

    public Transform player;
    private bool player_in_range = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (enemy_health <= 0)
        {
            Debug.Log("Enemy Dead");
            Destroy(gameObject);
            return;
        }

        if (player_in_range && player != null)
        {
            if (player.position.x > transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            rb.linearVelocity = new Vector2(
                enemy_speed * direction,
                rb.linearVelocity.y
            );
        }
        else
        {
            patrol_time -= Time.deltaTime;

            if (patrol_time <= 0)
            {
                direction = -direction;
                patrol_time = 10f;
            }

            rb.linearVelocity = new Vector2(
                enemy_speed * direction,
                rb.linearVelocity.y
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            player_in_range = true;

            Debug.Log("PLAYER DETECTED");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player_in_range = false;
            player = null;

            Debug.Log("PLAYER LEFT RANGE");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ENEMY COLLISION WITH: " + collision.gameObject.name);
        Debug.Log("OBJECT TAG IS: [" + collision.gameObject.tag + "]");

        if (collision.gameObject.CompareTag("Bullet"))
        {
            enemy_health--;

            Debug.Log("ENEMY HEALTH: " + enemy_health);
        }
    }
}