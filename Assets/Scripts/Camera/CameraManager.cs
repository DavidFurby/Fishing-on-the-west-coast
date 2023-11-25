using UnityEngine;

[RequireComponent(typeof(FishingCamera))]
[RequireComponent(typeof(ExplorationCamera))]
[RequireComponent(typeof(CameraMovement))]
public class CameraManager : CameraStateMachine
{
    public static CameraManager Instance { get; private set; }
    internal FishingCamera fishing;
    internal ExplorationCamera exploration;
    internal CameraMovement movement;
    internal float cameraDistance;
    internal float originalCameraDistance = 0;

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
        PlayerEventController.OnEnterFishing += EnterFishingState;
        PlayerEventController.OnEnterReelingFish += EnterReelingFishState;
        PlayerEventController.OnEnterCasting += EnterCastingState;
        PlayerEventController.OnEnterSummary += EnterPlayerState;
        PlayerEventController.OnEnterReeling += EnterPlayerState;
        PlayerEventController.OnEnterCharacterDialog += EnterCharacterDialogState;

    }

    void Start()
    {
        SetState(new PlayerCamera());
        fishing = GetComponent<FishingCamera>();
        exploration = GetComponent<ExplorationCamera>();
        movement = GetComponent<CameraMovement>();
    }

    void OnDestroy()
    {
        PlayerEventController.OnEnterFishing -= EnterFishingState;
        PlayerEventController.OnEnterReelingFish -= EnterReelingFishState;
        PlayerEventController.OnEnterCasting -= EnterCastingState;
        PlayerEventController.OnEnterSummary -= EnterPlayerState;
        PlayerEventController.OnEnterReeling -= EnterPlayerState;
        PlayerEventController.OnEnterCharacterDialog -= EnterCharacterDialogState;
    }

    public void EnterPlayerState()
    {
        SetState(new PlayerCamera());
    }
    public void EnterCharacterDialogState()
    {
        SetState(new CharacterDialogCamera());
    }
    public void EnterCastingState()
    {
        SetState(new CastingBaitCamera());
    }

    public void EnterFishingState()
    {
        SetState(new FishingBaitCamera());
    }

    public void EnterReelingFishState()
    {
        SetState(new ReelingBaitCamera());
    }
}
