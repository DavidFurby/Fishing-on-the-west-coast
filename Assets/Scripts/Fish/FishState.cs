public class FishState : State
{
    protected FishController controller;
    public FishState(FishController controller)
    {
        this.controller = controller;
    }
}

public class Swimming : FishState
{
    public Swimming(FishController controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (controller.gameObject.activeInHierarchy)
        {
            controller.StartCoroutine(controller.movement.SetSpeed(1, 2, controller.movement.swimmingSpeed));
        }

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.movement.SwimAround();
    }
    public override void OnExit()
    {
        base.OnEnter();
        controller.StopCoroutine(controller.movement.SetSpeed(1, 2, controller.movement.swimmingSpeed));

    }
}
public class Baited : FishState
{
    public Baited(FishController controller) : base(controller)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.StartCoroutine(controller.movement.SetSpeed(5, 2, controller.movement.baitedSpeed));
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.movement.SwimTowardsTarget();
        controller.movement.RotateTowardsTarget();
    }
    public override void OnExit()
    {
        base.OnExit();
        controller.StopCoroutine(controller.movement.SetSpeed(5, 2, controller.movement.baitedSpeed));
    }
}
public class Retreat : FishState
{
    public Retreat(FishController controller) : base(controller)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.StartCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.retreatSpeed));
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.StartCoroutine(controller.movement.Retreat());
        controller.movement.RotateTowardsTarget();
    }

    public override void OnExit()
    {
        base.OnExit();
        controller.StopCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.retreatSpeed));

    }
}
public class Hooked : FishState
{
    public Hooked(FishController controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.behaviour.AttachToBait();
        controller.StartCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.baitedSpeed));
    }
    public override void Update()
    {
        base.Update();

    }
    public override void OnExit()
    {
        base.OnExit();
        controller.StopCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.baitedSpeed));
    }
}

public class HookedToFish : FishState
{
    public HookedToFish(FishController controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.behaviour.AttachToFish();
        controller.StartCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.baitedSpeed));
    }
    public override void OnExit()
    {
        base.OnExit();
        controller.StopCoroutine(controller.movement.SetSpeed(8, 2, controller.movement.baitedSpeed));
    }
}

public class Inspected : FishState
{
    public Inspected(FishController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.StartCoroutine(controller.movement.SetSpeed(1, 5, controller.movement.swimmingSpeed));

    }
    public override void OnExit()
    {
        base.OnExit();
        controller.StopCoroutine(controller.movement.SetSpeed(1, 5, controller.movement.swimmingSpeed));
    }
}
