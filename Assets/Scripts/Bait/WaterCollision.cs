using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] Bait bait;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == sea)
        {
            bait.inWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sea)
        {
            bait.inWater = false;
        }
    }


}
