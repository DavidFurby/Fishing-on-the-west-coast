using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State currentState;
    public State GetCurrentState()
    {
        return currentState;
    }

    public virtual void SetState(State state)
    {
        currentState?.OnExit();

        currentState = state;
        currentState.OnEnter();
    }

    protected virtual void Update()
    {
        currentState?.Update();
    }
    protected virtual void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
    protected virtual void LateUpdate()
    {
        currentState?.LateUpdate();
    }
}

public class ExplorationStateMachine : StateMachine
{
    public void SetState(ExplorationState state)
    {
        base.SetState(state);
    }
}

public class FishStateMachine : StateMachine
{

    public void SetState(FishState state)
    {
        base.SetState(state);
    }
}

public abstract class FishingStateMachine : StateMachine
{
    public void SetState(FishingState state)
    {
        base.SetState(state);
    }
}
