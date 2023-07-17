using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    [SerializeField] FishingController fishingControlls;
    [SerializeField] GameObject bait;
    private float cameraDistance;
    private float originalCameraDistance;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
    }

    void LateUpdate()
    {
        if (fishingControlls.GetCurrentState() is Casting)
        {
            UpdateCameraPosition();
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.1f;
            }
        }
        else if (fishingControlls.GetCurrentState() is Fishing)
        {
            UpdateCameraPosition();
            if (cameraDistance > originalCameraDistance)
            {
                cameraDistance -= 0.01f;
            }
        }
        else if (fishingControlls.GetCurrentState() is ReelingFish)
        {
            UpdateCameraPosition();
            if (cameraDistance < bait.transform.position.z - 2)
            {
                cameraDistance += 0.2f;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        transform.position = new Vector3(bait.transform.position.x, bait.transform.position.y, cameraDistance);
    }

    public void CatchAlertSound()
    {
        audioSource.Play();
    }
}