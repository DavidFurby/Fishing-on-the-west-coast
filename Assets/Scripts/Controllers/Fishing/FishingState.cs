using System.Collections.Generic;
using UnityEngine;

public class FishingState
{
    public readonly List<FishDisplay> totalFishes = new();
    public Bait bait;
    public FishingRod fishingRod;

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
        Debug.Log(totalFishes.Count);
    }
}

public class StandBy : FishingState
{
    public override void OnEnter()
    {
        base.OnEnter();
        totalFishes.Clear();
    }
}

public class Charging : FishingState { }

public class Casting : FishingState { }

public class Fishing : FishingState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }
}

public class Reeling : FishingState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }
}

public class ReelingFish : FishingState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }
}

public class InspectFish : FishingState
{
    public override void OnEnter()
    {
        base.OnEnter();
    }
}

public class FishingStateMachine
{
    private FishingState currentState = new StandBy();

    public FishingState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(FishingState state)
    {
        currentState?.OnExit();

        currentState = state;
        currentState.OnEnter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
