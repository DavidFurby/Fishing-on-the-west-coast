using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool IsInCatchArea { get; private set; }
    [HideInInspector] public FishDisplay fish;
    [SerializeField] private FishingSystem fishingSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (fish == null)
            {
                HandleFishEnter(other);
            }
            else if (fishingSystem.GetCurrentState() is ReelingFish)
            {
                CatchFishWhileReeling(other);
            }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject)
        {
            ResetValues();
        }
    }

    private void CatchFishWhileReeling(Collider other)
    {
        if (other.TryGetComponent(out FishMovement newFishMovement))
        {
            StartCoroutine(fishingSystem?.fishingCamera.CatchAlert());
            newFishMovement.GetBaited(fishingSystem.caughtFishes[^1].gameObject);
            newFishMovement.SetState(new Hooked(newFishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                fishingSystem.AddFish(newFishComponent);
            }
            fishingSystem.fishingRodLogic.CalculateReelInSpeed();
        }
    }

    public void CatchFish()
    {
        if (fish != null)
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
