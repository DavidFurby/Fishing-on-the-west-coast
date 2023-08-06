using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingSystem system;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private Scrollbar balance;
    [SerializeField] public GameObject bait;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;
    private FixedJoint fixedJoint;


    public void Start()
    {
        rigidBody = bait.GetComponent<Rigidbody>();
        AttachBait();
    }

    private void AttachBait()
    {
        bait.transform.position = fishingRodTop.transform.position;
        // Create a FixedJoint component and attach it to the bait
        fixedJoint = bait.gameObject.AddComponent<FixedJoint>();

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
        rigidBody.AddForceAtPosition(forceFactor * system.fishingRodLogic.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
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
                system.SetState(new Idle(system));
            }
        }
        else
        {
            MoveTowardsTarget(targetPosition);
        }

    }
    private bool IsCloseToTarget(Vector3 targetPosition)
    {
        return Vector3.Distance(bait.transform.position, targetPosition) < 0.5f;
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - bait.transform.position).normalized;
        bait.transform.position = new Vector3(bait.transform.position.x, (bait.transform.position.y + balance.value * Time.deltaTime * (balance.value >= 0.5 ? 1 : -1) * 10), bait.transform.position.z);
        bait.transform.Translate(system.fishingRodLogic.reelInSpeed * Time.fixedDeltaTime * direction, Space.World);
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
