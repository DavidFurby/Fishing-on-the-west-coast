using UnityEngine;
using System.Collections;

public class FishMovement : FishStateMachine
{
    [HideInInspector] public GameObject target;
    [HideInInspector] public Transform tastyPart;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _baitedSpeed = 0.5f;
    [SerializeField] private float _retreatSpeed = 0.5f;
    [SerializeField] private float _rotateSpeed = 2;
    private Vector3 _offset;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        SetState(new Swimming(this));
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.solverIterations = 100;
        SetBones();
    }

    private void SetBones()
    {
        //Gets an Array of Armatures's children aka all the bones, in order Head -> Tail
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();
        SetTastyPart();
    }

    // Sets the position of the tasty part of the fish
    private void SetTastyPart()
    {
        //If TastyPart Object already exists, then it's location is probably
        //manually decided, so no need to set the position. If it doesn't exist
        //then we create it and set the pos to the tail.
        if (transform.Find("TastyPart"))
        {
            tastyPart = transform.Find("TastyPart");
        }
        else
        {
            tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            tastyPart.SetParent(transform);

            //sets tasty part to tail area (last in array) as default
            //(not setting to tail directly as to not get unwanted interactions on tail animations)
            SetTastyPartPosition(_bones[^1]);
        }
    }

    // Makes the fish swim around
    public void SwimAround()
    {
        _rigidBody.velocity = transform.forward * _speed;
        UpwardsForce();
    }

    // Makes the fish swim towards its target
    public void SwimTowardsTarget()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 0)
        {
            SetState(new Retreat(this));
        }
        else
        {
            _rigidBody.velocity = transform.forward * _baitedSpeed;
        }
        UpwardsForce();
    }

    //Retreat if too close too target
    public IEnumerator Retreat()
    {
        Vector3 direction = transform.position - target.transform.position;
        _rigidBody.AddForce(direction.normalized * _retreatSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(1, 3));
        SetState(new Baited(this));
    }

    //Keep the fish from shinking while swimming
    private void UpwardsForce()
    {
        float upwardForce = Mathf.Abs(Physics.gravity.y) * _rigidBody.mass;
        _rigidBody.AddForce(Vector3.up * upwardForce);
    }

    // Makes the fish munch on its target
    public void AttachToTarget()
    {
        //sets pos to target pos. Pivot point is always center so need offset to look good.
        //Also using rotate to make sure the rotation is correct

        //If target is Fish set the position to tastyPart
        if (target.TryGetComponent<FishMovement>(out var fishMovement))
        {
            if (fishMovement.tastyPart != null)
            {
                transform.position = fishMovement.tastyPart.position;
            }
        }
        else
        {
            transform.position = target.transform.position + target.transform.rotation * _offset;
        }
        RotateTowards();
    }

    // Rotates the fish towards its target
    public void RotateTowards()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.z = 0; // Set the z-component to zero
        if (direction == Vector3.zero)
        {
            //avoid direction viewing vector being zero
            direction = Vector3.up * 0.01f;
        }
        Quaternion _lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }

    // Sets the position of the tasty part of the fish
    public void SetTastyPartPosition(Transform obj)
    {

        //Because of how the armature is imported i need to rotate this :^)
        tastyPart.SetPositionAndRotation(obj.position, Quaternion.Euler(-89.98f, 0f, 0f));
    }

    // Sets the state of the fish to baited and sets its target
    public void GetBaited(GameObject target)
    {
        this.target = target;
        SetState(new Baited(this));
    }

    //Set the position of the fish to the target if hooked
    public void SetRotationSpeed()
    {
        if (target != null)
        {
            _rotateSpeed = 10;
        }
        else
        {
            _rotateSpeed = 2;
        }
    }
}