
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


    private void Start()
    {
        distanceTextUI.gameObject.SetActive(true);
        SpawnDistanceRecordMarker();

    }
    void FixedUpdate()
    {
        CalculateDistance();

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
        distance = Vector3.Distance(fishingRodTop.transform.position, transform.position);
        distanceTextUI.text = $"Distance: {distance:F2} meter";
    }
}