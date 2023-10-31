using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] internal float speed = 0.5f;
    [SerializeField] private float _baitedSpeed = 0.8f;
    [SerializeField] private float _retreatSpeed = 50f;
    [SerializeField] private float _rotateSpeed = 2;
    internal FishController fishController;

    #endregion

    public void Initialize(FishController controller)
    {
        fishController = controller;
    }
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

    public void RotateTowardsTarget()
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
        fishController.fishBehaviour.rigidBody.velocity = transform.forward * speed;
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
