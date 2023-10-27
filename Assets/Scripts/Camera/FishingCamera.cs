using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    private AudioSource alertSound;
    private BaitLogic bait;
    private int fishingCameraDistance = 5;


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
            PlayerEventController.OnEndSummary += OnEndSummary;
            PlayerEventController.OnEnterReelingFish += OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling += OnCatchWhileReeling;
            CatchArea.OnCatchWhileReeling += IncreaseFishingCameraDistance;

        }
        else
        {
            PlayerEventController.OnEndSummary -= OnEndSummary;
            PlayerEventController.OnEnterReelingFish -= OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling -= OnCatchWhileReeling;
            CatchArea.OnCatchWhileReeling -= IncreaseFishingCameraDistance;
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
        CameraController.Instance.MoveCameraToTarget(bait.transform, 0.005f, fishingCameraDistance);
    }

    internal void UpdateCameraDuringReeling()
    {
        CameraController.Instance.SetCameraToTarget(bait.transform);
        CameraController.Instance.MoveCameraToTarget(bait.transform);
    }

    private void IncreaseFishingCameraDistance()
    {
        if (fishingCameraDistance < 15)
        {
            fishingCameraDistance += 10;
            print(fishingCameraDistance);
        }
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
