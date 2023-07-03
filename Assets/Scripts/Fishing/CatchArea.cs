using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an area where fish can be caught.
/// </summary>
public class CatchArea : MonoBehaviour
{
    /// <summary>
    /// Gets a value indicating whether a fish is in the catch area.
    /// </summary>
    public bool IsInCatchArea { get; private set; }
    public readonly List<Catch> totalCatches = new();
    private FishMovement fishMovement;
    [HideInInspector] public Catch fish;
    [SerializeField] private FishingController fishingControlls;
    [SerializeField] private FishingRodLogic FishingRodLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fish == null)
        {
            IsInCatchArea = true;
            if (other.GetComponent<FishMovement>().state == FishMovement.FishState.Baited)
            {
                if (other.TryGetComponent(out FishMovement fishMovement))
                {
                    this.fishMovement = fishMovement;
                }
                if (other.TryGetComponent(out Catch fish))
                {
                    this.fish = fish;
                }
            }

        }
        else if (fishingControlls != null && fishingControlls.fishingStatus == FishingController.FishingStatus.ReelingFish && other.CompareTag("Fish") && fish != null)
        {
            CatchFishWhileReeling(other);
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

    //Automatically catch another fish if it collides while reeling another fish
    private void CatchFishWhileReeling(Collider other)
    {
        StartCoroutine(fishingControlls?.CatchAlert());
        if (other.TryGetComponent(out FishMovement newFishMovement))
        {
            if (totalCatches.Count > 0)
            {
                newFishMovement.GetBaited(totalCatches[totalCatches.Count - 1].gameObject);
            }
            newFishMovement.SetFishState(FishMovement.FishState.Hooked);
            if (other.TryGetComponent(out Catch newFishComponent))
            {
                totalCatches.Add(newFishComponent);
            }
            FishingRodLogic.CalculateReelInSpeed();
        }

    }

    //Catch fish while fishing
    public void CatchFish()
    {
        if (fish != null && fishMovement != null && fishMovement.state != FishMovement.FishState.Hooked)
        {
            fishMovement.SetFishState(FishMovement.FishState.Hooked);
            totalCatches.Add(fish.GetComponent<Catch>());
        }
    }

    //Reset the values if the catch is lost
    public void RemoveCatch()
    {

        fish?.DestroyFish();

        fishMovement = null;
        fish = null;
        IsInCatchArea = false;
        totalCatches.Clear();
    }
}