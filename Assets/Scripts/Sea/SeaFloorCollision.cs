using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFloorCollision : MonoBehaviour
{
    public static event Action OnSeaFloorCollision;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
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
