using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    [SerializeField] private FishingSystem system;
    [SerializeField] private AudioSource audioSource;

    private float cameraDistance;
    private float originalCameraDistance;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
    }

    public void SetCameraToBait()
    {
        transform.position = new Vector3(system.baitLogic.bait.transform.position.x, system.baitLogic.bait.transform.position.y, cameraDistance);
    }

        public void SetCameraToFish(FishDisplay fish)
    {
        transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, cameraDistance);
    }

    public void MoveCameraCloserToBait(float speed = 0.1f)
    {
        if (cameraDistance < system.baitLogic.transform.position.z - 2)
        {
            cameraDistance += speed;
        }
    }

    public void MoveCameraToOriginal(float speed = 0.01f)
    {
        if (cameraDistance > originalCameraDistance)
        {
            cameraDistance -= speed;
        }
    }
    public void MoveCameraCloserToFish(FishDisplay fish, float speed = 0.1f)
    {
        if (cameraDistance < fish.transform.position.z)
        {
            cameraDistance += speed;
        }
    }

    public void CatchAlertSound()
    {
        audioSource.Play();
    }

    public IEnumerator CatchAlert()
    {
        CatchAlertSound();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
    }
}
