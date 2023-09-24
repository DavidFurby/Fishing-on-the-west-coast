using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    private float waterLevel;
    private const float FloatHeight = 150f;
    private readonly float BounceDamp = 0.1f;
    private const float dragMultiplier = 2f;
    private readonly Dictionary<int, float> addedDragByInstanceID = new();
    private readonly Dictionary<int, Vector3> previousVelocityByInstanceID = new();

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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            ApplyBuoyancy(rigidBody);
            ApplyDrag(other, rigidBody);

        }
    }

    private void ApplyDrag(Collider other, Rigidbody rigidBody)
    {
        int instanceID = other.GetInstanceID();
        Vector3 currentVelocity = rigidBody.velocity;
        if(previousVelocityByInstanceID.TryGetValue(instanceID, out Vector3 previousVelocity)) {
            Vector3 velocityDifference = currentVelocity - previousVelocity;
            float addedDrag = velocityDifference.magnitude * dragMultiplier;
            rigidBody.drag = addedDrag;
            addedDragByInstanceID[instanceID] = addedDrag;
        }
       previousVelocityByInstanceID[instanceID] = currentVelocity;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            if (addedDragByInstanceID.TryGetValue(other.GetInstanceID(), out _))
            {
                rigidBody.drag = 0;
                addedDragByInstanceID.Remove(other.GetInstanceID());
                previousVelocityByInstanceID.Remove(other.GetInstanceID());
            }
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
