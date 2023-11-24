using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private int spawnDelay = 2;
    [SerializeField] private int fishPoolSize = 10;
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
        PlayerEventController.OnEnterIdle += StopSpawnFish;
        PlayerEventController.OnEnterIdle += RemoveAllFishes;
        PlayerEventController.OnEnterFishing += InvokeSpawnFish;
        BaitLogic.UpdatePosition += SetTargetPosition;

    }

    private void UnsubscribeFromEvents()
    {
        PlayerEventController.OnEnterIdle -= StopSpawnFish;
        PlayerEventController.OnEnterIdle -= RemoveAllFishes;
        PlayerEventController.OnEnterFishing -= InvokeSpawnFish;
        BaitLogic.UpdatePosition -= SetTargetPosition;

    }


    private void InvokeSpawnFish()
    {
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    private void StopSpawnFish()
    {
        if (this != null)
            CancelInvoke(nameof(SpawnFish));
    }
    private void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void SpawnFish()
    {
        int randomFishIndex = Random.Range(0, fishPrefabs.Length);

        (Vector3 fishSpawnPosition, Quaternion fishSpawnRotation) = CalculateSpawnPosition();

        GameObject fish = ObjectPool.Instance.GetFromPool(fishPrefabs[randomFishIndex].gameObject, fishPoolSize);
        fish.transform.SetPositionAndRotation(fishSpawnPosition, fishSpawnRotation);
        fish.GetComponent<FishController>().SetState(new Swimming(fish.GetComponent<FishController>()));
        fish.SetActive(true);
    }

    private (Vector3, Quaternion) CalculateSpawnPosition()
    {
        // Determine fish spawn X position
        float fishSpawnX;
        if (PlayerManager.Instance.GetCurrentState() is ReelingFish)
        {
            fishSpawnX = targetPosition.x - 20;
        }
        else
        {
            fishSpawnX = Random.value < 0.5 ? targetPosition.x - 10 : targetPosition.x + 5;
        }

        // Determine fish spawn Y position within sea collider bounds
        float fishSpawnY = Random.Range(seaCollider.bounds.min.y, seaCollider.bounds.max.y);

        // Determine fish spawn rotation based on X position
        Quaternion fishSpawnRotation = fishSpawnX < targetPosition.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

        // Create fish spawn position vector
        Vector3 fishSpawnPosition = new(fishSpawnX, fishSpawnY, targetPosition.z);

        return (fishSpawnPosition, fishSpawnRotation);
    }


    private void RemoveAllFishes()
    {
        
        ObjectPool.Instance.RemovePool("Fish");
    }
}
