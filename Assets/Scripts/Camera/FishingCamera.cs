using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private FishingController controller;
    private float cameraDistance;
    private float originalCameraDistance;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
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
            FishingController.OnCastingCamera += CastingCamera;
            FishingController.OnFishingCamera += FishCamera;
            FishingController.OnEndSummary += OnEndSummary;
            FishingController.OnReelingCamera += ReelingCamera;
            FishingController.OnStartReelingFish += OnStartReelingFish;
            CatchArea.OnCatchWhileReeling += OnCatchWhileReeling;
        }
        else
        {
            FishingController.OnCastingCamera -= CastingCamera;
            FishingController.OnFishingCamera -= FishCamera;
            FishingController.OnEndSummary -= OnEndSummary;
            FishingController.OnReelingCamera -= ReelingCamera;
            FishingController.OnStartReelingFish -= OnStartReelingFish;
            CatchArea.OnCatchWhileReeling -= OnCatchWhileReeling;
        }
    }

    public void CastingCamera() {
        SetCameraToTarget(controller.baitLogic.transform);
        MoveCameraCloserToTarget(controller.baitLogic.transform);
    }
    
    public void FishCamera() {
        SetCameraToTarget(controller.baitLogic.transform);
        MoveCameraToOriginal();
    }
    
    public void ReelingCamera() {
        SetCameraToTarget(controller.baitLogic.transform);
        MoveCameraCloserToTarget(controller.baitLogic.transform);
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
        if (audioSource != null)
            audioSource.Play();
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
        MoveCameraToOriginal();
    }

    private void OnStartReelingFish()
    {
        StartCoroutine(PlayCatchAlertSoundAndPauseGame());
    }

    private void OnCatchWhileReeling()
    {
        StartCoroutine(PlayCatchAlertSoundAndPauseGame());
    }
}
