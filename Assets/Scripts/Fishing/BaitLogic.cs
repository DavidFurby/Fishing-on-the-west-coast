using System;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    public bool IsPulling { get; private set; }

    #region Serialized Fields
    private GameObject rodTop;
    private Balance balance;
    #endregion

    #region Private Fields
    private AudioSource splashSound;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;
    private FixedJoint fixedJoint;

    #endregion

    #region Events
    public static event Action<Vector3> UpdatePosition;
    #endregion

    public void Start()
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

    void OnDestroy()
    {
        FishingEventController.OnWhileReelingBait -= ReelIn;
        FishingEventController.OnWhileCasting -= Cast;
        WaterCollision.OnEnterSea -= PlaySplashSound;
        FishingEventController.OnWhileFishing -= PullBaitTowardsTarget;

    }

    void Update()
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

    private static void SetState()
    {
        if (FishingController.Instance.fishesOnHook.Count > 0)
        {
            FishingController.Instance.SetState(new InspectFish(FishingController.Instance));
        }
        else
        {
            FishingController.Instance.SetState(new FishingIdle(FishingController.Instance));
        }
    }

    private void DetachBait()
    {
        Destroy(fixedJoint);
    }

    public void Cast()
    {
        DetachBait();
        rigidBody.AddForceAtPosition(forceFactor * FishingController.Instance.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
        if (forceFactor > 0)
        {
            forceFactor -= 0.1f;
        }
    }

    public void ReelIn()
    {
        targetPosition = rodTop.transform.position;

        if (IsCloseToTarget(targetPosition, 2))
        {
            AttachBait();
            SetState();
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

    public void PullBaitTowardsTarget()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IsPulling = true;
            Vector3 direction = (rodTop.transform.position - transform.position).normalized;
            rigidBody.AddForce(direction * 10f, ForceMode.Impulse);
            IsPulling = false;
            if (IsCloseToTarget(targetPosition, 10))
            {
                FishingController.Instance.SetState(new Reeling(FishingController.Instance));
            }
        }
    }
    public void PlaySplashSound()
    {
        splashSound.Play();
    }
}
