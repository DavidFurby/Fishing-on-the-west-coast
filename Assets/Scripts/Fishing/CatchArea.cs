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
    [HideInInspector] public FishDisplay fish;
    [SerializeField] private FishingController fishingControlls;
    [SerializeField] private FishingRodLogic FishingRodLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fish == null)
        {
            IsInCatchArea = true;
            if (other.GetComponent<FishMovement>().state == FishMovement.FishState.Baited)
            {
                if (other.TryGetComponent(out FishDisplay fish))
                {
                    this.fish = fish;
                }
            }

        }
        else if (fishingControlls.GetCurrentState() is ReelingFish && other.CompareTag("Fish") && fish != null)
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
            // Modified to use the totalFishes variable in the FishingState class
            if (fishingControlls.GetCurrentState().totalFishes.Count > 0)
            {
                newFishMovement.GetBaited(fishingControlls.GetCurrentState().totalFishes[^1].gameObject);
            }
            newFishMovement.SetFishState(FishMovement.FishState.Hooked);
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                // Modified to use the totalFishes variable in the FishingState class
                fishingControlls.GetCurrentState().AddFish(newFishComponent);
            }
            FishingRodLogic.CalculateReelInSpeed();
        }
    }


    //Catch fish while fishing
    public void CatchFish()
    {
        if (fish != null)
        {
            fish.GetComponent<FishMovement>().SetFishState(FishMovement.FishState.Hooked);
            // Modified to use the totalFishes variable in the FishingState class
            fishingControlls.GetCurrentState().totalFishes.Add(fish.GetComponent<FishDisplay>());
        }
    }

    //Reset the values if the catch is lost
    public void RemoveCatch()
    {

        fish?.DestroyFish();
        fish = null;
        IsInCatchArea = false;
        // Modified to use the totalFishes variable in the FishingState class
        fishingControlls.GetCurrentState().totalFishes.Clear();
    }
}
