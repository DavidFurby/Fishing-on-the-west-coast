using System.Collections;
using UnityEngine;

public class FishingCamera : MonoBehaviour
{
    [SerializeField] private FishingController system;
    [SerializeField] private AudioSource audioSource;

    private float cameraDistance;
    private float originalCameraDistance;

    private void Start()
    {
        cameraDistance = transform.position.z;
        originalCameraDistance = cameraDistance;
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        FishingController.OnCastingCamera += SetCameraToBait;
        FishingController.OnCastingCamera += () => MoveCameraCloserToBait();
        FishingController.OnFishingCamera += SetCameraToBait;
        FishingController.OnFishingCamera += () => MoveCameraToOriginal();
        FishingController.OnEndSummary += () => MoveCameraToOriginal();
        FishingController.OnReelingCamera += SetCameraToBait;
        FishingController.OnReelingCamera += () => MoveCameraCloserToBait();
        FishingController.OnStartReelingFish += () => StartCoroutine(CatchAlert());
        CatchArea.OnCatchWhileReeling += () => StartCoroutine(CatchAlert());
    }

    private void UnsubscribeFromEvents()
    {
        FishingController.OnCastingCamera -= SetCameraToBait;
        FishingController.OnCastingCamera -= () => MoveCameraCloserToBait();
        FishingController.OnFishingCamera -= SetCameraToBait;
        FishingController.OnFishingCamera -= () => MoveCameraToOriginal();
        FishingController.OnEndSummary -= () => MoveCameraToOriginal();
        FishingController.OnReelingCamera -= SetCameraToBait;
        FishingController.OnReelingCamera -= () => MoveCameraCloserToBait();
        FishingController.OnStartReelingFish -= () => StartCoroutine(CatchAlert());
        CatchArea.OnCatchWhileReeling -= () => StartCoroutine(CatchAlert());
    }

    public void SetCameraToBait()
    {
        if (gameObject != null && system.baitLogic != null)
            gameObject.transform.position = new Vector3(system.baitLogic.transform.position.x, system.baitLogic.transform.position.y, cameraDistance);
    }

    public void SetCameraToFish(FishDisplay fish)
    {
        if (fish != null)
            transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, cameraDistance);
    }

    public void MoveCameraCloserToBait(float speed = 0.1f)
    {
        if (system.baitLogic != null && cameraDistance < system.baitLogic.transform.position.z - 2)
            cameraDistance = Mathf.MoveTowards(cameraDistance, system.baitLogic.transform.position.z - 2, speed);
    }

    public void MoveCameraToOriginal(float speed = 0.01f)
    {
        if (cameraDistance > originalCameraDistance)
            cameraDistance = Mathf.MoveTowards(cameraDistance, originalCameraDistance, speed);
    }

    public void MoveCameraCloserToFish(FishDisplay fish, float speed = 0.1f)
    {
        if (fish != null && cameraDistance < fish.transform.position.z)
            cameraDistance = Mathf.MoveTowards(cameraDistance, fish.transform.position.z, speed);
    }

    public void CatchAlertSound()
    {
        if (audioSource != null)
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
