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
        system.fishingMiniGame.ChargingBalance();
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
        system.fishingRodLogic.WaitForSwingAnimation();
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
        system.fishingCamera.SetCameraToBait();
        system.fishingCamera.MoveCameraCloserToBait();
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
        system.fishingCamera.SetCameraToBait();
        system.fishingCamera.MoveCameraToOriginal();
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
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.fishingCamera.SetCameraToBait();
        system.fishingCamera.MoveCameraCloserToBait(0.2f);
    }
    public override void OnExit()
    {
        base.OnExit();
        system.fishingMiniGame.EndBalanceMiniGame();
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
