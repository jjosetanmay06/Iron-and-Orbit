using UnityEngine;

public class Boss_behavior : MonoBehaviour
{
    public int boss_health = 20;
    public float boss_speed = 3f;

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
        // Boss dies when health reaches zero
        if (boss_health <= 0)
        {
            Debug.Log("Boss Dead");
            Destroy(gameObject);
            return;
        }

        // Chase player when in range
        if (player_in_range)
        {
            if (player.position.x > transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }
        else
        {
            // Patrol timer
            patrol_time -= Time.deltaTime;

            if (patrol_time <= 0)
            {
                direction = -direction;
                patrol_time = 10f;
            }
        }

        // Move boss
        rb.linearVelocity = new Vector2(
            boss_speed * direction,
            rb.linearVelocity.y
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            player_in_range = true;

            Debug.Log("PLAYER DETECTED BY BOSS");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player_in_range = false;
            player = null;

            Debug.Log("PLAYER LEFT BOSS RANGE");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("BOSS COLLISION WITH: " + collision.gameObject.name);
        Debug.Log("OBJECT TAG IS: [" + collision.gameObject.tag + "]");

        if (collision.gameObject.CompareTag("Bullet"))
        {
            boss_health--;

            Debug.Log("BOSS HEALTH: " + boss_health);

            if (boss_health <= 0)
            {
                Debug.Log("BOSS DEFEATED");
            }
        }
    }
}