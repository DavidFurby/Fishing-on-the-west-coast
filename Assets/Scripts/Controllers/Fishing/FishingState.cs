using System.Collections.Generic;
using UnityEngine;

public abstract class FishingState
{
    public readonly List<FishDisplay> totalFishes = new();
    public Bait bait;
    public FishingRod fishingRod;
    protected FishingController controller;

    public FishingState(FishingController controller)
    {
        this.controller = controller;
    }

    public virtual void OnEnter()
    {
        bait = MainManager.Instance.game.EquippedBait;
        fishingRod = MainManager.Instance.game.EquippedFishingRod;
    }

    public virtual void OnExit() { }

    public virtual void Update() { }

    public void AddFish(FishDisplay fish)
    {
        totalFishes.Add(fish);
    }
}

public class Idle : FishingState
{
    public Idle(FishingController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {

        base.OnEnter();
        totalFishes.Clear();
    }
    public override void Update()
    {
        base.Update();
        controller.StartFishing();
    }
}

public class Charging : FishingState
{
    public Charging(FishingController controller) : base(controller)
    {
    }

    public override void Update()
    {
        base.Update();
        controller.StartFishing();
    }
}

public class Casting : FishingState
{
    public Casting(FishingController controller) : base(controller)
    {
    }
}

public class Fishing : FishingState
{
    public Fishing(FishingController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void Update()
    {
        base.Update();
        controller.StartReeling();
    }
}

public class Reeling : FishingState
{
    public Reeling(FishingController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void Update()
    {
        base.Update();
    }

}

public class ReelingFish : FishingState
{
    public ReelingFish(FishingController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void Update()
    {
        base.Update();
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
    }
}

public abstract class FishingStateMachine : MonoBehaviour
{
    private FishingState currentState;
    public FishingState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(FishingState state)
    {
        currentState?.OnExit();

        currentState = state;
        currentState.OnEnter();
        Debug.Log(currentState);
    }

    void Update()
    {
        currentState?.Update();
    }
}
