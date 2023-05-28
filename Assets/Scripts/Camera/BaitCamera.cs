using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] GameObject bait;
    private float cameraDistance;

    private void Start()
    {
        cameraDistance = transform.position.z;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (fishingControlls.fishingStatus != FishingControlls.GetFishingStatus.StandBy)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.1f;
            }

        }
    }
}


