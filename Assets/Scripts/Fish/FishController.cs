using UnityEngine;

[RequireComponent(typeof(FishMovement))]
[RequireComponent(typeof(FishBehaviour))]

public class FishController : FishStateMachine
{
    public FishBehaviour fishBehaviour;
    public FishMovement fishMovement;

    private void Start()
    {
        if (!TryGetComponent(out fishBehaviour))
        {
            Debug.LogError("FishBehaviour not found!");
        }

        if (!TryGetComponent(out fishMovement))
        {
            Debug.LogError("FishMovement not found!");
        }

        fishBehaviour.Initialize(this);
        fishMovement.Initialize(this);
    }
}
