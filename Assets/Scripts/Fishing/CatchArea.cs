using UnityEngine;

public class CatchArea : MonoBehaviour
{
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
        if (other.CompareTag("Fish") && fishingGameSystem.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            fishingGameSystem.IsInCatchArea = true;
        }
    }

    private void HandleFishEnter(Collider other)
    {
        if (other.GetComponent<FishMovement>().GetCurrentState() is Baited)
        {
            fishingGameSystem.IsInCatchArea = true;
            if (other.TryGetComponent(out FishDisplay fish))
            {
                fishingGameSystem.FishAttachedToBait = fish;
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
}
