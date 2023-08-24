using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : FishingStateMachine
{
    #region Serialized Fields
    [Header("Fishing Components")]
    public FishingMiniGame fishingMiniGame;
    public FishingRodLogic fishingRodLogic;
    public BaitLogic baitLogic;
    #endregion

    #region Events
    public static event Action OnChargeRelease;
    public static event Action OnStartCharging;
    public static event Action OnRemoveFishes;
    public static event Action OnStartFishing;
    public static event Action OnReelingFish;
    public static event Action OnStartInspecting;
    public static event Action OnNextSummary;
    public static event Action OnEndSummary;
    public static event Action OnCastingCamera;
    public static event Action OnFishingCamera;
    public static event Action OnReelingCamera;
    public static event Action OnStartReeling;
    #endregion

    [HideInInspector] public FishDisplay FishAttachedToBait { get; set; }
    [HideInInspector] public List<FishDisplay> fishesOnHook = new();

    [HideInInspector] public bool IsInCatchArea { get; set; }

    [HideInInspector] public bool FishIsBaited { get; set; }

    private void Start()
    {
        SetState(new NotFishing(this));
        FishingSpot.StartFishing += () => SetState(new FishingIdle(this));
        OnStartReeling += CatchFish;
        OnStartReeling += () => SetState(new ReelingFish(this));
    }

    #region Public Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsInCatchArea && FishAttachedToBait != null)
            {
                OnStartReeling.Invoke();
            }
            else
            {
                fishingRodLogic.ReelInSpeed();
                SetState(new Reeling(this));
            }
        }
    }

    /// <summary>
    /// Starts fishing if the space key is held down.
    /// </summary>
    public void StartCharging()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnStartCharging.Invoke();
            SetState(new Charging(this));
        }
    }
    public void Charge()
    {
        fishingRodLogic.ChargeCasting();
    }
    public void Release()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetState(new Swinging(this));
        }
    }
    public void CatchFish()
    {
        if (GetCurrentState() is Fishing)
        {
            FishAttachedToBait.GetComponent<FishMovement>().SetState(new Hooked(FishAttachedToBait.GetComponent<FishMovement>()));
            AddFish(FishAttachedToBait);
        }
    }
    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (fishesOnHook.Count > 0)
        {
            SetState(new InspectFish(this));
        }
    }

    //Drop the fish if you fail the mini game
    public void LoseCatch()
    {
        ResetValues();
        SetState(new Reeling(this));
    }

    //Add fish to totalFishes list
    public void AddFish(FishDisplay fish)
    {
        fishesOnHook.Add(fish);
    }

    public void ClearCaughtFishes()
    {
        foreach (var fish in fishesOnHook)
        {
            fish.ReturnToPool();
        }
        fishesOnHook.Clear();
    }
    public void ResetValues()
    {
        ClearCaughtFishes();
        FishAttachedToBait = null;
        IsInCatchArea = false;
        FishIsBaited = false;
    }
    public void RaiseRemoveFishes()
    {
        OnRemoveFishes?.Invoke();
    }
    public void RaiseSpawnFishes()
    {
        OnStartFishing?.Invoke();
    }
    public void RaiseChargeRelease()
    {
        OnChargeRelease?.Invoke();
    }
    public void RaiseInitiateCatchSummary()
    {
        OnStartInspecting?.Invoke();
    }
    public void RaiseNextSummary()
    {
        OnNextSummary?.Invoke();
    }
    public void RaiseEndSummary()
    {
        OnEndSummary?.Invoke();
    }
    public void RaiseCastingCamera()
    {
        OnCastingCamera?.Invoke();
    }
    public void RaiseFishingCamera()
    {
        OnFishingCamera?.Invoke();
    }
    public void RaiseReelingCamera()
    {
        OnReelingCamera?.Invoke();
    }
    public void RaiseReelingFish() {
        OnReelingFish?.Invoke();
    }
}
#endregion

