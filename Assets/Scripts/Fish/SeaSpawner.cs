using UnityEngine;

public class SeaSpawner : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private FishDisplay[] fishPrefabs;
    [SerializeField] private int spawnDelay;
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingSystem fishingSystem;

    // Private fields
    private Renderer seaRenderer;
    private Vector3 seaPosition;
    private float fishSpawnDirection;
    private Quaternion fishSpawnRotation;
    private Vector3 fishSpawnPosition;

    private void Start()
    {
        // Get the size and center of the sea object
        seaPosition = transform.position;
        seaRenderer = GetComponent<Renderer>();
    }

    // Destroy the fish if it exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (other.transform.position.y > seaPosition.y + seaRenderer.bounds.extents.y)
            {
                other.attachedRigidbody.AddForce(Vector3.down * 2, ForceMode.Impulse);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
    public void InvokeSpawnFish()
    {
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    // Spawn a fish at a random position
    private void SpawnFish()
    {
        // Choose a random fish from the array
        int randomFishIndex = Random.Range(0, fishPrefabs.Length);

        CalculateSpawnPosition();

        // Instantiate the fish
        GameObject fish = Instantiate(fishPrefabs[randomFishIndex].gameObject, fishSpawnPosition, fishSpawnRotation);
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
                Destroy(fish);
            }
        }
    }
}
