using System;
using UnityEngine;

public class SeaFloorCollision : MonoBehaviour
{
    public static event Action OnSeaFloorCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            print("collided");
            OnSeaFloorCollision.Invoke();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            OnSeaFloorCollision.Invoke();
        }
    }
}
