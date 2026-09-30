using UnityEngine;

public class enemy_drone : MonoBehaviour
{
    private Rigidbody2D rb;

    public int drone_velocity = 7;
    public float height_above_player = 5f;

    public Transform player;

    public int drone_health = 3;

    public GameObject bombPrefab;

    public float bomb_timer = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        Vector2 target_position = new Vector2(
            player.position.x,
            player.position.y + height_above_player
        );

        Vector2 direction = (target_position - (Vector2)transform.position).normalized;

        rb.linearVelocity = direction * drone_velocity;

        bomb_timer -= Time.deltaTime;

        if (bomb_timer <= 0)
        {
            Instantiate(
                bombPrefab,
                transform.position,
                Quaternion.identity
            );

            bomb_timer = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_movement playerScript =
                collision.gameObject.GetComponent<Player_movement>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(3);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            drone_health--;

            if (drone_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}