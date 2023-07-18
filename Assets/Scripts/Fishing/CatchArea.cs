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
        IsInCatchArea = true;
        if (other.GetComponent<FishMovement>().GetCurrentState() is Baited)
        {
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
            IsInCatchArea = false;
            fish = null;
        }
    }

    private void CatchFishWhileReeling(Collider other)
    {
        StartCoroutine(fishingSystem?.baitCamera.CatchAlert());
        if (other.TryGetComponent(out FishMovement newFishMovement))
        {
            newFishMovement.GetBaited(fishingSystem.totalFishes[^1].gameObject);
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
            Debug.Log(fish);
            fish.GetComponent<FishMovement>().SetState(new Hooked(fish.GetComponent<FishMovement>()));
            fishingSystem.AddFish(fish);
        }
    }

    public void RemoveCatch()
    {
        fish?.DestroyFish();
        fish = null;
        IsInCatchArea = false;
    }
}
