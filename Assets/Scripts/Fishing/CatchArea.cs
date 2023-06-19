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

    //Automatically catch another fish if it collides while reeling another fish
    private void CatchFishWhileReeling(Collider other)
    {
        GameObject newFish = other.gameObject;
        FishMovement newFishMovement = newFish.GetComponent<FishMovement>();
        newFishMovement.GetBaited(totalFishes[totalFishes.Count - 1].gameObject);
        newFishMovement.SetFishState(FishMovement.FishState.Hooked);
        totalFishes.Add(newFish.GetComponent<Fish>());
        fishingControlls.CatchAlert();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject)
        {
            isInCatchArea = false;
            fish = null;
        }
    }

    public void CatchFish()
    {
        if (fish != null && fishMovement != null && fishMovement.state != FishMovement.FishState.Hooked)
        {
            fishMovement.SetFishState(FishMovement.FishState.Hooked);
            totalFishes.Add(fish.GetComponent<Fish>());
        }
    }
    public void RemoveCatch()
    {
        fish.DestroyFish();
        fishMovement = null;
        fish = null;
        isInCatchArea = false;
    }
}