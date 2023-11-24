using System;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    #region Internal Properties

    internal FishDisplay FishAttachedToBait { get; set; }
    internal FishDisplay FishInCatchArea { get; set; }
    internal FishDisplay BaitedFish { get; set; }
    internal List<FishDisplay> fishesOnHook = new();
    internal float chargeLevel = 1;
    internal float reelInSpeed;
    internal float castingPower;
    internal float initialCastingPower = 20;
    internal float initialReelInSpeed = 5f;
    public static event Action OnAddFishToHook;
    protected PlayerManager manager;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }

    #endregion

    #region Public Methods

    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (FishInCatchArea != null)
            {
                manager.RaiseEnterReelingFish();
            }
            else
            {
                manager.SetState(new Reeling());
            }
        }
    }

    public void StartCharging()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            manager.SetState(new Charging());
        }
    }

    public void Release()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            manager.SetState(new Swinging());
        }
    }

    public void EnterSea()
    {
        if (manager.GetCurrentState() is Casting)
        {
            manager.SetState(new Fishing());
        }
    }

    public void CatchFish()
    {
        if (manager.GetCurrentState() is Fishing && FishInCatchArea != null)
        {
            FishAttachedToBait = FishInCatchArea;
            FishAttachedToBait.GetComponent<FishController>().SetState(new Hooked(FishAttachedToBait.GetComponent<FishController>()));
            AddFish(FishAttachedToBait);
        }
    }

    public void LoseCatch()
    {
        ResetValues();
        manager.SetState(new Reeling());
    }

    public void AddFish(FishDisplay fish)
    {
        fishesOnHook.Add(fish);
        OnAddFishToHook.Invoke();
    }

    public void ChargeCasting()
    {
        if (castingPower < MainManager.Instance.Inventory.EquippedRod.throwRange)
        {
            castingPower++;
        }
    }

    public void ResetValues()
    {
        ClearCaughtFishes();
        FishAttachedToBait = null;
        FishInCatchArea = null;
        BaitedFish = null;
        reelInSpeed = initialReelInSpeed;
        castingPower = initialCastingPower;
        chargeLevel = 1;
    }

    #endregion

    #region Private Methods

    private void ClearCaughtFishes()
    {
        foreach (FishDisplay fish in fishesOnHook)
        {
            fish.ReturnToPool();
        }
        fishesOnHook.Clear();
    }

    #endregion
}
