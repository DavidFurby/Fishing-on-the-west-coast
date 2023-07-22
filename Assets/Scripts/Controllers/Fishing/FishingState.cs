using System.Diagnostics;

public abstract class FishingState : State
{
    protected FishingSystem system;

    public FishingState(FishingSystem system)
    {
        this.system = system;
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
        system.seaSpawner.StopSpawnFish();
        system.catchArea.ResetValues();
        system.fishingRodLogic.ResetValues();
    }
    public override void Update()
    {
        base.Update();
        system.StartFishing();
        system.itemMenu.HandleInputs();
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
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.fishingCamera.UpdateCameraPosition();
        system.fishingCamera.MoveCameraCloserToBait();
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
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.fishingCamera.UpdateCameraPosition();
        system.fishingCamera.MoveCameraToOriginal();
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
    public override void LateUpdate()
    {
        base.LateUpdate();
        system.fishingCamera.UpdateCameraPosition();
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
