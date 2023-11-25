using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    private AudioSource alertSound;
    private BaitLogic bait;
    private const int fishingCameraDistance = 5;


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

        }
        else
        {
            PlayerEventController.OnEndSummary -= OnEndSummary;
            PlayerEventController.OnEnterReelingFish -= OnEnterReelingFish;
            CatchArea.OnCatchWhileReeling -= OnCatchWhileReeling;
        }
    }

    internal void UpdateCameraDuringCasting()
    {
        CameraManager.Instance.movement.SetCameraToTarget(bait.transform.position);
        CameraManager.Instance.movement.MoveCameraToTarget(bait.transform.position);
    }

    internal void UpdateCameraDuringFishing()
    {
        CameraManager.Instance.movement.SetCameraToTarget(bait.transform.position);
        CameraManager.Instance.movement.MoveCameraToTarget(bait.transform.position, 0.005f, fishingCameraDistance);
    }

    internal void UpdateCameraDuringReeling()
    {
        CameraManager.Instance.movement.SetCameraToTarget(bait.transform.position);
        CameraManager.Instance.movement.MoveCameraToTarget(bait.transform.position);
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
        CameraManager.Instance.movement.MoveCameraToOriginal();
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
