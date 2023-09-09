using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DistanceRecord : MonoBehaviour
{
    #region Fields
    private readonly string markerPath = "GameObjects/Environment/DistanceRecordMarker";
    private GameObject currentDistanceRecordMarker;
    private GameObject distanceRecordMarker;
    [Tooltip("Text UI for displaying the distance value")] private TextMeshProUGUI distanceTextUI;
   [SerializeField] [Tooltip("The sea game object")] private SeaLogic sea;
    [Tooltip("The starting point for calculating the distance")] private FishingSpot from;
    [Tooltip("The end point for calculating the distance")] private BaitLogic to;
    private float distance;
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        sea = GameObject.FindObjectOfType<SeaLogic>();
        from = GameObject.FindObjectOfType<FishingSpot>();
        to = GameObject.FindObjectOfType<BaitLogic>();
        distanceTextUI = GetComponentInChildren<TextMeshProUGUI>();
        distanceRecordMarker = Resources.Load<GameObject>(markerPath);
        FishingController.OnEnterFishing += UpdateDistanceRecord;
        FishingController.OnEnterIdle += SpawnDistanceRecordMarker;
        FishingController.OnEnterIdle += SetActive;
    }

    private void Start()
    {
        distanceTextUI.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        FishingController.OnEnterFishing -= UpdateDistanceRecord;
        FishingController.OnEnterIdle -= SpawnDistanceRecordMarker;
        FishingController.OnEnterIdle -= SetActive;
    }

    void Update()
    {
        CalculateDistance();
    }
    #endregion

    #region Public Methods
    private void UpdateDistanceRecord()
    {
        if (MainManager.Instance.BestDistance < distance)
        {
            MainManager.Instance.BestDistance = distance;
            SpawnDistanceRecordMarker();
        }
    }


    private void SpawnDistanceRecordMarker()
    {
        if (MainManager.Instance.BestDistance != 0)
        {
            if (currentDistanceRecordMarker != null)
            {
                Destroy(currentDistanceRecordMarker);
            }
            Vector3 position = new(from.transform.position.x + MainManager.Instance.BestDistance, sea.transform.position.y + sea.GetComponent<Renderer>().bounds.extents.y, to.transform.position.z);
            currentDistanceRecordMarker = Instantiate(distanceRecordMarker, position, Quaternion.identity);
            StartCoroutine(MoveMarker(position, 5f));
        }
    }

    private IEnumerator MoveMarker(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(currentDistanceRecordMarker.transform.position, targetPosition) > 0.01f)
        {
            currentDistanceRecordMarker.transform.position = Vector3.Lerp(currentDistanceRecordMarker.transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
    }

    #endregion

    #region Private Methods
    //Calculate distance cast
    private void CalculateDistance()
    {
        if (from != null && to != null)
        {
            distance = Vector3.Distance(from.transform.position, to.transform.position);
            int roundedDistance = Mathf.RoundToInt(distance);
            distanceTextUI.text = string.Format("Distance: {0:F1} meter", roundedDistance);
        }
    }

    private void SetActive()
    {
        distanceTextUI.gameObject.SetActive(true);
    }

}
#endregion