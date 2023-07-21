using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaitLogic : MonoBehaviour
{
    [SerializeField] private GameObject sea;
    [SerializeField] private GameObject fishingRodTop;
    [SerializeField] private FishingSystem system;
    [SerializeField] private Scrollbar balance;
    [SerializeField] private AudioSource splashSound;
    [SerializeField] private TextMeshProUGUI distanceTextUI;
    [SerializeField] private GameObject distanceRecordMarker;

    [HideInInspector] public bool inWater = false;
    private GameObject currentDistanceRecordMarker;
    private float distance;
    private float waterLevel;
    private const float FloatHeight = 2f;
    private const float BounceDamp = 0.5f;
    private float forceFactor = 1f;
    private Vector3 targetPosition;
    private Rigidbody rigidBody;


    private void Start()
    {
        distanceTextUI.gameObject.SetActive(false);
        rigidBody = GetComponent<Rigidbody>();
        targetPosition = fishingRodTop.transform.position;
        waterLevel = sea.transform.position.y + 1f;
        AttachBait();
        SpawnDistanceRecordMarker();
    }

    private void FixedUpdate()
    {
        CalculateDistance();
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
            if (system.caughtFishes.Count > 0)
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
    public void UpdateDistanceRecord()
    {
        if (MainManager.Instance.game.BestDistance < distance)
        {
            MainManager.Instance.game.BestDistance = distance;
            SpawnDistanceRecordMarker();
        }
    }
    public void SpawnDistanceRecordMarker()
    {
        if (MainManager.Instance.game.BestDistance != 0)
        {
            if (currentDistanceRecordMarker != null)
            {
                Destroy(currentDistanceRecordMarker);
            }
            Vector3 position = new(fishingRodTop.transform.position.x + MainManager.Instance.game.BestDistance, sea.transform.position.y + sea.GetComponent<Renderer>().bounds.extents.y, transform.position.z);
            currentDistanceRecordMarker = Instantiate(distanceRecordMarker, position, Quaternion.identity);

        }

    }
    //Calculate distance cast
    private void CalculateDistance()
    {
        bool isStandBy = system.GetCurrentState() is Idle;
        distanceTextUI.gameObject.SetActive(!isStandBy);
        if (!isStandBy)
        {
            distance = Vector3.Distance(fishingRodTop.transform.position, transform.position);
            distanceTextUI.text = $"Distance: {distance:F2} meter";
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
        if (system.catchArea.IsInCatchArea)
        {
            splashSound.Play();
            transform.position = new Vector3(transform.position.x + Mathf.Sin(Time.time * 20) * 0.01f, transform.position.y, transform.position.z);
        }
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
    public void Float()
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