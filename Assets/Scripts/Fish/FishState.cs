public class FishState : State
{
    protected FishController fishController;
    public FishState(FishController fishController)
    {
        this.fishController = fishController;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class Swimming : FishState
{
    public Swimming(FishController fishController) : base(fishController)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (fishController.gameObject.activeInHierarchy)
        {
            fishController.StartCoroutine(fishController.fishMovement.SetSpeed(1, 2, fishController.fishMovement.swimmingSpeed));
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.fishMovement.SwimAround();
    }
    public override void OnExit()
    {
        base.OnEnter();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(1, 2, fishController.fishMovement.swimmingSpeed));

    }
}
public class Baited : FishState
{
    public Baited(FishController fishController) : base(fishController)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        fishController.StartCoroutine(fishController.fishMovement.SetSpeed(5, 2, fishController.fishMovement.baitedSpeed));
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.fishMovement.SwimTowardsTarget();
        fishController.fishMovement.RotateTowardsTarget();
    }
    public override void OnExit()
    {
        base.OnExit();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(5, 2, fishController.fishMovement.baitedSpeed));
    }
}
public class Retreat : FishState
{
    public Retreat(FishController fishController) : base(fishController)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishController.StartCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.retreatSpeed));
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.StartCoroutine(fishController.fishMovement.Retreat());
        fishController.fishMovement.RotateTowardsTarget();
    }

    public override void OnExit()
    {
        base.OnExit();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.retreatSpeed));

    }
}
public class Hooked : FishState
{
    public Hooked(FishController fishController) : base(fishController)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishController.fishBehaviour.AttachToBait();
        fishController.StartCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.baitedSpeed));
    }
    public override void Update()
    {
        base.Update();

    }
    public override void OnExit()
    {
        base.OnExit();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.baitedSpeed));
    }
}

public class HookedToFish : FishState
{
    public HookedToFish(FishController fishController) : base(fishController)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishController.fishBehaviour.AttachToFish();
        fishController.StartCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.baitedSpeed));
    }
    public override void OnExit()
    {
        base.OnExit();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(8, 2, fishController.fishMovement.baitedSpeed));
    }
}

public class Inspected : FishState
{
    public Inspected(FishController fishController) : base(fishController)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        fishController.StartCoroutine(fishController.fishMovement.SetSpeed(1, 5, fishController.fishMovement.swimmingSpeed));

    }
    public override void OnExit()
    {
        base.OnExit();
        fishController.StopCoroutine(fishController.fishMovement.SetSpeed(1, 5, fishController.fishMovement.swimmingSpeed));
    }
}
