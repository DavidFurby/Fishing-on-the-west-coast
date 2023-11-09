using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] internal float swimmingSpeed;
    [SerializeField] internal float baitedSpeed;
    [SerializeField] internal float retreatSpeed;
    private float speed;
    private bool settingSpeed = false;
    private readonly float _rotateSpeed = 0.8f;
    internal FishController fishController;
    private Animator animator;
    #endregion

    #region Initialization
    public void Initialize(FishController controller)
    {
        fishController = controller;
    }
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #region Public Methods
    public void SwimAround()
    {
        MoveFish(swimmingSpeed);
    }

    public void SwimTowardsTarget()
    {
        if (IsCloseToTarget())
        {
            fishController.SetState(new Retreat(fishController));
        }
        else
        {
            MoveFish(baitedSpeed);
        }
    }

    public IEnumerator Retreat()
    {
        Vector3 direction = GetDirectionAwayFromTarget();
        MoveFish(retreatSpeed, direction);
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
    internal void MoveFish(float newSpeed, Vector3? direction = null)
    {
        speed = newSpeed;
        if (fishController.fishBehaviour != null)
        {
            Vector3 moveDirection = direction ?? transform.forward;
            fishController.fishBehaviour.rigidBody.velocity = moveDirection.normalized * Random.Range(speed / 2, speed * 2);
        }
    }

    internal IEnumerator SetSpeed(float animationSpeed, float duration, float movementSpeed)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;
            float easedTime = Mathf.SmoothStep(0, 1, normalizedTime);
            animator.speed = Mathf.Lerp(animationSpeed, 0, easedTime);
            speed = Mathf.Lerp(movementSpeed, 0, easedTime);
            yield return null;
        }

        animator.speed = animationSpeed;
        speed = movementSpeed;


    }

    internal IEnumerator PullBait()
    {
        Vector3 direction = GetDirectionAwayFromTarget();
        yield return new WaitForSeconds(Random.Range(1, 5));
        MoveFish(baitedSpeed, direction);
    }
    #endregion

    #region Private Methods
    private bool IsCloseToTarget()
    {
        return fishController.fishBehaviour.target != null && Vector3.Distance(transform.position, fishController.fishBehaviour.target.transform.position) <= 0.2f;
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
