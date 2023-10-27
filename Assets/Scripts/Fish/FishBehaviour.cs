using UnityEngine;
[RequireComponent(typeof(FishMovement))]
public class FishBehaviour : FishStateMachine
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    private Transform tastyPart;
    internal FishMovement fishMovement;
    internal Rigidbody rigidBody;


    private void OnEnable()
    {
        SeaFloorCollision.OnBaitCollision += StopBeingBaited;
    }

    void Start()
    {
        fishMovement = GetComponent<FishMovement>();
        InitializeFish();
        SetTastyPart();
    }
    private void OnDestroy()
    {
        SeaFloorCollision.OnBaitCollision -= StopBeingBaited;
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
        SetState(new Baited(this));
    }
    internal void IfExitSea(bool isTop)
    {
        if (gameObject != null && GetCurrentState() is Swimming || GetCurrentState() is Baited)
        {
            Vector3 direction = isTop ? -transform.up : transform.up;
            fishMovement.MoveFishInDirection(direction, fishMovement.speed);
            fishMovement.RotateTowards(direction);
        }
    }
    public void StopBeingBaited()
    {
        if (GetCurrentState() is Baited)
        {
            SetState(new Swimming(this));
            target = null;
            Vector3 direction = transform.eulerAngles.z > 180 ? -transform.right : transform.right;
            fishMovement.MoveFishInDirection(direction, fishMovement.speed);
            fishMovement.RotateTowards(direction);
        }
    }

    private void InitializeFish()
    {
        SetState(new Swimming(this));
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