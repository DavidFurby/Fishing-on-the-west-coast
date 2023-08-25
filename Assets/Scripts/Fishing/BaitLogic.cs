using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingController system;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private Scrollbar balance;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;
    private FixedJoint fixedJoint;


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        FishingController.OnReelInBait += ReelIn;
        FishingController.OnWhileCasting += Cast;
        AttachBait();
    }

    void OnDestroy()
    {
        FishingController.OnReelInBait -= ReelIn;
        FishingController.OnWhileCasting -= Cast;

    }
    void OnDisable()
    {
        FishingController.OnReelInBait -= ReelIn;
        FishingController.OnWhileCasting -= Cast;

    }

    private void AttachBait()
    {
        transform.position = fishingRodTop.transform.position;
        // Create a FixedJoint component and attach it to the bait
        fixedJoint = gameObject.AddComponent<FixedJoint>();

        // Set the connected body of the FixedJoint to be the fishingRodTop
        fixedJoint.connectedBody = fishingRodTop.GetComponent<Rigidbody>();

        // Reset other values
        ResetValues();
    }

    private void DetachBait()
    {
        // Destroy the FixedJoint component to detach the bait from the fishingRodTop
        Destroy(fixedJoint);
    }

    public void Cast()
    {
        DetachBait();
        rigidBody.AddForceAtPosition(forceFactor * system.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
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
            if (system.fishesOnHook.Count > 0)
            {
                system.HandleCatch();
            }
            else
            {
                system.SetState(new FishingIdle(system));
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
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position = new Vector3(transform.position.x, (transform.position.y + balance.value * Time.deltaTime * (balance.value >= 0.5 ? 1 : -1) * 10), transform.position.z);
        transform.Translate(system.reelInSpeed * Time.fixedDeltaTime * direction, Space.World);
    }

    public void Shake()
    {
        splashSound.Play();
    }

    private void ResetValues()
    {
        forceFactor = 1f;
    }
}
