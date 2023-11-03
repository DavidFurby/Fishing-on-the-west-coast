using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FishBehaviour : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    private Transform[] _bones;
    private Transform tastyPart;
    internal Rigidbody rigidBody;
    internal FishController controller;

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
        this.controller = controller;
    }

    public void AttachToBait()
    {
        if (target != null)
        {
            transform.position = target.transform.TransformPoint(_bones[0].localPosition);

            AddHingeJoint(target.GetComponent<Rigidbody>());
        }
    }
    public void AttachToFish()
    {
        if (target != null)
        {
            if (target.TryGetComponent<FishBehaviour>(out var targetFishBehaviour))
            {
                Transform targetTastyPart = targetFishBehaviour.tastyPart;
                if (targetTastyPart != null && targetTastyPart)
                {
                    transform.position = target.transform.TransformPoint(targetTastyPart.localPosition);
                    AddHingeJoint(targetTastyPart.GetComponent<Rigidbody>());
                }
            }

        }
    }

    private void AddHingeJoint(Rigidbody targetBody)
    {
        HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        joint.connectedBody = targetBody;
        joint.anchor = gameObject.transform.InverseTransformPoint(_bones[0].position);
        SetSpring(joint);

    }

    private static void SetSpring(HingeJoint joint)
    {
        joint.useSpring = true;
        JointSpring joint_spring = joint.spring;
        joint_spring.spring = 1;
        joint.spring = joint_spring;
    }

    public void ApplyWaterDrag()
    {
        Vector3 oppositeDirection = -rigidBody.velocity.normalized;
        Vector3 torqueDirection = Vector3.Cross(oppositeDirection, transform.up);
        float torqueAmount = 0.1f;
        rigidBody.AddTorque(torqueDirection * torqueAmount, ForceMode.Force);
    }

    public void GetBaited(GameObject target)
    {
        this.target = target;
        controller.SetState(new Baited(controller));
    }

    internal void IfExitSea(bool isTop)
    {
        if (gameObject != null && (controller.GetCurrentState() is Swimming || controller.GetCurrentState() is Baited))
        {
            Vector3 direction = isTop ? -transform.up : transform.up;
            controller.fishMovement.MoveFishInDirection(direction, controller.fishMovement.speed);
            controller.fishMovement.RotateTowards(direction);
        }
    }

    public void StopBeingBaited()
    {
        if (controller.GetCurrentState() is Baited)
        {
            target = null;
            controller.SetState(new Swimming(controller));
            Vector3 direction = transform.eulerAngles.z > 180 ? -transform.right : transform.right;
            controller.fishMovement.MoveFishInDirection(direction, controller.fishMovement.speed);
            controller.fishMovement.RotateTowards(direction);
        }
    }

    private void InitializeFish()
    {
        controller.SetState(new Swimming(controller));
        rigidBody = GetComponent<Rigidbody>();
        _bones = transform.Find("Armature").GetChild(0).GetComponentsInChildren<Transform>();
    }

    //The part of the fish that other fishes should attach to
    private void SetTastyPart()
    {
        tastyPart = transform.Find("TastyPart");

        if (tastyPart == null)
        {
            tastyPart = new GameObject("TastyPart").GetComponent<Transform>();
            tastyPart.SetParent(gameObject.transform);
            tastyPart.SetPositionAndRotation(_bones[^1].position, Quaternion.Euler(-89.98f, 0f, 0f));
            tastyPart.AddComponent<Rigidbody>();
        }
    }
}
