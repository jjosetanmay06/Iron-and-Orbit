using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletspeed = 5f;

    private Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        rb.linearVelocity = direction * bulletspeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}