using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    [SerializeField] private SeaLogic seaLogic;
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingSystem system;
    [SerializeField] private AudioSource splashSound;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;
    [SerializeField] private Scrollbar balance;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetPosition = fishingRodTop.transform.position;
        AttachBait();
    }

    public void Cast()
    {
        rigidBody.isKinematic = false;
        rigidBody.AddForceAtPosition(forceFactor * system.fishingRodLogic.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
        if (forceFactor > 0)
        {
            forceFactor -= 0.1f;
        }
    }
    public void ReelIn()
    {
        Vector3 targetPosition = fishingRodTop.transform.position;
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
        return Vector3.Distance(transform.position, targetPosition) < 1f;
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position = new Vector3(transform.position.x, (transform.position.y + balance.value * Time.deltaTime * (balance.value >= 0.5 ? 1 : -1) * 10), transform.position.z);
        transform.Translate(system.fishingRodLogic.reelInSpeed * Time.fixedDeltaTime * direction, Space.World);
    }

    public void Shake()
    {
        splashSound.Play();
    }

    private void AttachBait()
    {
        rigidBody.isKinematic = true;
        transform.position = targetPosition;
        ResetValues();
    }

    private void ResetValues()
    {
        forceFactor = 1f;
    }
}