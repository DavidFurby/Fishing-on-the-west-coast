using UnityEngine;

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

        // Check if the FishingCamera script is already attached
        if (!TryGetComponent<FishingCamera>(out fishingCamera))
        {
            // If not, add it at runtime
            fishingCamera = this.gameObject.AddComponent<FishingCamera>();
        }

        // Check if the ExplorationCamera script is already attached
        if (!TryGetComponent<ExplorationCamera>(out explorationCamera))
        {
            // If not, add it at runtime
            explorationCamera = this.gameObject.AddComponent<ExplorationCamera>();
        }
    }

    void OnDisable()
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
