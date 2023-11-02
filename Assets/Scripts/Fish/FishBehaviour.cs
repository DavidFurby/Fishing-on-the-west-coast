using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishBehaviour : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    private Transform tastyPart;
    internal Rigidbody rigidBody;
    internal FishController fishController;

    private void OnEnable()
    {
        SeaFloorCollision.OnBaitCollision += StopBeingBaited;
        InitializeFish();
        SetTastyPart();
    }

    private void OnDestroy()
    {
        SeaFloorCollision.OnBaitCollision -= StopBeingBaited;
    }

    public void Initialize(FishController controller)
    {
        fishController = controller;
    }

    public void AttachToTarget()
    {
        if (target.TryGetComponent<FishMovement>(out _) || target.TryGetComponent<FixedJoint>(out _))
        {
            AddFixedJoint();
        }
    }

    private void AddFixedJoint()
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = target.GetComponent<Rigidbody>();
        joint.anchor = gameObject.transform.InverseTransformPoint(_bones[0].position);
        
        transform.position = target.transform.TransformPoint(_bones[0].localPosition);
    }

    public void GetBaited(GameObject target)
    {
        this.target = target;
        fishController.SetState(new Baited(fishController));
    }

    internal void IfExitSea(bool isTop)
    {
        if (gameObject != null && (fishController.GetCurrentState() is Swimming || fishController.GetCurrentState() is Baited))
        {
            Vector3 direction = isTop ? -transform.up : transform.up;
            fishController.fishMovement.MoveFishInDirection(direction, fishController.fishMovement.speed);
            fishController.fishMovement.RotateTowards(direction);
        }
    }

    public void StopBeingBaited()
    {
        if (fishController.GetCurrentState() is Baited)
        {
            target = null;
            fishController.SetState(new Swimming(fishController));
            Vector3 direction = transform.eulerAngles.z > 180 ? -transform.right : transform.right;
            fishController.fishMovement.MoveFishInDirection(direction, fishController.fishMovement.speed);
            fishController.fishMovement.RotateTowards(direction);
        }
    }

    private void InitializeFish()
    {
        fishController.SetState(new Swimming(fishController));
        rigidBody = GetComponent<Rigidbody>();
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();
    }

    private void SetTastyPart()
    {
        tastyPart = transform.Find("TastyPart");
        
        if (tastyPart == null)
        {
            tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            tastyPart.SetParent(gameObject.transform);
            tastyPart.SetPositionAndRotation(_bones[^1].position, Quaternion.Euler(-89.98f, 0f, 0f));
        }
    }
}
