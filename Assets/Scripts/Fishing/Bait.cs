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
    [SerializeField] private FishingControlls fishingControlls;
    private Rigidbody rigidBody;
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private AudioSource reelSound;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        targetPosition = fishingRodTop.transform.position;
        waterLevel = sea.transform.position.y + 1f;
        AttachBait();
    }

    private void FixedUpdate()
    {
        Float();
        ReelIn();
        Shake();
        Cast();
    }

    public void Cast()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Casting)
        {
            rigidBody.isKinematic = false;
            transform.position = transform.position;
            rigidBody.AddForceAtPosition(forceFactor * fishingControlls.throwPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
            if (forceFactor > 0)
            {
                forceFactor -= 0.1f;
            }
        }
    }

    public void ReelIn()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Reeling)
        {
            Vector3 targetPosition = fishingRodTop.transform.position;
            reelSound.Play();
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Attach");
                AttachBait();
            }
            else
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.Translate(ReelInSpeed * Time.fixedDeltaTime * direction, Space.World);
            }
        }
    }

    private void Shake()
    {
        if (catchArea.isInCatchArea)
        {
            splashSound.Play();
            transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * 20) * 0.01f, transform.position.y, transform.position.z);
        }
    }

    private void AttachBait()
    {
        reelSound.Stop();
        rigidBody.isKinematic = true;
        transform.position = targetPosition;
        fishingControlls.fishingStatus = FishingControlls.GetFishingStatus.StandBy;
        fishingControlls.throwPower = 0f;
        forceFactor = 1f;
        catchArea.CollectFish();
    }
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