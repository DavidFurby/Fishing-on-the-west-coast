using UnityEngine;

public class IconAnimator : MonoBehaviour
{
    private const float speed = 1f;

    private const float amplitude = 0.5f;

    private float startYPosition;

    private void Start()
    {
        startYPosition = transform.position.y;
    }

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        float newYPosition = startYPosition + amplitude * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }
}
