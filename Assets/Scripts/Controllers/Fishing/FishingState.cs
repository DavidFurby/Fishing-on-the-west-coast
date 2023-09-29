public abstract class FishingState : State
{
    protected FishingController system;

    public FishingState()
    {
        system = FishingController.Instance;
    }
}
public class NotFishing : FishingState
{
    public NotFishing() : base() { }
}

public class FishingIdle : FishingState
{
    public FishingIdle() : base()
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
    public Charging() : base()
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
    public Swinging() : base()
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
    public Casting() : base()
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
    public Fishing() : base()
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
    public Reeling() : base()
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
    public ReelingFish() : base()
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
    public InspectFish() : base()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        system.RaiseEnterInspecting();
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
