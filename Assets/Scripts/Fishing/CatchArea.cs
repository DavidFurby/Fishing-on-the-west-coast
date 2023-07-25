using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool IsInCatchArea { get; set; }
    [HideInInspector] public FishDisplay fish;
    [SerializeField] private FishingSystem fishingSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (fishingSystem.GetCurrentState() is ReelingFish)
            {
                CatchFishWhileReeling(other);
            }
            else
            {
                HandleFishEnter(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject.GetComponent<FishDisplay>())
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
                this.fish = fish;
            }
        }
    }



    private void CatchFishWhileReeling(Collider other)
    {
        if (other.TryGetComponent(out FishMovement newFishMovement))
        {
            StartCoroutine(fishingSystem?.fishingCamera.CatchAlert());
            newFishMovement.GetBaited(fishingSystem.caughtFishes[^1].gameObject);
            newFishMovement.SetState(new HookedToFish(newFishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                fishingSystem.AddFish(newFishComponent);
            }
            fishingSystem.fishingRodLogic.CalculateReelInSpeed();
        }
    }

    public void CatchFish()
    {
        if (fishingSystem.GetCurrentState() is Fishing)
        {
            fish.GetComponent<FishMovement>().SetState(new Hooked(fish.GetComponent<FishMovement>()));
            fishingSystem.AddFish(fish);
        }
    }

    public void ResetValues()
    {
        fish = null;
        IsInCatchArea = false;
    }
}
