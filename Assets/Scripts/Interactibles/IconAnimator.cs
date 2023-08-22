using UnityEngine;

public class IconAnimator : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 0.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * speed) * amplitude;
    }
}
