
public class FishState : State
{
    protected FishMovement fishMovement;
    public FishState(FishMovement fishMovement)
    {
        this.fishMovement = fishMovement;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

public class Swimming : FishState
{
    public Swimming(FishMovement fishMovement) : base(fishMovement)
    {
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.SwimAround();
    }
}
public class Baited : FishState
{
    public Baited(FishMovement fishMovement) : base(fishMovement)
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.SwimTowardsTarget();
        fishMovement.RotateTowardsTarget();
    }
}
public class Retreat : FishState
{
    public Retreat(FishMovement fishMovement) : base(fishMovement)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.StartCoroutine(fishMovement.Retreat());
        fishMovement.RotateTowardsTarget();
    }
}
public class Hooked : FishState
{
    public Hooked(FishMovement fishMovement) : base(fishMovement)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        fishMovement.MunchOnBait();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.RotateTowardsTarget();
    }
}

public class HookedToFish : FishState
{
    public HookedToFish(FishMovement fishMovement) : base(fishMovement)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.RotateTowardsTarget();
        fishMovement.MunchOnFish();
    }
}

public class Inspected : FishState
{
    public Inspected(FishMovement fishMovement) : base(fishMovement)
    {
    }
}