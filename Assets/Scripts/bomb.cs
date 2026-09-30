using UnityEngine;

public class bomb : MonoBehaviour
{
    public float timer_bomb = 20f;

    void Update()
    {
        timer_bomb -= Time.deltaTime;

        if (timer_bomb <= 0)
        {
            Destroy(gameObject);
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
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}