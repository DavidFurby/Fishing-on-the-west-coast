using System;
using UnityEngine;

public abstract class FishingEventController : FishingStateMachine
{
    #region Events
    public static event Action OnStartCharging;
    public static event Action OnChargeRelease;
    public static event Action OnEnterIdle;
    public static event Action OnEnterFishing;
    public static event Action OnEnterCasting;
    public static event Action OnReelingFish;
    public static event Action OnStartInspecting;
    public static event Action OnNextSummary;
    public static event Action OnEndSummary;
    public static event Action OnEnterReeling;
    public static event Action OnEnterReelingFish;
    public static event Action OnWhileCharging;
    public static event Action OnExitReelingFish;
    public static event Action OnWhileFishing;
    public static event Action OnWhileCasting;
    public static event Action OnWhileReelingBait;

    #endregion
    internal void RaiseOnEnterCasting()
    {
        OnEnterCasting.Invoke();
    }
    internal void RaiseWhileCasting()
    {
        OnWhileCasting.Invoke();
    }
    internal void RaiseReelInBait()
    {
        OnWhileReelingBait?.Invoke();
    }
    internal void RaiseWhileFishing()
    {
        OnWhileFishing?.Invoke();
    }

    internal void RaiseEnterReeling()
    {
        OnEnterReeling?.Invoke();
    }

    internal void RaiseEnterReelingFish()
    {
        OnEnterReelingFish?.Invoke();
    }

    internal void RaiseReelingFish()
    {
        OnReelingFish?.Invoke();
    }


    public void RaiseEnterIdle()
    {
        OnEnterIdle?.Invoke();
    }
    public void RaiseEnterFishing()
    {
        OnEnterFishing?.Invoke();
    }
    internal void RaiseChargeRelease()
    {
        OnChargeRelease?.Invoke();
    }

    internal void RaiseStartCharging()
    {
        OnStartCharging?.Invoke();
    }

    internal void RaiseStartInspecting()
    {
        OnStartInspecting?.Invoke();
    }

    internal void RaiseNextSummary()
    {
        OnNextSummary?.Invoke();
    }

    internal void RaiseEndSummary()
    {
        OnEndSummary?.Invoke();
    }
    internal void RaiseWhileCharging()
    {
        OnWhileCharging?.Invoke();
    }
    internal void RaiseOnExitReelingFish()
    {
        OnExitReelingFish?.Invoke();
    }
}

