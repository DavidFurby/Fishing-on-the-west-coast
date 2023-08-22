using UnityEngine;

public class SeaLogic : MonoBehaviour
{
    [SerializeField] private int spawnDelay;
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingSystem fishingSystem;

    private FishDisplay[] fishPrefabs;
    private Renderer seaRenderer;
    private Vector3 seaPosition;

    private void Start()
    {
        seaPosition = transform.position;
        seaRenderer = GetComponent<Renderer>();
        fishPrefabs = Resources.LoadAll<FishDisplay>("GameObjects/Fishes");
    }

    public void InvokeSpawnFish()
    {
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    public void StopSpawnFish()
    {
        Debug.Log("SpawnFish");
        CancelInvoke(nameof(SpawnFish));
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
        Vector3 fishSpawnPosition = Vector3.zero;
        Quaternion fishSpawnRotation = Quaternion.identity;

        if (fishingSystem.GetCurrentState() is not FishingIdle)
        {
            float fishSpawnX = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + seaRenderer.bounds.extents.y);

            fishSpawnRotation = fishSpawnX < bait.transform.position.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

            fishSpawnPosition = new Vector3(fishSpawnX, spawnVertical, bait.transform.position.z);
        }

        return (fishSpawnPosition, fishSpawnRotation);
    }

    public void RemoveAllFishes()
    {
        GameObject[] fishes = GameObject.FindGameObjectsWithTag("Fish");
        if (fishes.Length != 0)
        {
            foreach (GameObject fish in fishes)
            {
                ObjectPool.Instance.ReturnToPool(fish);
            }
        }
    }
}
