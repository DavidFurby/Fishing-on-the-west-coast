using System;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public static event Action<Collider, GameObject> OnBaitFish;
    public static event Action OnCatchWhileReeling;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (FishingController.Instance.GetCurrentState() is ReelingFish)
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
        if (other.CompareTag("Fish") && FishingController.Instance.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            FishingController.Instance.IsInCatchArea = true;
        }
    }

    private void HandleFishEnter(Collider other)
    {
        var fishMovement = other.GetComponent<FishMovement>();
        if (fishMovement.GetCurrentState() is Baited)
        {
            FishingController.Instance.IsInCatchArea = true;
            if (other.TryGetComponent(out FishDisplay fish))
            {
                FishingController.Instance.FishAttachedToBait = fish;
            }
        }
    }

    private void CatchFishDuringReelingState(Collider other)
    {
        FishMovement fishMovement = other.GetComponent<FishMovement>();
        OnBaitFish.Invoke(other, FishingController.Instance.fishesOnHook[^1].gameObject);
        if (fishMovement.GetCurrentState() is Baited)
        {
            OnCatchWhileReeling.Invoke();
            fishMovement.SetState(new HookedToFish(fishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                FishingController.Instance.AddFish(newFishComponent);
            }
        }
    }
}
