
public class FishState : State
{
    protected FishBehaviour fishBehaviour;
    public FishState(FishBehaviour fishBehaviour)
    {
        this.fishBehaviour = fishBehaviour;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class Swimming : FishState
{
    public Swimming(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishBehaviour.fishMovement.SwimAround();
    }
}
public class Baited : FishState
{
    public Baited(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishBehaviour.fishMovement.SwimTowardsTarget();
        fishBehaviour.fishMovement.RotateTowardsTarget();
    }
}
public class Retreat : FishState
{
    public Retreat(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishBehaviour.StartCoroutine(fishBehaviour.fishMovement.Retreat());
        fishBehaviour.fishMovement.RotateTowardsTarget();
    }
}
public class Hooked : FishState
{
    public Hooked(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishBehaviour.AttachToBait();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishBehaviour.fishMovement.RotateTowardsTarget();
    }
}

public class HookedToFish : FishState
{
    public HookedToFish(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishBehaviour.AttachToFish();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishBehaviour.fishMovement.RotateTowardsTarget();
    }
}

public class Inspected : FishState
{
    public Inspected(FishBehaviour fishBehaviour) : base(fishBehaviour)
    {
    }
}