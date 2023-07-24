using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FishMovement : FishStateMachine
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _baitedSpeed = 0.5f;

    [SerializeField] private float _retreatSpeed = 50f;


    [SerializeField] private float _rotateSpeed = 2;
    public Transform tastyPart;
    [SerializeField] private float _offsetAmount = 0.4f;
    private Vector3 _offset;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        SetState(new Swimming(this));

        _rigidBody = GetComponent<Rigidbody>();
        //Gets an Array of Armatures's children aka all the bones, in order Head -> Tail
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();

        SetTastyPart();
    }

    private void SetTastyPart()
    {
        //If TastyPart Object already exists, then it's location is probably
        //manually decided, so no need to set the position. If it doesn't exist
        //then we create it and set the pos to the tail.
        tastyPart = transform.Find("TastyPart");
        if (tastyPart == null)
        {
            tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            tastyPart.SetParent(gameObject.transform);

            //sets tasty part to tail area (last in array) as default
            //(not setting to tail directly as to not get unwanted interactions on tail animations)
            SetTastyPartPosition(_bones[^1]);
        }
    }

    // Rotate towards the bait if baited
    public void RotateTowardsBait()
    {
        if (target != null)
            _offset.y = _offsetAmount;


    }


    public void SwimAround()
    {
        _rigidBody.velocity = transform.forward * _speed;
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
    }
    //Retreat if too close too target
    public IEnumerator Retreat()
    {
        Vector3 direction = transform.position - target.transform.position;
        _rigidBody.velocity = direction.normalized * _retreatSpeed;
        yield return new WaitForSeconds(Random.Range(2, 4));
        SetState(new Baited(this));
    }
    public void MunchOn()
    {

        // Set the connectedBody property of the hinge joint to the target's Rigidbody
        if (target.TryGetComponent<FishMovement>(out var fishMovement))
        {
            transform.position = fishMovement.tastyPart.position;

        }
        else
        {
            // Add a HingeJoint component to the fish game object
            HingeJoint hinge = gameObject.AddComponent<HingeJoint>();

            hinge.connectedBody = target.GetComponent<Rigidbody>();

            // Set other properties of the hinge joint as desired
            hinge.anchor = Vector3.zero;
            hinge.axis = Vector3.right;
        }

    }


    public void RotateTowards()
    {
        Vector3 direction = target.transform.position - transform.position;
        if (direction == Vector3.zero)
        {
            //avoid direction viewing vector being zero
            direction = new Vector3(0, 0, 0.01f);
        }
        Quaternion _lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }



    public void SetTastyPartPosition(Transform obj)
    {

        //Because of how the armature is imported i need to rotate this :^)
        tastyPart.SetPositionAndRotation(obj.position, Quaternion.Euler(-89.98f, 0f, 0f));
    }


    public void GetBaited(GameObject target)
    {
        this.target = target;
        SetState(new Baited(this));
    }
    //Set the position of the fish to the bait if hooked
    public void HookToTarget()
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