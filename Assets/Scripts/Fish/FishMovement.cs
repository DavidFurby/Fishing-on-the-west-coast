using UnityEngine;
using System.Collections.Generic;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    private Transform[] _bones;
    [SerializeField] private float _speed = 0.3f;
    [SerializeField] private float _rotateSpeed = 2;
    private Transform _tastyPart;

    [SerializeField] private float _offsetAmount = 0.4f;
    private Vector3 _offset;

    private Rigidbody _rb;

    public enum FishState
    {
        Swimming,
        Baited,
        Hooked,
    }

    public FishState state;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

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
            SetTastyPartPosition(_bones[_bones.Length - 1]);
        }
    }

    private void FixedUpdate()
    {
        if (_target)
        {

            switch (state)
            {
                case FishState.Swimming:
                    SwimAround();
                    break;

                case FishState.Baited:
                    SwimTowards(_target);
                    break;

                case FishState.Hooked:
                    MunchOn(_target);
                    break;

                default:
                    SwimAround();
                    break;
            }
        }
        else
        {
            SwimAround();
        }
    }

    private void SwimAround()
    {
        _rb.velocity = transform.forward * _speed;
    }

    private void SwimTowards(GameObject target)
    {
        _rb.velocity = transform.forward * _speed;
        RotateTowards(target.transform.position);
    }

    public void MunchOn(GameObject target)
    {
        //sets pos to target pos. Pivot point is always center so need offset to look good.
        //Also using rotate to make sure the rotation is correct
        transform.position = target.transform.position + target.transform.rotation * _offset;
        RotateTowards(target.transform.position);
    }

    private void RotateTowards(Vector3 target)
    {

        Quaternion _lookRotation = Quaternion.LookRotation((target - transform.position).normalized);

        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }

    public void SetTastyPartPosition(Transform obj)
    {
        _tastyPart.position = obj.position;

        //Because of how the armature is imported i need to rotate this :^)
        _tastyPart.rotation = Quaternion.Euler(-89.98f, 0f, 0f);
    }


    public void GetBaited(GameObject bait)
    {
        _target = bait;
        SetFishState(FishState.Baited);
    }

    public void SetFishState(FishState fishState)
    {
        state = fishState;

        if (state == FishState.Hooked)
        {
            _rotateSpeed = 10;
        }
        else
        {
            _rotateSpeed = 2;
        }
    }
}