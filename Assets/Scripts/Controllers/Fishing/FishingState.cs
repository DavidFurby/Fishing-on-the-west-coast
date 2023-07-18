using System.Collections.Generic;
using UnityEngine;

public abstract class FishingState
{
    protected FishingSystem system;

    public FishingState(FishingSystem system)
    {
        this.system = system;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate()
    {

    }
}

public class Idle : FishingState
{
    public Idle(FishingSystem system) : base(system)
    {
    }

    public override void OnEnter()
    {

        base.OnEnter();
        system.seaSpawner.RemoveAllFishes();

    }
    public override void Update()
    {
        base.Update();
        system.StartFishing();
    }
}

public class Charging : FishingState
{
    public Charging(FishingSystem system) : base(system)
    {
    }

    public override void Update()
    {
        base.Update();
        system.StartFishing();
        system.fishingMiniGame.ChargingBalance();
    }
}

public class Casting : FishingState
{
    public Casting(FishingSystem system) : base(system)
    {

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.Cast();
    }
}

public class Fishing : FishingState
{
    public Fishing(FishingSystem system) : base(system)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.seaSpawner.InvokeSpawnFish();

    }
    public override void Update()
    {
        base.Update();
        system.StartReeling();
        system.baitLogic.Shake();
    }
}

public class Reeling : FishingState
{
    public Reeling(FishingSystem controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.ReelIn();
    }

}

public class ReelingFish : FishingState
{
    public ReelingFish(FishingSystem controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void Update()
    {
        base.Update();
        system.fishingMiniGame.CalculateBalance();
        system.fishingMiniGame.BalanceLost();
        system.fishingMiniGame.BalanceControls();
        system.fishingMiniGame.HandleBalanceColor();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.ReelIn();
    }
}

public class InspectFish : FishingState
{
    public InspectFish(FishingSystem controller) : base(controller)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.catchSummary.InitiateCatchSummary();
    }
    public override void Update()
    {
        base.Update();
        system.catchSummary.NextSummary();
    }
    public override void OnExit()
    {
        base.OnExit();
        system.catchSummary.EndSummary();
    }
}

public abstract class FishingStateMachine : MonoBehaviour
{
    private FishingState currentState;
    public FishingState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(FishingState state)
    {
        currentState?.OnExit();

        currentState = state;
        currentState.OnEnter();
    }

    void Update()
    {
        currentState?.Update();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
