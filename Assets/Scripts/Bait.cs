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
    public bool thrown;
    public bool reelingIn;
    [SerializeField] GameObject fishingRodTop;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        waterLevel = sea.transform.position.y;
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
        if (thrown)
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
                Debug.Log("false");
                thrown = false;
            }
        }
    }
    private void ReelIn()
    {
        if (reelingIn)
        {
            Vector3 targetPosition = fishingRodTop.transform.position;
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                rigidBody.isKinematic = true;
                transform.position = targetPosition;
                reelingIn = false;
            }
            else
            {
                float speed = 8f;
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.Translate(speed * Time.fixedDeltaTime * direction, Space.World);
            }
        }
    }
    private void Float()
    {
        if (inWater)
        {
            Vector3 actionPoint = transform.position + transform.TransformDirection(Vector3.down);
            float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - rigidBody.velocity.y * bounceDamp);
                rigidBody.AddForceAtPosition(uplift, actionPoint);
            }
            if (actionPoint.y < waterLevel)
            {
                Debug.Log("below water");
                actionPoint.y = waterLevel;
            }
        }
    }
}
