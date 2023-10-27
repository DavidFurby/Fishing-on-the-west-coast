using System.Collections.Generic;
using UnityEngine;

public class FishingController : PlayerEventController
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

    #endregion

    #region Public Methods

    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (FishInCatchArea != null)
            {
                RaiseEnterReelingFish();
            }
            else
            {
                SetState(new Reeling());
            }
        }
    }

    public void StartCharging()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SetState(new Charging());
        }
    }

    public void Release()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetState(new Swinging());
        }
    }

    public void EnterSea()
    {
        if (GetCurrentState() is Casting)
        {
            SetState(new Fishing());
        }
    }

    public void CatchFish()
    {
        if (GetCurrentState() is Fishing && FishInCatchArea != null)
        {
            FishAttachedToBait = FishInCatchArea;
            FishAttachedToBait.GetComponent<FishBehaviour>().SetState(new Hooked(FishAttachedToBait.GetComponent<FishBehaviour>()));
            AddFish(FishAttachedToBait);
        }
    }

    public void LoseCatch()
    {
        ResetValues();
        SetState(new Reeling());
    }

    public void AddFish(FishDisplay fish)
    {
        fishesOnHook.Add(fish);
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
