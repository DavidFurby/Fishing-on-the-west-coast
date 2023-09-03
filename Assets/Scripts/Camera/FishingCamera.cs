using System.Collections;
using UnityEngine;
using static CameraState;

public class FishingCamera : MonoBehaviour
{
    private AudioSource alertSound;
    private BaitLogic bait;
    private float cameraDistance;
    private float originalCameraDistance;

    // This method is called when the script instance is being loaded.
    private void Start()
    {
        bait = FindAnyObjectByType<BaitLogic>();
        alertSound = GetComponent<AudioSource>();
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
        SubscribeToEvents(true);
    }

    // This function is called when the MonoBehaviour will be destroyed.
    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }

    // This method subscribes or unsubscribes to events based on the passed boolean value.
    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
           

            FishingController.OnEndSummary += OnEndSummary;
            FishingController.OnEnterReelingFish += OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling += OnCatchWhileReeling;
        }
        else
        {
         

            FishingController.OnEndSummary -= OnEndSummary;
            FishingController.OnEnterReelingFish -= OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling -= OnCatchWhileReeling;
        }
    }

    // These methods update the camera during different actions.
    internal void UpdateCameraDuringCasting()
    {
        SetCameraToTarget(bait.transform);
        MoveCameraCloserToTarget(bait.transform);
    }

    internal void UpdateCameraDuringFishing()
    {
        SetCameraToTarget(bait.transform);
        MoveCameraToOriginal();
    }

    internal void UpdateCameraDuringReeling()
    {
        SetCameraToTarget(bait.transform);
        MoveCameraCloserToTarget(bait.transform);
    }


    public void SetCameraToTarget(Transform target)
    {
        if (gameObject != null && target != null)
            gameObject.transform.position = new Vector3(target.position.x, target.position.y, cameraDistance);
    }

    public void MoveCameraCloserToTarget(Transform target, float speed = 0.1f)
    {
        if (target != null && cameraDistance < target.position.z - 2)
            cameraDistance = Mathf.MoveTowards(cameraDistance, target.position.z - 2, speed);
    }

    public void MoveCameraToOriginal(float speed = 0.01f)
    {
        if (cameraDistance > originalCameraDistance)
            cameraDistance = Mathf.MoveTowards(cameraDistance, originalCameraDistance, speed);
    }

    private void CatchAlertSound()
    {
        if (alertSound != null)
            alertSound.Play();
    }

    public IEnumerator PlayCatchAlertSoundAndPauseGame()
    {
        CatchAlertSound();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
    }

    // These methods update the camera during different actions.
    private void OnEndSummary()
    {
        MoveCameraToOriginal();
    }

    private void OnEnterReelingFish()
    {
        StartCoroutine(PlayCatchAlertSoundAndPauseGame());
    }

    private void OnCatchWhileReeling()
    {
        StartCoroutine(PlayCatchAlertSoundAndPauseGame());
    }
}
