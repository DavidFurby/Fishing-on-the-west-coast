public abstract class PlayerState : State
{
    protected PlayerManager controller;

    public PlayerState()
    {
        controller = PlayerManager.Instance;
    }
}
public class ExplorationIdle : PlayerState
{
    public ExplorationIdle() : base()
    {

    }

    public override void Update()
    {
        controller.HandleInput();
        controller.RaiseOpenItemMenuEvent();

    }
    public override void FixedUpdate()
    {
        controller.HandleMovement();
    }

}

public class Interacting : PlayerState
{
    public Interacting() : base()
    {
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.movement.RotateTowardsInteractive();
    }
}
public class PlayerInDialog : PlayerState
{
    public PlayerInDialog() : base()
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterCharacterDialog();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.RaiseWhileCharacterDialog();
    }
}

public class Shopping : PlayerState
{
    public Shopping() : base()
    {
    }
    public override void Update()
    {
        controller.RaiseNavigateShopEvent();
    }
}

public class FishingIdle : PlayerState
{
    public FishingIdle() : base()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterIdle();
    }
    public override void Update()
    {
        base.Update();
        controller.fishingController.StartCharging();
    }
}

public class Charging : PlayerState
{
    public Charging() : base()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterCharging();
    }
    public override void Update()
    {
        base.Update();
        controller.RaiseWhileCharging();
    }
}
public class Swinging : PlayerState
{
    public Swinging() : base()
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterSwinging();
    }

}

public class Casting : PlayerState
{
    public Casting() : base()
    {

    }
    public override void OnEnter()
    {
        controller.RaiseOnEnterCasting();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.RaiseWhileCasting();
    }
}

public class Fishing : PlayerState
{
    public Fishing() : base()
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterFishing();

    }
    public override void Update()
    {
        base.Update();
        controller.RaiseWhileFishing();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class Reeling : PlayerState
{
    public Reeling() : base()
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterReeling();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.RaiseReelInBait();
    }

}

public class ReelingFish : PlayerState
{
    public ReelingFish() : base()
    {
    }
    public override void Update()
    {
        base.Update();
        controller.RaiseReelingFish();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.RaiseReelInBait();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        controller.RaiseOnExitReelingFish();
    }
}


public class InspectFish : PlayerState
{
    public InspectFish() : base()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RaiseEnterInspecting();
    }
    public override void Update()
    {
        base.Update();
        controller.RaiseNextSummary();
    }
    public override void OnExit()
    {
        base.OnExit();
        controller.RaiseEndSummary();
    }
}
