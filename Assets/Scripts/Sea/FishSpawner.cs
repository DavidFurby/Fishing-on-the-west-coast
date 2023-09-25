using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private int spawnDelay = 3;
    [SerializeField] private int fishPoolSize = 5;
    private Vector3 targetPosition;
    private FishDisplay[] fishPrefabs;
    private BoxCollider seaCollider;
    private void Start()
    {
        seaCollider = GetComponent<BoxCollider>();
        fishPrefabs = Resources.LoadAll<FishDisplay>("GameObjects/Fishes");
        SubscribeToEvents();
    }
    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        FishingEventController.OnEnterIdle += StopSpawnFish;
        FishingEventController.OnEnterIdle += RemoveAllFishes;
        FishingEventController.OnEnterFishing += InvokeSpawnFish;
        BaitLogic.UpdatePosition += SetTargetPosition;

    }

    private void UnsubscribeFromEvents()
    {
        FishingEventController.OnEnterIdle -= StopSpawnFish;
        FishingEventController.OnEnterIdle -= RemoveAllFishes;
        FishingEventController.OnEnterFishing -= InvokeSpawnFish;
        BaitLogic.UpdatePosition -= SetTargetPosition;

    }


    public void InvokeSpawnFish()
    {
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    public void StopSpawnFish()
    {
        if (this != null)
            CancelInvoke(nameof(SpawnFish));
    }
    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void SpawnFish()
    {
        int randomFishIndex = Random.Range(0, fishPrefabs.Length);

        (Vector3 fishSpawnPosition, Quaternion fishSpawnRotation) = CalculateSpawnPosition();

        GameObject fish = ObjectPool.Instance.GetFromPool(fishPrefabs[randomFishIndex].gameObject, fishPoolSize);
        fish.transform.SetPositionAndRotation(fishSpawnPosition, fishSpawnRotation);
        fish.SetActive(true);
    }

    private (Vector3, Quaternion) CalculateSpawnPosition()
    {
        float fishSpawnX = Random.value < 0.5 ? targetPosition.x - 5 : targetPosition.x + 5;
        float fishSpawnY = Random.Range(seaCollider.bounds.min.y, seaCollider.bounds.max.y);

        Quaternion fishSpawnRotation = fishSpawnX < targetPosition.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
        Vector3 fishSpawnPosition = new(fishSpawnX, fishSpawnY, targetPosition.z);

        return (fishSpawnPosition, fishSpawnRotation);
    }

    public void RemoveAllFishes()
    {
        GameObject[] fishes = GameObject.FindGameObjectsWithTag("Fish");
        if (fishes.Length != 0)
            foreach (GameObject fish in fishes)
                ObjectPool.Instance.ReturnToPool(fish);

    }
}
