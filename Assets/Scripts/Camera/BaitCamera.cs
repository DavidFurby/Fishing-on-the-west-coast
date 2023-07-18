using System.Collections;
using UnityEngine;

public class BaitCamera : MonoBehaviour
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

    public void UpdateCameraPosition()
    {
        transform.position = new Vector3(system.baitLogic.transform.position.x, system.baitLogic.transform.position.y, cameraDistance);
    }

    public void MoveCameraCloserToBait(float speed = 0.1f)
    {
        if (cameraDistance < system.baitLogic.transform.position.z - 2)
        {
            cameraDistance += speed;
        }
    }

    public void MoveCameraAwayFromBait(float speed = 0.01f)
    {
        if (cameraDistance > originalCameraDistance)
        {
            cameraDistance -= speed;
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
