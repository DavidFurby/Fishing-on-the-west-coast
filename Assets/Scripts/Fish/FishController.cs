using UnityEngine;

[RequireComponent(typeof(FishMovement))]
[RequireComponent(typeof(FishBehaviour))]

public class FishController : FishStateMachine
{
    public FishBehaviour behaviour;
    public FishMovement movement;

    private void Awake()
    {
        behaviour = GetComponent<FishBehaviour>();
        movement = GetComponent<FishMovement>();

        behaviour.Initialize(this);
        movement.Initialize(this);
    }
}
