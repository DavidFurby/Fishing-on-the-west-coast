using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FishMovement : FishStateMachine
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _baitedSpeed = 0.5f;

    [SerializeField] private float _retreatSpeed = 5f;


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
    }
    // Rotate towards the bait if baited
    public void RotateTowardsBait()
    {
        if (target != null)
            _offset.y = _offsetAmount;

        //Gets an Array of Armatures's children aka all the bones, in order Head -> Tail
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();

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


    public void SwimAround()
    {
        _rigidBody.velocity = transform.forward * _speed;
    }

    public void SwimTowardsTarget()
    {
        Debug.Log("SwimTowardsTarget");
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
        Debug.Log("Retreat");
        Vector3 direction = transform.position - target.transform.position;
        _rigidBody.AddForce(direction.normalized * _retreatSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(Random.Range(2, 4));
        SetState(new Baited(this));
    }
    
    //Keep the fish from sinking while swimming
    public void UpwardsForce()
    {
        float upwardForce = Mathf.Abs(Physics.gravity.y * Random.Range(-2, 2)) * _rigidBody.mass;
        _rigidBody.AddForce(Vector3.up * upwardForce);
    }


    public void MunchOn()
    {
        //sets pos to target pos. Pivot point is always center so need offset to look good.
        //Also using rotate to make sure the rotation is correct

        //If target is Fish set the position to tastyPart
        if (target.TryGetComponent<FishMovement>(out var fishMovement))
        {
            transform.position = fishMovement.tastyPart.position;
        }
        else
        {
            transform.position = target.transform.position + target.transform.rotation * _offset;
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