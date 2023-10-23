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
            if (PlayerController.Instance.GetCurrentState() is ReelingFish)
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
        if (other.CompareTag("Fish") && PlayerController.Instance.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            PlayerController.Instance.IsInCatchArea = false;
        }
    }

    private void HandleFishEnter(Collider other)
    {
        FishMovement fishMovement = other.GetComponent<FishMovement>();
        if (fishMovement.GetCurrentState() is Baited)
        {
            PlayerController.Instance.IsInCatchArea = true;
            if (other.TryGetComponent(out FishDisplay fish))
            {
                PlayerController.Instance.FishAttachedToBait = fish;
            }
        }
    }

    private void CatchFishDuringReelingState(Collider other)
    {
        FishMovement fishMovement = other.GetComponent<FishMovement>();
        OnBaitFish.Invoke(other, PlayerController.Instance.fishesOnHook[^1].gameObject);
        if (fishMovement.GetCurrentState() is Baited)
        {
            OnCatchWhileReeling.Invoke();
            fishMovement.SetState(new HookedToFish(fishMovement));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                PlayerController.Instance.AddFish(newFishComponent);
            }
        }
    }
}
