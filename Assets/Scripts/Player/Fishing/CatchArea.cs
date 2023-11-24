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
            if (PlayerManager.Instance.GetCurrentState() is ReelingFish)
            {
                CatchFishWhileReelingState(other);
            }
            else if (PlayerManager.Instance.GetCurrentState() is Fishing)
            {
                HandleFishEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && PlayerManager.Instance.fishingController.FishAttachedToBait == other.GetComponent<FishDisplay>())
        {
            PlayerManager.Instance.fishingController.FishInCatchArea = null;
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
                PlayerManager.Instance.fishingController.FishInCatchArea = fish;
            }
        }
    }

    private void CatchFishWhileReelingState(Collider other)
    {
        FishController fishController = other.GetComponent<FishController>();
        OnBaitFish.Invoke(other, PlayerManager.Instance.fishingController.fishesOnHook[^1].gameObject);
        if (fishController.GetCurrentState() is Baited)
        {
            OnCatchWhileReeling.Invoke();
            fishController.SetState(new HookedToFish(fishController));
            if (other.TryGetComponent(out FishDisplay newFishComponent))
            {
                PlayerManager.Instance.fishingController.AddFish(newFishComponent);
            }
        }
    }
}
