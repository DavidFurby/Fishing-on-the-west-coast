using UnityEngine;

[RequireComponent(typeof(FishingCamera))]
[RequireComponent(typeof(ExplorationCamera))]
public class CameraController : CameraStateMachine
{
    public static CameraController Instance { get; private set; }
    internal FishingCamera fishingCamera;
    internal ExplorationCamera explorationCamera;
    internal float cameraDistance;
    internal readonly float originalCameraDistance = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        cameraDistance = originalCameraDistance;

    }
    private void OnEnable()
    {
        FishingEventController.OnEnterFishing += EnterFishingState;
        FishingEventController.OnEnterReelingFish += EnterReelingFishState;
        FishingEventController.OnEnterCasting += EnterCastingState;
        FishingEventController.OnEnterInspecting += EnterPlayerState;
        FishingEventController.OnEnterReeling += EnterPlayerState;
    }

    void Start()
    {
        SetState(new PlayerCamera(this));

        fishingCamera = GetComponent<FishingCamera>();
        explorationCamera = GetComponent<ExplorationCamera>();
    }

    void OnDestroy()
    {
        FishingEventController.OnEnterFishing -= EnterFishingState;
        FishingEventController.OnEnterReelingFish -= EnterReelingFishState;
        FishingEventController.OnEnterCasting -= EnterCastingState;
        FishingEventController.OnEnterInspecting -= EnterPlayerState;
        FishingEventController.OnEnterReeling -= EnterPlayerState;

    }
    public void SetCameraToTarget(Transform target, float YValue = 0)
    {
        if (target != null)
            gameObject.transform.position = new Vector3(target.position.x, target.position.y + YValue, cameraDistance);
    }

    public void MoveCameraToTarget(Transform target, float speed = 0.1f, float distanceFrom = 2)
    {
        if (target != null && cameraDistance != target.position.z - distanceFrom)
            cameraDistance = Mathf.MoveTowards(cameraDistance, target.position.z - distanceFrom, speed);
    }

    public void MoveCameraToOriginal(float speed = 0.01f)
    {
        if (cameraDistance != originalCameraDistance)
            cameraDistance = Mathf.MoveTowards(cameraDistance, originalCameraDistance, speed);
    }

    public void EnterPlayerState()
    {
        SetState(new PlayerCamera(this));
    }
    public void EnterCastingState()
    {
        SetState(new CastingBaitCamera(this));
    }

    public void EnterFishingState()
    {
        SetState(new FishingBaitCamera(this));
    }

    public void EnterReelingFishState()
    {
        SetState(new ReelingBaitCamera(this));
    }
}
