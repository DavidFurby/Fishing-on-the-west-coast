
using TMPro;
using UnityEngine;
public class DistanceRecord : MonoBehaviour
{

    private GameObject currentDistanceRecordMarker;
    [SerializeField] private GameObject distanceRecordMarker;
    [SerializeField] private TextMeshProUGUI distanceTextUI;
    [SerializeField] private GameObject sea;
    [SerializeField] private GameObject fishingRodTop;

    private float distance;

    void OnEnable()
    {
        FishingController.OnEnterFishing += UpdateDistanceRecord;
    }
    private void Start()
    {
        distanceTextUI.gameObject.SetActive(true);
        SpawnDistanceRecordMarker();

    }

    void OnDisable()
    {
        FishingController.OnEnterFishing -= UpdateDistanceRecord;
    }
    void FixedUpdate()
    {
        CalculateDistance();

    }
    public void UpdateDistanceRecord()
    {
        if (MainManager.Instance.BestDistance < distance)
        {
            MainManager.Instance.BestDistance = distance;
            SpawnDistanceRecordMarker();
        }
    }
    public void SpawnDistanceRecordMarker()
    {
        if (MainManager.Instance.BestDistance != 0)
        {
            if (currentDistanceRecordMarker != null)
            {
                Destroy(currentDistanceRecordMarker);
            }
            Vector3 position = new(fishingRodTop.transform.position.x + MainManager.Instance.BestDistance, sea.transform.position.y + sea.GetComponent<Renderer>().bounds.extents.y, transform.position.z);
            currentDistanceRecordMarker = Instantiate(distanceRecordMarker, position, Quaternion.identity);

        }

    }
    //Calculate distance cast
    private void CalculateDistance()
    {
        distance = Vector3.Distance(fishingRodTop.transform.position, transform.position);
        distanceTextUI.text = $"Distance: {distance:F2} meter";
    }
}