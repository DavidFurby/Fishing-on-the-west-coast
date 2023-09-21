using System.Diagnostics;

public abstract class FishingState : State
{
    protected FishingController system;

    public FishingState()
    {
        this.system = FishingController.Instance;
    }
}
public class NotFishing : FishingState
{
    public NotFishing(FishingController system) : base() { }
}

public class FishingIdle : FishingState
{
    public FishingIdle(FishingController system) : base()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseEnterIdle();
    }
    public override void Update()
    {
        base.Update();
        system.StartCharging();
    }
}

public class Charging : FishingState
{
    public Charging(FishingController system) : base()
    {
    }

    public override void Update()
    {
        base.Update();
        system.RaiseWhileCharging();
    }
}
public class Swinging : FishingState
{
    public Swinging(FishingController system) : base()
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
    public Casting(FishingController system) : base()
    {

    }
    public override void OnEnter() {
        system.RaiseOnEnterCasting();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.RaiseWhileCasting();
    }
}

public class Fishing : FishingState
{
    public Fishing(FishingController system) : base()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseEnterFishing();

    }
    public override void Update()
    {
        base.Update();
        system.RaiseWhileFishing();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class Reeling : FishingState
{
    public Reeling(FishingController controller) : base()
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseEnterReeling();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        system.RaiseReelInBait();
    }

}

public class ReelingFish : FishingState
{
    public ReelingFish(FishingController controller) : base()
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
        system.RaiseReelInBait();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        system.RaiseOnExitReelingFish();
    }
}


public class InspectFish : FishingState
{
    public InspectFish(FishingController controller) : base()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseStartInspecting();
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
