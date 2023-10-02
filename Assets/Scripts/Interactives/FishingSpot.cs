using System;
using UnityEngine;
[RequireComponent(typeof(Interactive))]
public class FishingSpot : MonoBehaviour, IInteractive
{
    public static event Action<Vector3, Quaternion> StartFishing;

    public void Interact()
    {
        StartFishing.Invoke(transform.position, transform.rotation);
    }
}
