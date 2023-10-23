using System;
using UnityEngine;

public class SeaFloorCollision : MonoBehaviour
{
    public static event Action OnBaitCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            OnBaitCollision?.Invoke();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            OnBaitCollision?.Invoke();
        }
    }
}
