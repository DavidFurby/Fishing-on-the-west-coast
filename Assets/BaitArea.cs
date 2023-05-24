using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            other.GetComponent<FishMovement>().GetBaited(transform.position);
        }
    }
}
