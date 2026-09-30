using UnityEngine;

public class club_movement : MonoBehaviour
{
    public float club_speed = 1f;
    public int damage = 1;
    public float hit_cooldown = 2f;

    public bool player_in_range = false;

    public Transform player;

    private float start_x;
    private float start_y;
    private float start_z;

    private int direction = -1;
    private float next_hit_time = 0f;

    void Start()
    {
        start_x = transform.localPosition.x;
        start_y = transform.localPosition.y;
        start_z = transform.localPosition.z;
    }

    void Update()
    {
        if (player_in_range)
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y + direction * club_speed * Time.deltaTime,
                transform.localPosition.z
            );

            if (transform.localPosition.y >= start_y)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    start_y,
                    transform.localPosition.z
                );

                direction = -1;
            }
        }
        else
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                start_y,
                transform.localPosition.z
            );

            direction = -1;
        }

        if (player != null)
        {
            if (player.position.x > transform.parent.position.x)
            {
                transform.localPosition = new Vector3(
                    Mathf.Abs(start_x),
                    transform.localPosition.y,
                    start_z
                );
            }
            else
            {
                transform.localPosition = new Vector3(
                    -Mathf.Abs(start_x),
                    transform.localPosition.y,
                    start_z
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            player_in_range = true;

            Debug.Log("PLAYER IN CLUB RANGE");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player_in_range = false;
            player = null;

            direction = -1;

            Debug.Log("PLAYER LEFT CLUB RANGE");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            direction = 1;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= next_hit_time)
            {
                Player_movement playerScript =
                    collision.gameObject.GetComponent<Player_movement>();

                if (playerScript != null)
                {
                    playerScript.TakeDamage(damage);

                    next_hit_time = Time.time + hit_cooldown;

                    Debug.Log("CLUB HIT PLAYER FOR " + damage + " DAMAGE");
                }
            }

            direction = -1;
        }
    }
}