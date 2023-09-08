using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeaLogic : MonoBehaviour
{
    [SerializeField] private int spawnDelay;
    private Vector3 targetPosition;
    private FishDisplay[] fishPrefabs;
    private Renderer seaRenderer;


    private void Start()
    {
        TryGetComponent<Renderer>(out seaRenderer);
        fishPrefabs = Resources.LoadAll<FishDisplay>("GameObjects/Fishes");
        SubscribeToEvents();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        FishingController.OnEnterIdle += StopSpawnFish;
        FishingController.OnEnterIdle += RemoveAllFishes;
        FishingController.OnEnterFishing += InvokeSpawnFish;
        BaitLogic.UpdatePosition += SetTargetPosition;

    }

    private void UnsubscribeFromEvents()
    {
        FishingController.OnEnterIdle -= StopSpawnFish;
        FishingController.OnEnterIdle -= RemoveAllFishes;
        FishingController.OnEnterFishing -= InvokeSpawnFish;
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

        GameObject fish = ObjectPool.Instance.GetFromPool(fishPrefabs[randomFishIndex].gameObject);
        fish.transform.SetPositionAndRotation(fishSpawnPosition, fishSpawnRotation);
        fish.SetActive(true);
    }

    private (Vector3, Quaternion) CalculateSpawnPosition()
    {
        float fishSpawnX = Random.value < 0.5 ? targetPosition.x - 5 : targetPosition.x + 5;
        float spawnVertical = Random.Range(transform.position.y, transform.position.y + seaRenderer.bounds.extents.y);

        Quaternion fishSpawnRotation = fishSpawnX < targetPosition.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

        Vector3 fishSpawnPosition = new(fishSpawnX, spawnVertical, targetPosition.z);


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