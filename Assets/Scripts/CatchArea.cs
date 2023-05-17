using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Debug.Log("Get it!");
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Got it!");
            }
        }
    }
}
