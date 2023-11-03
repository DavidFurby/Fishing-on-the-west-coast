using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] internal float speed;
    [SerializeField] private float _baitedSpeed;
    [SerializeField] private float _retreatSpeed;
    private readonly float _rotateSpeed = 0.5f;
    internal FishController fishController;
    #endregion

    #region Initialization
    public void Initialize(FishController controller)
    {
        fishController = controller;
    }
    #endregion

    #region Public Methods
    public void SwimAround()
    {
        MoveFish(speed);
    }

    public void SwimTowardsTarget()
    {
        if (IsCloseToTarget())
        {
            fishController.SetState(new Retreat(fishController));
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
        fishController.SetState(new Baited(fishController));
    }
    #endregion

    #region Internal Methods
    internal void RotateTowardsTarget()
    {
        Vector3 direction = GetDirectionTowardsTarget();
        if (direction != Vector3.zero)
        {
            RotateTowards(direction);
        }
    }

    internal void RotateTowards(Vector3 direction)
    {
        Quaternion _lookRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }

    internal void MoveFish(float speed)
    {
        if (fishController.fishBehaviour != null)
        {
            fishController.fishBehaviour.rigidBody.velocity = transform.forward * speed;
        }
    }

    internal void MoveFishInDirection(Vector3 direction, float speed)
    {
        fishController.fishBehaviour.rigidBody.velocity = direction.normalized * speed;
    }
    #endregion

    #region Private Methods
    private bool IsCloseToTarget()
    {
        return fishController.fishBehaviour.target != null && Mathf.Approximately(Vector3.Distance(transform.position, fishController.fishBehaviour.target.transform.position), 0.1f);
    }

    private Vector3 GetDirectionTowardsTarget()
    {
        return fishController.fishBehaviour.target != null ? fishController.fishBehaviour.target.transform.position - transform.position : Vector3.zero;
    }

    private Vector3 GetDirectionAwayFromTarget()
    {
        return fishController.fishBehaviour.target != null ? transform.position - fishController.fishBehaviour.target.transform.position : Vector3.zero;
    }
    #endregion
}
