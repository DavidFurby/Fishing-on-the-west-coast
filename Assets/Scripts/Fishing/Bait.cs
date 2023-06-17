using UnityEngine;
using UnityEngine.UI;

public class Bait : MonoBehaviour
{
    [SerializeField] private GameObject sea;
    public bool inWater = false;
    private float waterLevel;
    private const float FloatHeight = 2f;
    private const float BounceDamp = 0.5f;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingControlls fishingControlls;
    private Rigidbody rigidBody;
    [SerializeField] private Scrollbar balance;
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private AudioSource splashSound;


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
        if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Casting)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForceAtPosition(forceFactor * fishingControlls.castingPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
            if (forceFactor > 0)
            {
                forceFactor -= 0.1f;
            }
        }
    }
    public void ReelIn()
    {
        if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Reeling || fishingControlls.fishingStatus == FishingControlls.FishingStatus.ReelingFish)
        {
            Vector3 targetPosition = fishingRodTop.transform.position;
            if (IsCloseToTarget(targetPosition))
            {
                AttachBait();
                HandleCatch();
            }
            else
            {
                MoveTowardsTarget(targetPosition);
            }
        }
    }

    private bool IsCloseToTarget(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.1f;
    }

    private void HandleCatch()
    {
        if (catchArea.fish != null)
        {
            fishingControlls.HandleCatch();
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        float reelInSpeed = CalculateReelInSpeed();
        transform.Translate(reelInSpeed * Time.fixedDeltaTime * direction, Space.World);
    }

    private float CalculateReelInSpeed()
    {
        float reelInSpeed = 15f;
        if (catchArea.fish != null)
        {
            reelInSpeed /= catchArea.fish.GetComponent<Fish>().Size / 10;
            transform.position = new Vector3(transform.position.x, (transform.position.y + balance.value * Time.deltaTime * (balance.value >= 0.5 ? 1 : -1) * 10), transform.position.z);
        }
        return reelInSpeed;
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
        rigidBody.isKinematic = true;
        transform.position = targetPosition;
        fishingControlls.SetFishingStatus(FishingControlls.FishingStatus.StandBy);
        fishingControlls.castingPower = 0f;
        forceFactor = 1f;
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