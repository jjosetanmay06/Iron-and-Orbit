using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_movement : MonoBehaviour
{
    public int health = 15;

    public Slider HealthBar;
    public float speed = 5f;
    public float jumpheight = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform firePointup;
    public Transform firePointdown;
    public Transform firePointleft;

    public Vector2 direction;

    private Rigidbody2D rb;
    private bool touch_ground = false;

    public GameObject knifePrefab;
    public Transform hand;

    private GameObject currentKnife;

    public float dashForce = 15f;
    public float dashDuration = 0.15f;

    private bool isDashing = false;
    private float lastDirection = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HealthBar.maxValue = health;
        HealthBar.value = health;
    }

    void Update()
    {
        float movement = Input.GetAxisRaw("Horizontal");

        if (movement != 0)
        {
            lastDirection = movement;
        }

        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(
                movement * speed,
                rb.linearVelocity.y
            );
        }

        if (Input.GetButtonDown("Jump") && touch_ground)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpheight
            );
        }

        // DASH
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Attack"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 difference = mousePosition - (Vector2)transform.position;

            Transform selectedFirePoint;

            if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            {
                if (difference.x > 0)
                {
                    direction = new Vector2(1, 0);
                    selectedFirePoint = firePoint;
                }
                else
                {
                    direction = new Vector2(-1, 0);
                    selectedFirePoint = firePointleft;
                }
            }
            else
            {
                if (difference.y > 0)
                {
                    direction = new Vector2(0, 1);
                    selectedFirePoint = firePointup;
                }
                else
                {
                    direction = new Vector2(0, -1);
                    selectedFirePoint = firePointdown;
                }
            }

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                selectedFirePoint.position,
                Quaternion.identity
            );

            BulletMovement bullet = bulletObject.GetComponent<BulletMovement>();

            bullet.SetDirection(direction);
        }
        if (Input.GetMouseButtonDown(1))
        {
            currentKnife = Instantiate(
                knifePrefab,
                hand.position,
                hand.rotation
            );
        }

        if (Input.GetMouseButton(1))
        {
            if (currentKnife != null)
            {
                currentKnife.transform.position = hand.position;
                currentKnife.transform.rotation = hand.rotation;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (currentKnife != null)
            {
                Destroy(currentKnife);
                currentKnife = null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION ENTER: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            touch_ground = true;
            Debug.Log("TOUCH GROUND = TRUE");
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            TakeDamage(1);

            Debug.Log("PLAYER HEALTH: " + health);

            if (health <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }
        if (collision.gameObject.CompareTag("Boss1"))
        {
            TakeDamage(1);

            Debug.Log("PLAYER HEALTH: " + health);

            if (health <= 0)
            {
                Debug.Log("GAME OVER");
            }
        }
        if (collision.gameObject.CompareTag("bandage"))
        {
            health += 1;

            if (health > HealthBar.maxValue)
            {
                health = (int)HealthBar.maxValue;
            }

            HealthBar.value = health;

            Debug.Log("BANDAGE PICKED UP! Health: " + health);

            Destroy(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("COLLISION EXIT: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            touch_ground = false;
            Debug.Log("TOUCH GROUND = FALSE");
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;

        HealthBar.value = health;

        Debug.Log("Player Health: " + health);
    }
    IEnumerator Dash()
    {
        isDashing = true;

        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0;

        rb.linearVelocity = new Vector2(
            lastDirection * dashForce,
            0
        );

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;

        rb.linearVelocity = new Vector2(
            0,
            rb.linearVelocity.y
        );

        isDashing = false;
    }
}