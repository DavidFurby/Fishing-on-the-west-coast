using UnityEngine;

public class Bait : MonoBehaviour
{
    [SerializeField] GameObject sea;
    public bool inWater = false;
    private float waterLevel;
    private readonly float floatHeight = 2f;
    private readonly float bounceDamp = 0.5f;
    private float forceFactor = 1f;
    private readonly float reelInSpeed = 15f;
    Vector3 targetPosition;
    [SerializeField] GameObject fishingRodTop;
    [SerializeField] FishingControlls fishingControlls;
    private Rigidbody rigidBody;
    [SerializeField] CatchArea catchArea;
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
        Throw();
    }


    public void Throw()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Throwing)
        {
            rigidBody.isKinematic = false;
            transform.position = transform.position;
            rigidBody.AddForceAtPosition(forceFactor * fishingControlls.throwPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
            if (forceFactor > 0)
            {
                forceFactor -= 0.1f;
            }
            else
            {
                forceFactor = 1f;
                fishingControlls.throwPower = 0f;
                fishingControlls.fishingStatus = FishingControlls.GetFishingStatus.Fishing;
            }
        }
    }
    private void ReelIn()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Reeling)
        {
            Vector3 targetPosition = fishingRodTop.transform.position;
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                AttachBait();
            }
            else
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.Translate(reelInSpeed * Time.fixedDeltaTime * direction, Space.World);
            }
        }
    }
    private void Shake()
    {
        if (catchArea.isInCatchArea)
        {
            transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * 20) * 0.01f, transform.position.y, transform.position.z);
        }
    }

    private void AttachBait()
    {
        rigidBody.isKinematic = true;
        transform.position = targetPosition;
        fishingControlls.fishingStatus = FishingControlls.GetFishingStatus.StandBy;
        fishingControlls.throwPower = 0f;
    }
    private void Float()
    {
        if (inWater)
        {
            rigidBody.drag = 2f;
            Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
            float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
                rigidBody.AddForceAtPosition(uplift, actionPoint);
            }
        }
        else
        {
            rigidBody.drag = 0f;
        }
    }
}
