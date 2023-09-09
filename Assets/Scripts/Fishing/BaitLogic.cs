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
        FishingController.OnWhileReelingBait += ReelIn;
        FishingController.OnWhileCasting += Cast;
        WaterCollision.OnEnterSea += PlaySplashSound;
        FishingController.OnWhileFishing += PullBaitTowardsTarget;
        AttachBait();
    }

    void OnDestroy()
    {
        FishingController.OnWhileReelingBait -= ReelIn;
        FishingController.OnWhileCasting -= Cast;
        WaterCollision.OnEnterSea -= PlaySplashSound;
        FishingController.OnWhileFishing -= PullBaitTowardsTarget;

    }

    void Update()
    {
        UpdatePosition?.Invoke(transform.position);
    }

    private void AttachBait()
    {
        transform.position = rodTop.transform.position;

        // Create a FixedJoint component and attach it to the bait
        fixedJoint = gameObject.AddComponent<FixedJoint>();

        // Set the connected body of the FixedJoint to be the rodTop
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
        // Destroy the FixedJoint component to detach the bait from the rodTop
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
        else
        {
            MoveTowardsTarget(targetPosition);
        }
    }

    private bool IsCloseToTarget(Vector3 targetPosition, float distance)
    {
        return Vector3.Distance(transform.position, targetPosition) < distance;
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, FishingController.Instance.reelInSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(transform.position.x, (transform.position.y + balance.GetBalanceValue() * Time.deltaTime * (balance.GetBalanceValue() >= 0.5 ? 1 : -1) * 10), transform.position.z);

    }

    public void PullBaitTowardsTarget()
    {
        // Execute the original method logic
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
