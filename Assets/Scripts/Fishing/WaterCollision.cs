using System;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    private float waterLevel;
    private const float FloatHeight = 150f;
    private const float BounceDamp = 1f;

    public static event Action OnEnterSea;



    void Start()
    {
        waterLevel = transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            OnEnterSea.Invoke();
        }

        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            rigidBody.drag += 2;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            ApplyBuoyancy(rigidBody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            rigidBody.drag -= 2;
        }
    }

    public void ApplyBuoyancy(Rigidbody rigidBody)
    {
        Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / FloatHeight);
        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * BounceDamp);
            rigidBody.AddForceAtPosition(uplift, actionPoint);
        }
    }
}
