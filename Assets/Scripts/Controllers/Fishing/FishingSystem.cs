using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishingSystem : FishingStateMachine
{
    #region Serialized Fields
    [Header("Fishing Components")]
    public FishingCamera fishingCamera;
    public FishingMiniGame fishingMiniGame;
    public FishingRodLogic fishingRodLogic;
    public BaitLogic baitLogic;
    #endregion

    #region Events
    public static event Action<bool> OnChargeRelease;
    public static event Action<bool> OnStartCharging;
    public static event Action OnRemoveFishes;
    public static event Action OnStartFishing;
    public static event Action OnStartInspecting;
    public static event Action OnNextSummary;
    public static event Action OnEndSummary;


    #endregion

    [HideInInspector] public FishDisplay FishAttachedToBait { get; set; }
    [HideInInspector] public List<FishDisplay> fishesOnHook = new();

    [HideInInspector] public bool IsInCatchArea { get; set; }

    [HideInInspector] public bool FishIsBaited { get; set; }

    private void Start()
    {
        SetState(new NotFishing(this));
        FishingSpot.StartFishing += RaiseStartFishing;
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
                StartCoroutine(fishingCamera.CatchAlert());
                fishingMiniGame.StartBalanceMiniGame();
                CatchFish();
                baitLogic.ReelIn();
                SetState(new ReelingFish(this));
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
            OnStartCharging.Invoke(true);
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
    private void RaiseStartFishing()
    {
        SetState(new FishingIdle(this));
    }
    public void RaiseRemoveFishes()
    {
        OnRemoveFishes.Invoke();
    }
    public void RaiseSpawnFishes()
    {
        OnStartFishing.Invoke();
    }
    public void RaiseChargeRelease()
    {
        OnChargeRelease.Invoke(false);
    }
    public void RaiseInitiateCatchSummary()
    {
        OnStartInspecting.Invoke();
    }
    public void RaiseNextSummary()
    {
        OnNextSummary.Invoke();
    }
    public void RaiseEndSummary()
    {
        OnEndSummary.Invoke();
    }
}
#endregion

