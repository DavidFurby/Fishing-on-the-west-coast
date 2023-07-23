using UnityEngine;

public class SeaLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private int spawnDelay;
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingSystem fishingSystem;

    // Private fields
    private FishDisplay[] fishPrefabs;
    private Renderer seaRenderer;
    private Vector3 seaPosition;
    private float fishSpawnDirection;
    private Quaternion fishSpawnRotation;
    private Vector3 fishSpawnPosition;
    private readonly float waterLevel;


    private void Start()
    {
        // Get the size and center of the sea object
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

    // Spawn a fish at a random position
    private void SpawnFish()
    {
        // Choose a random fish from the array
        int randomFishIndex = Random.Range(0, fishPrefabs.Length);

        CalculateSpawnPosition();

        // Instantiate the fish
        GameObject fish = ObjectPool.Instance.GetFromPool(fishPrefabs[randomFishIndex].gameObject);
        fish.transform.SetPositionAndRotation(fishSpawnPosition, fishSpawnRotation);
        fish.SetActive(true);
    }
    // Calculate the spawn position of the fish
    private void CalculateSpawnPosition()
    {
        if (fishingSystem.GetCurrentState() is not Idle)
        {
            // Calculate horizontal and vertical spawn position
            fishSpawnDirection = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + seaRenderer.bounds.extents.y);

            // Calculate spawn rotation based on horizontal position
            fishSpawnRotation = fishSpawnDirection < bait.transform.position.x ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

            // Calculate spawn position
            fishSpawnPosition = new Vector3(fishSpawnDirection, spawnVertical, bait.transform.position.z);
        }
    }


    // Remove all fishes from the scene
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