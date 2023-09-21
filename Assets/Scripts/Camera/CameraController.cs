using UnityEngine;

[RequireComponent(typeof(FishingCamera))]
[RequireComponent(typeof(ExplorationCamera))]
public class CameraController : CameraStateMachine
{
    public static CameraController Instance { get; private set; }

    internal FishingCamera fishingCamera;
    internal ExplorationCamera explorationCamera;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
    }
    private void OnEnable()
    {
        FishingEventController.OnEnterFishing += EnterFishingState;
        FishingEventController.OnEnterReelingFish += EnterReelingFishState;
        FishingEventController.OnEnterCasting += EnterCastingState;
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
        FishingController.OnEnterFishing -= EnterFishingState;
        FishingController.OnEnterReelingFish -= EnterReelingFishState;
        FishingController.OnEnterCasting -= EnterCastingState;
        FishingController.OnEnterReeling -= EnterPlayerState;
    }

    // These methods set the state of the camera during different actions.

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
