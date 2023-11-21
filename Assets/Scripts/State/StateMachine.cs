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

public class FishStateMachine : StateMachine
{

    public void SetState(FishState state)
    {
        base.SetState(state);
    }
}

public abstract class PlayerStateMachine : StateMachine
{
    public void SetState(PlayerState state)
    {
        base.SetState(state);
    }
}

public abstract class CameraStateMachine : StateMachine
{
    public void SetState(CameraState state)
    {
        base.SetState(state);
    }
}

public abstract class CharacterStateMachine : StateMachine
{
    public void SetState(CharacterState state)
    {
        base.SetState(state);
    }
}