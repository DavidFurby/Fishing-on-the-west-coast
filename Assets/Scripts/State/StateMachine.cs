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
        currentState.FixedUpdate();
    }
    protected virtual void LateUpdate()
    {
        currentState.LateUpdate();
    }
}
