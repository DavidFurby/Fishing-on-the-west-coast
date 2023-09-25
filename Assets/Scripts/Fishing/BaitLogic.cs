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
        FishingEventController.OnWhileReelingBait += ReelIn;
        FishingEventController.OnWhileCasting += Cast;
        WaterCollision.OnEnterSea += PlaySplashSound;
        FishingEventController.OnWhileFishing += PullBaitTowardsTarget;
        AttachBait();
    }

    private void OnDestroy()
    {
        FishingEventController.OnWhileReelingBait -= ReelIn;
        FishingEventController.OnWhileCasting -= Cast;
        WaterCollision.OnEnterSea -= PlaySplashSound;
        FishingEventController.OnWhileFishing -= PullBaitTowardsTarget;
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

    private void Cast()
    {
        DetachBait();
        rigidBody.AddForceAtPosition(forceFactor * FishingController.Instance.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
        if (forceFactor > 0)
        {
            forceFactor -= 0.1f;
        }
    }

    private void ReelIn()
    {
        targetPosition = rodTop.transform.position;

        if (IsCloseToTarget(targetPosition, 8))
        {
            AttachBait();
            SetState(FishingController.Instance);
        }
        else if (IsFarFromTarget(targetPosition, 100))
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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, FishingController.Instance.reelInSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y + balance.GetBalanceValue() * Time.deltaTime * (balance.GetBalanceValue() >= 0.5 ? 1 : -1) * 10, transform.position.z);
    }

    private void PullBaitTowardsTarget()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IsPulling = true;
            Vector3 direction = (rodTop.transform.position - transform.position).normalized;
            rigidBody.AddForce(direction * 10f, ForceMode.Impulse);
            IsPulling = false;
            if (IsCloseToTarget(targetPosition, 10))
            {
                FishingController.Instance.SetState(new Reeling());
            }
        }
    }

    private void PlaySplashSound()
    {
        splashSound.Play();
    }
}
