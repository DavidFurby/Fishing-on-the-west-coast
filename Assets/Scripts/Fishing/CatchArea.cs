using System.Collections.Generic;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    public Fish fish;
    private FishMovement fishMovement;
    [SerializeField] private FishingControlls fishingControlls;
    public List<Fish> totalFishes;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fish == null)
        {
            isInCatchArea = true;
            fishMovement = other.gameObject.GetComponent<FishMovement>();
            fish = other.gameObject.GetComponent<Fish>();
        }
        else if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.ReelingFish && other.CompareTag("Fish") && fish != null)
        {
            CatchFishWhileReeling(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject)
        {
            isInCatchArea = false;
            fish = null;
        }
    }

    //Automatically catch another fish if it collides while reeling another fish
    private void CatchFishWhileReeling(Collider other)
    {
        StartCoroutine(fishingControlls.CatchAlert());
        GameObject newFish = other.gameObject;
        FishMovement newFishMovement = newFish.GetComponent<FishMovement>();
        newFishMovement.GetBaited(totalFishes[^1].gameObject);
        newFishMovement.SetFishState(FishMovement.FishState.Hooked);
        totalFishes.Add(newFish.GetComponent<Fish>());
        fishingControlls.CalculateReelInSpeed();
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
        fish.DestroyFish();
        fishMovement = null;
        fish = null;
        isInCatchArea = false;
        totalFishes.Clear();
    }
}