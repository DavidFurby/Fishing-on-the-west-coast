using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] GameObject bait;
    private float cameraDistance;
    private float originalCameraDistance;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Casting)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.1f;
            }

        }
        else if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Fishing)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance > originalCameraDistance)
            {
                cameraDistance -= 0.01f;
            }

        }
    }
}


