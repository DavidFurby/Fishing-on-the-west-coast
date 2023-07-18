using UnityEngine;
using System.Collections.Generic;

public class FishMovement : FishStateMachine
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _rotateSpeed = 2;
    private Transform _tastyPart;
    [SerializeField] private float _offsetAmount = 0.4f;
    private Vector3 _offset;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        SetState(new Swimming(this));

        _rb = GetComponent<Rigidbody>();
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
        if (transform.Find("TastyPart"))
        {
            _tastyPart = transform.Find("TastyPart");
        }
        else
        {
            _tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            _tastyPart.SetParent(gameObject.transform);

            //sets tasty part to tail area (last in array) as default
            //(not setting to tail directly as to not get unwanted interactions on tail animations)
            SetTastyPartPosition(_bones[^1]);
        }
    }

    public void SwimAround()
    {
        _rb.velocity = transform.forward * _speed;
    }

    public void SwimTowards()
    {
        _rb.velocity = transform.forward * _speed;
        RotateTowards();
    }

    public void MunchOn()
    {
        //sets pos to target pos. Pivot point is always center so need offset to look good.
        //Also using rotate to make sure the rotation is correct
        transform.position = target.transform.position + target.transform.rotation * _offset;
        RotateTowards();
    }

    private void RotateTowards()
    {

        Quaternion _lookRotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);

        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }

    public void SetTastyPartPosition(Transform obj)
    {

        //Because of how the armature is imported i need to rotate this :^)
        _tastyPart.SetPositionAndRotation(obj.position, Quaternion.Euler(-89.98f, 0f, 0f));
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