using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DistanceRecord : MonoBehaviour
{
    #region Fields
    private readonly string markerPath = "GameObjects/DistanceRecordMarker";
    private GameObject currentDistanceRecordMarker;
    private GameObject distanceRecordMarker;
    [Tooltip("Text UI for displaying the distance value")] private TextMeshProUGUI distanceTextUI;
    [SerializeField][Tooltip("The sea game object")] private GameObject sea;
    [SerializeField][Tooltip("The starting point for calculating the distance")] private GameObject from;
    [SerializeField][Tooltip("The end point for calculating the distance")] private GameObject to;
    private float distance;
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        distanceTextUI = GetComponent<TextMeshProUGUI>();
        distanceRecordMarker = Resources.Load<GameObject>(markerPath);
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
    #endregion

    #region Public Methods
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
            Vector3 position = new(from.transform.position.x + MainManager.Instance.BestDistance, sea.transform.position.y + sea.GetComponent<Renderer>().bounds.extents.y, to.transform.position.z);
            currentDistanceRecordMarker = Instantiate(distanceRecordMarker, position, Quaternion.identity);
            currentDistanceRecordMarker.transform.position = Vector3.Lerp(currentDistanceRecordMarker.transform.position, position, Time.deltaTime);
        }
    }
    #endregion

    #region Private Methods
    //Calculate distance cast
    private void CalculateDistance()
    {
        distance = Vector3.Distance(from.transform.position, to.transform.position);
        int roundedDistance = Mathf.RoundToInt(distance);
        distanceTextUI.text = string.Format("Distance: {0} meter", roundedDistance);
    }
    #endregion
}
