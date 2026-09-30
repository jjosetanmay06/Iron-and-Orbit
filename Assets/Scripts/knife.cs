using UnityEngine;

public class knife : MonoBehaviour
{
    public int damage = 1;

    private Transform hand;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            hand = player.transform.Find("Hand");
        }
    }

    void Update()
    {
        if (hand != null)
        {
            transform.position = hand.position;
            transform.rotation = hand.rotation;
        }

        // Destroy when right click is released
        if (!Input.GetMouseButton(1))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            enemy_behavior enemy =
                collision.gameObject.GetComponent<enemy_behavior>();

            if (enemy != null)
            {
                enemy.enemy_health -= damage;
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            Boss_behavior boss =
                collision.gameObject.GetComponent<Boss_behavior>();

            if (boss != null)
            {
                boss.boss_health -= damage;
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Drone"))
        {
            enemy_drone drone =
                collision.gameObject.GetComponent<enemy_drone>();

            if (drone != null)
            {
                drone.drone_health -= damage;
            }

            Destroy(gameObject);
        }
    }
}