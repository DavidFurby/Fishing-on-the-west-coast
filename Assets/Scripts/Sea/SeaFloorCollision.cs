using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFloorCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bait")) {
            FishingController.Instance.FishIsBaited = false;
        }
    }
}
