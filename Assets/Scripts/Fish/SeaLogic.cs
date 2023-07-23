using UnityEngine;

public class SeaLogic : MonoBehaviour
{
    [SerializeField] private int spawnDelay;
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingSystem fishingSystem;

    private FishDisplay[] fishPrefabs;
    private Renderer seaRenderer;
    private Vector3 seaPosition;
    private float fishSpawnDirection;
    private Quaternion fishSpawnRotation;
    private Vector3 fishSpawnPosition;
    private readonly float waterLevel;

    private void Start()
    {
        seaPosition = transform.position;
        seaRenderer = GetComponent<Renderer>();
        fishPrefabs = Resources.LoadAll<FishDisplay>("SpawnableFishes");
    }

    public void InvokeSpawnFish()
    {
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    public void StopSpawnFish()
    {
        CancelInvoke(nameof(SpawnFish));
    }

    private void SpawnFish()
    {
        int randomFishIndex = Random.Range(0, fishPrefabs.Length);

        CalculateSpawnPosition();

        GameObject fish = ObjectPool.Instance.GetFromPool(fishPrefabs[randomFishIndex].gameObject);
        fish.transform.SetPositionAndRotation(fishSpawnPosition, fishSpawnRotation);
        fish.SetActive(true);
    }

    private void CalculateSpawnPosition()
    {
        if (fishingSystem.GetCurrentState() is not Idle)
        {
            fishSpawnDirection = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + seaRenderer.bounds.extents.y);

            fishSpawnRotation = fishSpawnDirection < bait.transform.position.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

            fishSpawnPosition = new Vector3(fishSpawnDirection, spawnVertical, bait.transform.position.z);
        }
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

    public void Float(Rigidbody rigidBody, bool inWater, float FloatHeight, float BounceDamp)
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
