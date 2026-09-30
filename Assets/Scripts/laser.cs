using UnityEngine;

public class LaserTimer : MonoBehaviour
{
    public float chargeTime = 2f;
    public float waitTime = 1f;
    public float maxLength = 8f;

    private float timer = 0f;
    private Vector3 originalScale;

    private enum LaserState
    {
        Charging,
        Waiting
    }

    private LaserState state = LaserState.Charging;

    void Start()
    {
        originalScale = transform.localScale;

        // Start at zero length
        transform.localScale = new Vector3(
            0f,
            originalScale.y,
            originalScale.z
        );
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (state == LaserState.Charging)
        {
            float progress = timer / chargeTime;
            progress = Mathf.Clamp01(progress);

            // Grow in ONE direction
            transform.localScale = new Vector3(
                maxLength * progress,
                originalScale.y,
                originalScale.z
            );

            if (timer >= chargeTime)
            {
                timer = 0f;
                state = LaserState.Waiting;
            }
        }

        else if (state == LaserState.Waiting)
        {
            if (timer >= waitTime)
            {
                timer = 0f;

                // Disappear
                transform.localScale = new Vector3(
                    0f,
                    originalScale.y,
                    originalScale.z
                );

                state = LaserState.Charging;
            }
        }
    }
}