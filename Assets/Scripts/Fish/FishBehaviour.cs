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
    }

    void Start()
    {
    
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
    public void AttachToFish()
    {
        if (target.TryGetComponent<FishMovement>(out _))
        {
            GetOrCreateConfigurableJoint();

        }
    }
    public void AttachToBait()
    {
        GetOrCreateConfigurableJoint();
    }

    private void GetOrCreateConfigurableJoint()
    {
        if (!gameObject.TryGetComponent<ConfigurableJoint>(out _))
        {
            ConfigurableJoint joint = _bones[0].gameObject.AddComponent<ConfigurableJoint>();
            joint.connectedBody = target.GetComponent<Rigidbody>();
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Free;

            JointDrive jointDrive = new()
            {
                positionSpring = 1000f,
                positionDamper = 100f,
                maximumForce = Mathf.Infinity
            };
            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }
        PositionOnTarget();
    }
    private void PositionOnTarget()
    {
        transform.position = target.transform.position;

        rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationX;

        rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;

        rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;

        print(transform.position);
    }
    public void GetBaited(GameObject target)
    {
        this.target = target;
        fishController.SetState(new Baited(fishController));
    }
    internal void IfExitSea(bool isTop)
    {
        if (gameObject != null && fishController.GetCurrentState() is Swimming || fishController.GetCurrentState() is Baited)
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
            fishController.SetState(new Swimming(fishController));
            target = null;
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
            SetTastyPartPosition(_bones[^1]);
        }
    }

    public void SetTastyPartPosition(Transform obj)
    {
        tastyPart.SetPositionAndRotation(obj.position, Quaternion.Euler(-89.98f, 0f, 0f));
    }
}