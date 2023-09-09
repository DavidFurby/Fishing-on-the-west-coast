using UnityEngine;

[RequireComponent(typeof(FishingCamera))]
[RequireComponent(typeof(ExplorationCamera))]
public class CameraController : CameraStateMachine
{
    internal FishingCamera fishingCamera;
    internal ExplorationCamera explorationCamera;

    private void OnEnable()
    {
        FishingController.OnEnterFishing += EnterFishingState;
        FishingController.OnEnterReelingFish += EnterReelingFishState;
        FishingController.OnEnterCasting += EnterCastingState;
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
    }

    // These methods set the state of the camera during different actions.
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
