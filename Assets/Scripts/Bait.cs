using UnityEngine;

public class Bait : MonoBehaviour
{
    [SerializeField] GameObject sea;
    public bool inWater = false;
    private Rigidbody rigidBody;
    private float waterLevel = 0f;
    private float floatHeight = 2f;
    private float bounceDamp = 0.1f;
    private float forceFactor = 1f;
    public float throwPower;
    private readonly float reelInSpeed = 10f;
    Vector3 targetPosition;
    [SerializeField] GameObject fishingRodTop;
    [SerializeField] FishingControlls fishingControlls;
    private void Start()
    {
        targetPosition = fishingRodTop.transform.position;
        rigidBody = GetComponent<Rigidbody>();
        waterLevel = sea.transform.position.y;
        AttachBait();
    }

    private void FixedUpdate()
    {
        Float();
        AddForce();
        ReelIn();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == sea)
        {
            inWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sea)
        {
            inWater = false;
        }
    }
    public void AddForce()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Throwing)
        {
            rigidBody.isKinematic = false;
            transform.position = transform.position;
            rigidBody.AddForceAtPosition(forceFactor * throwPower * Time.fixedDeltaTime * new Vector3(1, 1, 0), rigidBody.position, ForceMode.Impulse);
            if (forceFactor > 0)
            {
                forceFactor -= 0.1f;
            }
            else
            {
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

    private void AttachBait()
    {
        rigidBody.isKinematic = true;
        transform.position = targetPosition;
        fishingControlls.fishingStatus = FishingControlls.GetFishingStatus.StandBy;
    }
    private void Float()
    {
        if (inWater)
        {
            rigidBody.drag = 1f;
            Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
            float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
                Debug.Log(uplift);
                rigidBody.AddForceAtPosition(uplift, actionPoint);
            }
            if (actionPoint.y < waterLevel)
            {

                rigidBody.AddForceAtPosition(Vector3.up * 2, actionPoint);
            }
        }
        else
        {
            rigidBody.drag = 0f;
        }
    }
}
