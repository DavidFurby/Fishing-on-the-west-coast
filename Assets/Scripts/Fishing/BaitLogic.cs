using System;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    public bool IsPulling { get; private set; }

    #region Serialized Fields
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private Scrollbar balance;
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
        splashSound = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        FishingController.OnReelInBait += ReelIn;
        FishingController.OnWhileCasting += Cast;
        WaterCollision.OnEnterSea += PlaySplashSound;
        FishingController.OnWhileFishing += PullBaitTowardsTarget;
        AttachBait();
    }

    void OnDisable()
    {
        FishingController.OnReelInBait -= ReelIn;
        FishingController.OnWhileCasting -= Cast;
        WaterCollision.OnEnterSea -= PlaySplashSound;
        FishingController.OnWhileFishing -= PullBaitTowardsTarget;

    }

    void Update()
    {
        UpdatePosition.Invoke(transform.position);

    }

    private void AttachBait()
    {
        transform.position = fishingRodTop.transform.position;

        // Create a FixedJoint component and attach it to the bait
        fixedJoint = gameObject.AddComponent<FixedJoint>();

        // Set the connected body of the FixedJoint to be the fishingRodTop
        fixedJoint.connectedBody = fishingRodTop.GetComponent<Rigidbody>();
    }

    private void DetachBait()
    {
        // Destroy the FixedJoint component to detach the bait from the fishingRodTop
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
        targetPosition = fishingRodTop.transform.position;

        if (IsCloseToTarget(targetPosition))
        {
            AttachBait();

            if (FishingController.Instance.fishesOnHook.Count > 0)
            {
                FishingController.Instance.SetState(new InspectFish(FishingController.Instance));
            }
            else
            {
                FishingController.Instance.SetState(new FishingIdle(FishingController.Instance));
            }
        }
        else
        {
            MoveTowardsTarget(targetPosition);
        }
    }

    private bool IsCloseToTarget(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.5f;
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, FishingController.Instance.reelInSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(transform.position.x, (transform.position.y + balance.value * Time.deltaTime * (balance.value >= 0.5 ? 1 : -1) * 10), transform.position.z);

    }

    public void PullBaitTowardsTarget()
    {

        // Execute the original method logic
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Timer");
            IsPulling = true;
            Vector3 direction = (fishingRodTop.transform.position - transform.position).normalized;
            rigidBody.AddForce(direction * 10f, ForceMode.Impulse);
            PlaySplashSound();
            IsPulling = false;
        }

    }
    public void PlaySplashSound()
    {
        splashSound.Play();
    }
}
