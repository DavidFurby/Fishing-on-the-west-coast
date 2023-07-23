using UnityEngine;
using System.Collections;

public class FishMovement : FishStateMachine
{
    [HideInInspector] public GameObject target;
    [HideInInspector] public Transform tastyPart;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _baitedSpeed = 0.5f;
    [SerializeField] private float _retreatSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 2;
    [SerializeField] private Rigidbody _rigidBody;

    void Start()
    {
        SetState(new Swimming(this));
        SetBones();
    }

    private void SetBones()
    {
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();
        SetTastyPart();
    }

    private void SetTastyPart()
    {
        if (transform.Find("TastyPart"))
        {
            tastyPart = transform.Find("TastyPart");
        }
        else
        {
            tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            tastyPart.SetParent(transform);
            SetTastyPartPosition(_bones[_bones.Length - 1]);
        }
    }

    public void SwimAround()
    {
        _rigidBody.velocity = transform.forward * _speed;
        UpwardsForce();
    }

    public void SwimTowardsTarget()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 0.1)
        {
            SetState(new Retreat(this));
        }
        else
        {
            _rigidBody.velocity = transform.forward * _baitedSpeed;
        }
        UpwardsForce();
    }

    public IEnumerator Retreat()
    {
        Vector3 direction = transform.position - target.transform.position;
        _rigidBody.AddForce(direction.normalized * _retreatSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(1, 3));
        SetState(new Baited(this));
    }

    private void UpwardsForce()
    {
        float upwardForce = Mathf.Abs(Physics.gravity.y) * _rigidBody.mass;
        _rigidBody.AddForce(Vector3.up * upwardForce);
    }

    public void AttachToTarget()
    {
        if (target.TryGetComponent<FishMovement>(out var fishMovement))
        {
            if (fishMovement.tastyPart != null)
            {
                transform.position = fishMovement.tastyPart.position;
            }
        }
        else
        {
            transform.position = target.transform.position + target.transform.rotation * Vector3.zero;
        }
    }

    public void RotateTowards()
    {
        Vector3 direction = target.transform.position - transform.position;
        
        if (direction == Vector3.zero)
            direction = Vector3.up * 0.01f;

        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotateSpeed);
    }

    public void SetTastyPartPosition(Transform obj)
    {
        tastyPart.SetPositionAndRotation(obj.position, Quaternion.Euler(-89.98f, 0f, 0f));
    }

    public void GetBaited(GameObject target)
    {
        this.target = target;
        SetState(new Baited(this));
        }
}
