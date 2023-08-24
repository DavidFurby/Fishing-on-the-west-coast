using System.Diagnostics;

public abstract class FishingState : State
{
    protected FishingController system;

    public FishingState(FishingController system)
    {
        this.system = system;
    }
}
public class NotFishing : FishingState
{
    public NotFishing(FishingController system) : base(system) { }
}

public class FishingIdle : FishingState
{
    public FishingIdle(FishingController system) : base(system)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseRemoveFishes();
        system.fishingRodLogic.ResetValues();
        system.ResetValues();
    }
    public override void Update()
    {
        base.Update();
        system.StartCharging();
    }
}

public class Charging : FishingState
{
    public Charging(FishingController system) : base(system)
    {
    }

    public override void Update()
    {
        base.Update();
        system.Charge();
        system.Release();
        system.RaiseWhileCharging();
    }
}
public class Swinging : FishingState
{
    public Swinging(FishingController system) : base(system)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseChargeRelease();
    }

}

public class Casting : FishingState
{
    public Casting(FishingController system) : base(system)
    {

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.Cast();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.RaiseCastingCamera();
    }
}

public class Fishing : FishingState
{
    public Fishing(FishingController system) : base(system)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseSpawnFishes();

    }
    public override void Update()
    {
        base.Update();
        system.StartReeling();
        if (system.IsInCatchArea)
        {
            system.baitLogic.Shake();
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.RaiseFishingCamera();
    }
}

public class Reeling : FishingState
{
    public Reeling(FishingController controller) : base(controller)
    {
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.ReelIn();
    }

}

public class ReelingFish : FishingState
{
    public ReelingFish(FishingController controller) : base(controller)
    {
    }
    public override void Update()
    {
        base.Update();
        system.RaiseReelingFish();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.baitLogic.ReelIn();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.RaiseReelingCamera();
    }
    public override void OnExit()
    {
        base.OnExit();
        system.RaiseOnExitReelingFish();
    }
}


public class InspectFish : FishingState
{
    public InspectFish(FishingController controller) : base(controller)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseNextSummary();
    }
    public override void Update()
    {
        base.Update();
        system.RaiseNextSummary();
    }
    public override void OnExit()
    {
        base.OnExit();
        system.RaiseEndSummary();
    }
}
