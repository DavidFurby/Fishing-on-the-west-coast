
public class FishState : State
{
    protected FishMovement fishMovement;
    public FishState(FishMovement fishMovement)
    {
        this.fishMovement = fishMovement;
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
        fishMovement.RotateTowardsBait();
        fishMovement.SwimTowardsTarget();
    }
}

public class Hooked : FishState
{
    public Hooked(FishMovement fishMovement) : base(fishMovement)
    {
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        fishMovement.HookToTarget();
        fishMovement.MunchOn();

    }
}
public class Inspected : FishState
{
    public Inspected(FishMovement fishMovement) : base(fishMovement)
    {
    }
}