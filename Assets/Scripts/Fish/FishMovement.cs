using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float swimSpeed;
    public Vector3 direction;
    private Rigidbody rb;
    private GameObject currentBait;

    public enum FishState
    {
        Swimming,
        Baited,
        Hooked
    }

    public FishState state;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Swim), 1, 5);
    }

    void FixedUpdate()
    {
        RotateTowardsBait();
        HookToBait();
    }

    // Rotate towards the bait if baited
    void RotateTowardsBait()
    {
        if (state == FishState.Baited && currentBait != null)
        {
            direction = (currentBait.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float angle = Quaternion.Angle(transform.rotation, targetRotation);
            float timeToComplete = angle / 180;
            float donePercentage = Mathf.Min(1f, Time.fixedDeltaTime / timeToComplete);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, donePercentage);
        }
    }

    // Coroutine to add force to fish's rigidbody
    void Swim()
    {
        rb.AddForce(swimSpeed * Time.fixedDeltaTime * direction, ForceMode.Impulse);
        rb.velocity = Vector3.zero;
    }

    // Move towards bait if triggered
    public void GetBaited(GameObject bait)
    {
        currentBait = bait;
        state = FishState.Baited;
    }

    // Attach fish to bait if hooked
    public void HookToBait()
    {
        if (state == FishState.Hooked && currentBait != null)
        {
            transform.position = currentBait.transform.position;
        }
    }
}