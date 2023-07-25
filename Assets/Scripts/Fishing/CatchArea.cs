using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool IsInCatchArea { get; set; }
    [HideInInspector] public FishDisplay Fish { get; set; }
    [SerializeField] private FishingSystem fishingGameSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (fishingGameSystem.GetCurrentState() is ReelingFish)
            {
                CatchFishDuringReelingState(other);
            }
            else
            {
                HandleFishEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && Fish == other.GetComponent<FishDisplay>())
        {
            ResetValues();
        }
    }

    private void HandleFishEnter(Collider other)
    {
        if (other.GetComponent<FishMovement>().GetCurrentState() is Baited)
        {
            IsInCatchArea = true;
            if (other.TryGetComponent(out FishDisplay fish))
            {
                Fish = fish;
            }
        }
    }

    private void CatchFishDuringReelingState(Collider other)
    {
        if (other.TryGetComponent(out FishMovement newFishMovement) && newFishMovement.GetCurrentState() is Baited)
        {
            StartCoroutine(fishingGameSystem?.fishingCamera.CatchAlert());
            newFishMovement.SetState(new HookedToFish(newFishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                fishingGameSystem.AddFish(newFishComponent);
            }
            fishingGameSystem.fishingRodLogic.CalculateReelInSpeed();
        }
    }

    public void CatchFish()
    {
        if (fishingGameSystem.GetCurrentState() is Fishing)
        {
            Fish.GetComponent<FishMovement>().SetState(new Hooked(Fish.GetComponent<FishMovement>()));
            fishingGameSystem.AddFish(Fish);
        }
    }

    public void ResetValues()
    {
        Fish = null;
        IsInCatchArea = false;
    }
}
