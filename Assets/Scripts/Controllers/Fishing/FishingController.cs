using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : FishingEventController
{
    [HideInInspector] public FishDisplay FishAttachedToBait { get; set; }
    [HideInInspector] public List<FishDisplay> fishesOnHook = new();
    [HideInInspector] public float chargeLevel = 1;
    [HideInInspector] public float reelInSpeed;
    [HideInInspector] public float castingPower;
    [HideInInspector] public float initialCastingPower = 20;
    [HideInInspector] public float initialReelInSpeed = 15f;

    [HideInInspector] public bool IsInCatchArea { get; set; }

    [HideInInspector] public bool FishIsBaited { get; set; }

    private void Start()
    {
        SetState(new NotFishing(this));
    }

    void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        WaterCollision.OnEnterSea += EnterSea;
        FishingSpot.StartFishing += () => SetState(new FishingIdle(this));
        OnStartReelingFish += CatchFish;
        OnStartReelingFish += () => SetState(new ReelingFish(this));
        OnEnterIdle += ResetValues;
        OnWhileCharging += Release;
        OnWhileFishing += StartReeling;
        OnWhileCharging += ChargeCasting;

    }

    private void UnsubscribeFromEvents()
    {
        WaterCollision.OnEnterSea -= EnterSea;
        FishingSpot.StartFishing -= () => SetState(new FishingIdle(this));
        OnStartReelingFish -= CatchFish;
        OnStartReelingFish -= () => SetState(new ReelingFish(this));
        OnEnterIdle -= ResetValues;
        OnWhileCharging -= Release;
        OnWhileFishing -= StartReeling;
        OnWhileCharging -= ChargeCasting;

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
                RaiseStartReelingFish();
            }
            else
            {
                RaiseStartReeling();
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
            RaiseStartCharging();
            SetState(new Charging(this));
        }
    }

    /// <summary>
    /// Releases the fishing line when the space key is released.
    /// </summary>
    public void Release()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetState(new Swinging(this));
        }
    }
    public void EnterSea()
    {
        if (GetCurrentState() is Casting)
        {
            SetState(new Fishing(this));
        }
    }

    /// <summary>
    /// Catches a fish if it is attached to the bait.
    /// </summary>
    public void CatchFish()
    {
        if (GetCurrentState() is Fishing)
        {
            FishAttachedToBait.GetComponent<FishMovement>().SetState(new Hooked(FishAttachedToBait.GetComponent<FishMovement>()));
            AddFish(FishAttachedToBait);
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

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>
    /// <param name="setChargingThrowSpeed">The action to perform while charging the casting power.</param>
    public void ChargeCasting()
    {
        if (castingPower < MainManager.Instance.game.Inventory.EquippedFishingRod.throwRange)
        {
            castingPower++;
        }
    }
    public void ClearCaughtFishes()
    {
        foreach (FishDisplay fish in fishesOnHook)
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
        reelInSpeed = initialReelInSpeed;
        castingPower = initialCastingPower;
    }
}
#endregion
