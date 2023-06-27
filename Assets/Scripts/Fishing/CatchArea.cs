using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
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

    public Fish fish;
    private FishMovement fishMovement;
    [SerializeField] private FishingControlls fishingControlls;
    public readonly List<Fish> totalFishes = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fish == null)
        {
            IsInCatchArea = true;
            other.gameObject.TryGetComponent(out fishMovement);
            other.gameObject.TryGetComponent(out fish);
        }
        else if (fishingControlls != null && fishingControlls.fishingStatus == FishingControlls.FishingStatus.ReelingFish && other.CompareTag("Fish") && fish != null)
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
        if (fishingControlls != null)
        {
            StartCoroutine(fishingControlls.CatchAlert());
            GameObject newFish = other.gameObject;
            if (newFish.TryGetComponent(out FishMovement newFishMovement))
            {
                if (totalFishes.Count > 0)
                {
                    newFishMovement.GetBaited(totalFishes[totalFishes.Count - 1].gameObject);
                }
                newFishMovement.SetFishState(FishMovement.FishState.Hooked);
                if (newFish.TryGetComponent(out Fish newFishComponent))
                {
                    totalFishes.Add(newFishComponent);
                }
                fishingControlls.CalculateReelInSpeed();
            }
        }
    }

    //Catch fish while fishing
    public void CatchFish()
    {
        if (fish != null && fishMovement != null && fishMovement.state != FishMovement.FishState.Hooked)
        {
            fishMovement.SetFishState(FishMovement.FishState.Hooked);
            totalFishes.Add(fish.GetComponent<Fish>());
        }
    }

    //Reset the values if the catch is lost
    public void RemoveCatch()
    {
        if (fish != null)
        {
            fish.DestroyFish();
        }
        fishMovement = null;
        fish = null;
        IsInCatchArea = false;
        totalFishes.Clear();
    }
}