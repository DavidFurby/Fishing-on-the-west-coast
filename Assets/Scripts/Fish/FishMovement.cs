using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FishMovement : FishStateMachine
{
    #region Serialized Fields
    [HideInInspector] public GameObject target;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _baitedSpeed = 0.8f;
    [SerializeField] private float _retreatSpeed = 50f;
    [SerializeField] private float _rotateSpeed = 2;
    public Transform tastyPart;
    #endregion

    #region Private Fields
    private Transform[] _bones;
    private Rigidbody _rigidBody;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        SeaFloorCollision.OnSeaFloorCollision += StopBeingBaited;
    }

    void Start()
    {
        InitializeFish();
        SetTastyPart();
    }

    private void OnDestroy()
    {
        SeaFloorCollision.OnSeaFloorCollision -= StopBeingBaited;
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

    public void MunchOnFish()
    {
        if (target.TryGetComponent<FishMovement>(out var fishMovement))
        {
            transform.position = fishMovement.tastyPart.position;
        }
    }

    public void MunchOnBait()
    {
        HingeJoint hinge = GetOrCreateHingeJoint();
        hinge.connectedBody = target.GetComponent<Rigidbody>();
        hinge.anchor = Vector3.zero;
        hinge.axis = Vector3.right;

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
            return distance < 0.1f;
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

    private HingeJoint GetOrCreateHingeJoint()
    {
        HingeJoint hinge = gameObject.GetComponent<HingeJoint>();
        if (hinge == null)
        {
            hinge = gameObject.AddComponent<HingeJoint>();
        }
        return hinge;
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
