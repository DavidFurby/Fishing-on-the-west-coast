using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] GameObject bait;
    private float cameraDistance;
    private float originalCameraDistance;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Casting)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.1f;
            }

        }
        else if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Fishing)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance > originalCameraDistance)
            {
                cameraDistance -= 0.01f;
            }

        }
        else if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Reeling)
        {

            transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.2f;
            }

        }
    }
    public void CatchAlert()
    {
        audioSource.Play();
    }
}


