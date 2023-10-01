using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    private const float dragMultiplier = 2f;
    private const float minDrag = 3f;

    private readonly Dictionary<int, float> addedDragByInstanceID = new();
    private readonly Dictionary<int, Vector3> previousVelocityByInstanceID = new();

    public static event Action OnEnterSea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            OnEnterSea?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out var rigidBody))
        {
            ApplyDrag(other, rigidBody);
        }
    }

    private void ApplyDrag(Collider other, Rigidbody rigidBody)
    {
        int instanceID = other.GetInstanceID();
        Vector3 currentVelocity = rigidBody.velocity;
        if (previousVelocityByInstanceID.TryGetValue(instanceID, out Vector3 previousVelocity))
        {
            Vector3 velocityDifference = currentVelocity - previousVelocity;
            float addedDrag = MathF.Max(velocityDifference.magnitude * dragMultiplier, minDrag);
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
}
