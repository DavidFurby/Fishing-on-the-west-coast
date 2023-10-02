using System;
using UnityEngine;

public class BaitLogic : MonoBehaviour
{
    public bool IsPulling { get; private set; }

    private GameObject rodTop;
    private Balance balance;
    private AudioSource splashSound;

    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;
    private FixedJoint fixedJoint;
    public static event Action<Vector3> UpdatePosition;

    private void Start()
    {
        balance = FindObjectOfType<Balance>();
        splashSound = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        rodTop = GameObject.FindGameObjectWithTag("RodTop");
        PlayerEventController.OnWhileReelingBait += ReelIn;
        PlayerEventController.OnWhileCasting += Cast;
        WaterCollision.OnEnterSea += PlaySplashSound;
        PlayerEventController.OnWhileFishing += PullBaitTowardsTarget;
        AttachBait();
    }

    private void OnDestroy()
    {
        PlayerEventController.OnWhileReelingBait -= ReelIn;
        PlayerEventController.OnWhileCasting -= Cast;
        WaterCollision.OnEnterSea -= PlaySplashSound;
        PlayerEventController.OnWhileFishing -= PullBaitTowardsTarget;
    }

    private void Update()
    {
        UpdatePosition?.Invoke(transform.position);
    }

    private void AttachBait()
    {
        transform.position = rodTop.transform.position;

        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = rodTop.GetComponent<Rigidbody>();
        forceFactor = 1;
    }

    private static void SetState(FishingController fishingController)
    {
        if (fishingController.fishesOnHook.Count > 0)
        {
            fishingController.SetState(new InspectFish());
        }
        else
        {
            fishingController.SetState(new FishingIdle());
        }
    }

    private void DetachBait()
    {
        Destroy(fixedJoint);
    }

    private void Cast(Transform playerTransform)
    {
        DetachBait();
        float upwardFactor = 1f;
        Vector3 forceDirection = playerTransform.forward + (Vector3.up * upwardFactor);
        rigidBody.AddForceAtPosition(forceFactor * PlayerController.Instance.castingPower * Time.fixedDeltaTime * forceDirection, rigidBody.position, ForceMode.Impulse);
        if (forceFactor > 0)
        {
            forceFactor -= 0.1f;
        }
    }

    private void ReelIn()
    {
        targetPosition = rodTop.transform.position;

        if (IsCloseToTarget(targetPosition, 4))
        {
            AttachBait();
            SetState(PlayerController.Instance);
        }
        else if (IsFarFromTarget(targetPosition, 100) && PlayerController.Instance.GetCurrentState() is Reeling)
        {
            transform.position = new Vector3(targetPosition.x + 50, transform.position.y, targetPosition.z);
        }
        else
        {
            MoveTowardsTarget(targetPosition);
        }
    }

    private bool IsCloseToTarget(Vector3 targetPosition, float distance)
    {
        return Vector3.Distance(transform.position, targetPosition) < distance;
    }

    private bool IsFarFromTarget(Vector3 targetPosition, float distance)
    {
        return Vector3.Distance(transform.position, targetPosition) > distance;
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        rigidBody.AddForce(PlayerController.Instance.reelInSpeed * MainManager.Instance.Inventory.EquippedRod.reelInSpeed * direction, ForceMode.Impulse);
        float newY = transform.position.y + balance.GetBalanceValue() * Time.deltaTime * (balance.GetBalanceValue() >= 0.5 ? 1 : -1);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void PullBaitTowardsTarget()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            IsPulling = true;
            Vector3 direction = (rodTop.transform.position - transform.position).normalized + new Vector3(0, 0.1f, 0);
            rigidBody.AddForce(direction * 1.5f, ForceMode.Impulse);
            IsPulling = false;
            if (IsCloseToTarget(targetPosition, 20))
            {
                PlayerController.Instance.SetState(new Reeling());
            }
        }
    }

    private void PlaySplashSound()
    {
        splashSound.Play();
    }
}
