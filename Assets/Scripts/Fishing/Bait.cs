using UnityEngine;

public class Bait : MonoBehaviour
{
    [SerializeField] private GameObject sea;
    public bool inWater = false;
    private float waterLevel;
    private const float FloatHeight = 2f;
    private const float BounceDamp = 0.5f;
    private float forceFactor = 1f;
    private const float ReelInSpeed = 15f;
    private Vector3 targetPosition;
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingControls fishingControls;
    private Rigidbody rigidBody;
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private AudioSource reelSound;

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetPosition = fishingRodTop.transform.position;
        waterLevel = sea.transform.position.y + 1f;
        AttachBait();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Float();
        ReelIn();
        Shake();
        Cast();
    }

    // Cast the bait
    public void Cast()
    {
        if (fishingControls != null && fishingControls.fishingStatus == FishingControls.GetFishingStatus.Casting)
        {
            rigidBody.isKinematic = false;
            transform.position = transform.position;
            rigidBody.AddForceAtPosition(forceFactor * fishingControls.throwPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
            if (forceFactor > 0)
            {
                forceFactor -= 0.1f;
            }
        }
    }

    // Reel in the bait
    public void ReelIn()
    {
        if (fishingControls != null && fishingControls.fishingStatus == FishingControls.GetFishingStatus.Reeling)
        {
            Vector3 targetPosition = fishingRodTop.transform.position;
            reelSound.Play();
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                AttachBait();
            }
            else
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.Translate(ReelInSpeed * Time.fixedDeltaTime * direction, Space.World);
            }
        }
    }

    // Shake the bait when in catch area
    private void Shake()
    {
        if (catchArea != null && catchArea.isInCatchArea)
        {
            splashSound.Play();
            transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * 20) * 0.01f, transform.position.y, transform.position.z);
        }
    }

    // Attach the bait to the fishing rod
    private void AttachBait()
    {
        reelSound.Stop();
        rigidBody.isKinematic = true;
        transform.position = targetPosition;

        if (fishingControls != null)
        {
            fishingControls.fishingStatus = FishingControls.GetFishingStatus.StandBy;
            fishingControls.throwPower = 0f;
        }

        forceFactor = 1f;

        if (catchArea != null)
        {
            catchArea.CollectFish();
        }
    }

    // Make the bait float on water
    private void Float()
    {
        if (inWater)
        {
            rigidBody.drag = 2f;
            Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
            float forceFactor = 1f - ((actionPoint.y - waterLevel) / FloatHeight);
            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * BounceDamp);
                rigidBody.AddForceAtPosition(uplift, actionPoint);
            }
        }
        else
        {
            rigidBody.drag = 0f;
        }
    }
}