using UnityEngine;
using System.Collections;
public class FishMovement : FishStateMachine
{
    #region Serialized Fields
    [HideInInspector] public GameObject target;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _baitedSpeed = 0.8f;
    [SerializeField] private float _retreatSpeed = 50f;
    [SerializeField] private float _rotateSpeed = 2;
    #endregion

    #region Private Fields
    private Transform[] _bones;
    private Rigidbody _rigidBody;
    private Transform tastyPart;

    #endregion

    #region Unity Methods
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
    #endregion

    #region Public Methods
    public void SwimAround()
    {
        MoveFish(_speed);
    }

    public void SwimTowardsTarget()
    {
        if (IsCloseToTarget())
        {
            SetState(new Retreat(this));
        }
        else
        {
            MoveFish(_baitedSpeed);
        }
    }

    public IEnumerator Retreat()
    {
        Vector3 direction = GetDirectionAwayFromTarget();
        MoveFishInDirection(direction, Random.Range(_retreatSpeed / 2, _retreatSpeed * 2));
        yield return new WaitForSeconds(Random.Range(2, 4));
        SetState(new Baited(this));
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

    public void RotateTowardsTarget()
    {
        Vector3 direction = GetDirectionTowardsTarget();
        RotateTowards(direction);
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion _lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }


    #endregion

    #region Private Methods
    private void InitializeFish()
    {
        SetState(new Swimming(this));
        _rigidBody = GetComponent<Rigidbody>();
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

    private bool IsCloseToTarget()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return Mathf.Approximately(distance, 0.1f);
        }
        else
        {
            return false;
        }

    }

    private Vector3 GetDirectionTowardsTarget()
    {
        if (target != null)
        {
            return target.transform.position - transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 GetDirectionAwayFromTarget()
    {
        if (target != null)
        {
            return transform.position - target.transform.position;

        }
        else
        {
            return Vector3.zero;
        }
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
    }

    private void PositionOnTarget()
    {
        transform.position = target.transform.position;

        _rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationX;

        _rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;

        _rigidBody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
    }

    private void MoveFish(float speed)
    {
        _rigidBody.velocity = transform.forward * speed;
    }

    private void MoveFishInDirection(Vector3 direction, float speed)
    {
        _rigidBody.velocity = direction.normalized * speed;
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
            MoveFishInDirection(direction, _speed);
            RotateTowards(direction);
        }
    }

    public void StopBeingBaited()
    {
        if (GetCurrentState() is Baited)
        {
            SetState(new Swimming(this));
            target = null;
            Vector3 direction = transform.eulerAngles.z > 180 ? -transform.right : transform.right;
            MoveFishInDirection(direction, _speed);
            RotateTowards(direction);
        }
    }
    #endregion
}
