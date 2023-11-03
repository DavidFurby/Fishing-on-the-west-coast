
using System.Numerics;

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
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.fishMovement.SwimAround();
    }
}
public class Baited : FishState
{
    public Baited(FishController fishController) : base(fishController)
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.fishMovement.SwimTowardsTarget();
        fishController.fishMovement.RotateTowardsTarget();
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
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishController.StartCoroutine(fishController.fishMovement.Retreat());
        fishController.fishMovement.RotateTowardsTarget();
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
    }
}

public class Inspected : FishState
{
    public Inspected(FishController fishController) : base(fishController)
    {
    }
}