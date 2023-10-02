using TMPro;
using UnityEngine;
using System.Collections;

public class DistanceRecord : MonoBehaviour
{
    #region Fields
    [SerializeField] private SeaTileManager seaTileManager;
    private readonly string markerPath = "GameObjects/Environment/DistanceRecordMarker";
    private GameObject currentDistanceRecordMarker;
    private GameObject distanceRecordMarker;
    private TextMeshProUGUI distanceTextUI;
    private FishingSpot from;
    private BaitLogic to;
    internal float distance;
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        from = FindObjectOfType<FishingSpot>();
        to = FindObjectOfType<BaitLogic>();
        distanceTextUI = GetComponentInChildren<TextMeshProUGUI>();
        distanceRecordMarker = Resources.Load<GameObject>(markerPath);
        PlayerEventController.OnEnterFishing += UpdateDistanceRecord;
        PlayerEventController.OnEnterIdle += SpawnDistanceRecordMarker;
        PlayerEventController.OnEnterCasting += SetActive;
        PlayerEventController.OnEnterReeling += SetInactive;
        PlayerEventController.OnEnterReelingFish += SetInactive;
    }

    private void Start()
    {
        distanceTextUI.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        PlayerEventController.OnEnterFishing -= UpdateDistanceRecord;
        PlayerEventController.OnEnterIdle -= SpawnDistanceRecordMarker;
        PlayerEventController.OnEnterCasting -= SetActive;
        PlayerEventController.OnEnterReeling -= SetInactive;
        PlayerEventController.OnEnterReelingFish -= SetInactive;

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
            Vector3 position = new(from.transform.position.x + MainManager.Instance.BestDistance, seaTileManager.currentSeaTilePrefab.transform.position.y +  seaTileManager.currentSeaTilePrefab.GetComponentInChildren<Renderer>().bounds.extents.y, to.transform.position.z);
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
        if (distanceTextUI != null)
        {
            distanceTextUI.gameObject.SetActive(true);
        }
    }
    private void SetInactive()
    {
        if (distanceTextUI != null)
        {
            distanceTextUI.gameObject.SetActive(false);
        }
    }
    #endregion
}
