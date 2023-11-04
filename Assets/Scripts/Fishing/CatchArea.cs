using System;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public static event Action<Collider, GameObject> OnBaitFish;
    public static event Action OnCatchWhileReeling;
    private Collider _collider;

    private void OnEnable()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        PlayerEventController.OnEnterFishing += SetTriggerActive;
        PlayerEventController.OnEnterIdle += SetTriggerInactive;
    }

    private void OnDestroy()
    {
        PlayerEventController.OnEnterFishing -= SetTriggerActive;
        PlayerEventController.OnEnterIdle -= SetTriggerInactive;
    }


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

    private void SetTriggerActive()
    {
        _collider.enabled = true;
    }
    private void SetTriggerInactive()
    {
        _collider.enabled = false;
    }

    private void HandleFishEnter(Collider other)
    {
        FishController fishController = other.GetComponent<FishController>();
        if (fishController.GetCurrentState() is Baited)
        {
            if (other.TryGetComponent(out FishDisplay fish))
            {
                PlayerController.Instance.FishInCatchArea = fish;
            }
        }
    }

    private void CatchFishWhileReelingState(Collider other)
    {
        FishController fishController = other.GetComponent<FishController>();
        OnBaitFish.Invoke(other, PlayerController.Instance.fishesOnHook[^1].gameObject);
        if (fishController.GetCurrentState() is Baited)
        {
            OnCatchWhileReeling.Invoke();
            fishController.SetState(new HookedToFish(fishController));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                PlayerController.Instance.AddFish(newFishComponent);
            }
        }
    }
}
