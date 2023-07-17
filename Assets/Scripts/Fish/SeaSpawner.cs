using UnityEditor.Rendering;
using UnityEngine;

public class SeaSpawner : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private FishDisplay[] fishPrefabs;
    [SerializeField] private int spawnDelay;
    [SerializeField] private GameObject bait;
    [SerializeField] private FishingController fishingController;

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
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    private void Update()
    {
        // Remove all fishes if the current state is StandBy
        if (fishingController.GetCurrentState() is Idle)
        {
            RemoveAllFishes();
        }
    }

    // Spawn a fish at a random position
    private void SpawnFish()
    {
        // Spawn a fish if the current state is not StandBy
        if (fishingController.GetCurrentState() is not Idle)
        {
            // Choose a random fish from the array
            int randomFishIndex = Random.Range(0, fishPrefabs.Length);

            CalculateSpawnPosition();

            // Instantiate the fish
            GameObject fish = Instantiate(fishPrefabs[randomFishIndex].gameObject, fishSpawnPosition, fishSpawnRotation);
            SetDirection(fish);
        }
    }

    // Set the direction of the fish based on its horizontal position
    private void SetDirection(GameObject fish)
    {
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        fishMovement.direction = fishSpawnDirection == seaPosition.x ? Vector3.right : Vector3.left;
    }

    // Calculate the spawn position of the fish
    private void CalculateSpawnPosition()
    {
        if (fishingController.GetCurrentState() is not Idle)
        {
            // Calculate horizontal and vertical spawn position
            fishSpawnDirection = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + seaRenderer.bounds.extents.y);

            // Calculate spawn rotation based on horizontal position
            fishSpawnRotation = fishSpawnDirection == seaPosition.x ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

            // Calculate spawn position
            fishSpawnPosition = new Vector3(fishSpawnDirection, spawnVertical, bait.transform.position.z);
        }
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
