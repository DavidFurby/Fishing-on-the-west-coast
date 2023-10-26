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
                CatchFishWhileReelingState(other);
            }
            else if (PlayerController.Instance.GetCurrentState() is Fishing)
            {
                HandleFishEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && PlayerController.Instance.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            PlayerController.Instance.FishInCatchArea = null;
        }
    }

    private void HandleFishEnter(Collider other)
    {
        FishMovement fishMovement = other.GetComponent<FishMovement>();
        if (fishMovement.GetCurrentState() is Baited)
        {
            if (other.TryGetComponent(out FishDisplay fish))
            {
                PlayerController.Instance.FishInCatchArea = fish;
            }
        }
    }

    private void CatchFishWhileReelingState(Collider other)
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
