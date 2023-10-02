using System;
using UnityEngine;

public abstract class PlayerEventController : PlayerStateMachine
{
    #region Events
    public static event Action OnStartCharging;
    public static event Action OnEnterSwinging;
    public static event Action OnEnterIdle;
    public static event Action OnEnterFishing;
    public static event Action OnEnterCasting;
    public static event Action OnReelingFish;
    public static event Action OnEnterSummary;
    public static event Action OnNextSummary;
    public static event Action OnEndSummary;
    public static event Action OnEnterReeling;
    public static event Action OnEnterReelingFish;
    public static event Action OnWhileCharging;
    public static event Action OnExitReelingFish;
    public static event Action OnWhileFishing;
    public static event Action<Transform> OnWhileCasting;
    public static event Action OnWhileReelingBait;

    #endregion
    internal void RaiseOnEnterCasting()
    {
        OnEnterCasting.Invoke();
    }
    internal void RaiseWhileCasting()
    {
        OnWhileCasting.Invoke(transform);
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

    internal void RaiseEnterCharging()
    {
        OnStartCharging?.Invoke();
    }
    internal void RaiseWhileCharging()
    {
        OnWhileCharging?.Invoke();
    }
    internal void RaiseEnterSwinging()
    {
        OnEnterSwinging?.Invoke();
    }
    internal void RaiseEnterInspecting()
    {
        OnEnterSummary?.Invoke();
    }

    internal void RaiseNextSummary()
    {
        OnNextSummary?.Invoke();
    }

    internal void RaiseEndSummary()
    {
        OnEndSummary?.Invoke();
    }

    internal void RaiseOnExitReelingFish()
    {
        OnExitReelingFish?.Invoke();
    }
}

