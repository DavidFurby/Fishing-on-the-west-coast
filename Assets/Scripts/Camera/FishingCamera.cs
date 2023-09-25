using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    private AudioSource alertSound;
    private BaitLogic bait;


    private void Start()
    {
        bait = FindAnyObjectByType<BaitLogic>();
        alertSound = GetComponent<AudioSource>();
        SubscribeToEvents(true);
    }

    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }

    private void SubscribeToEvents(bool subscribe)
    {
        if (subscribe)
        {
            FishingEventController.OnEndSummary += OnEndSummary;
            FishingEventController.OnEnterReelingFish += OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling += OnCatchWhileReeling;
        }
        else
        {
            FishingEventController.OnEndSummary -= OnEndSummary;
            FishingEventController.OnEnterReelingFish -= OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling -= OnCatchWhileReeling;
        }
    }

    internal void UpdateCameraDuringCasting()
    {
        CameraController.Instance.SetCameraToTarget(bait.transform);
        CameraController.Instance.MoveCameraToTarget(bait.transform);
    }

    internal void UpdateCameraDuringFishing()
    {
        CameraController.Instance.SetCameraToTarget(bait.transform);
        CameraController.Instance.MoveCameraToTarget(bait.transform, 0.005f, 6);
    }

    internal void UpdateCameraDuringReeling()
    {
        CameraController.Instance.SetCameraToTarget(bait.transform);
        CameraController.Instance.MoveCameraToTarget(bait.transform);
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
    private void OnEndSummary()
    {
        CameraController.Instance.MoveCameraToOriginal();
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
