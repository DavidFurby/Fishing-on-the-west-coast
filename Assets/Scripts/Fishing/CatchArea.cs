using System;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    [SerializeField] private FishingController fishController;
    public static event Action<Collider, GameObject> OnBaitFish;
    public static event Action OnCatchWhileReeling;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (fishController.GetCurrentState() is ReelingFish)
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
        if (other.CompareTag("Fish") && fishController.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            fishController.IsInCatchArea = true;
        }
    }

    private void HandleFishEnter(Collider other)
    {
        var fishMovement = other.GetComponent<FishMovement>();
        if (fishMovement.GetCurrentState() is Baited)
        {
            fishController.IsInCatchArea = true;
            if (other.TryGetComponent(out FishDisplay fish))
            {
                fishController.FishAttachedToBait = fish;
            }
        }
    }

    private void CatchFishDuringReelingState(Collider other)
    {
        FishMovement fishMovement = other.GetComponent<FishMovement>();
        OnBaitFish.Invoke(other, fishController.fishesOnHook[^1].gameObject);
        if (fishMovement.GetCurrentState() is Baited)
        {
            OnCatchWhileReeling.Invoke();
            fishMovement.SetState(new HookedToFish(fishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                fishController.AddFish(newFishComponent);
            }
        }
    }
}
