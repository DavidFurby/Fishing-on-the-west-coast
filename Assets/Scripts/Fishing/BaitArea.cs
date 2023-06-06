using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] GameObject bait;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish"))
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            fishMovement.GetBaited(bait);
        }
    }
}
