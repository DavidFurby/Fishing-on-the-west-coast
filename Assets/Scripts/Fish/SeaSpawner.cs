using System.Collections;
using UnityEngine;

public class SeaSpawner : MonoBehaviour
{

    new Renderer renderer;
    private Vector3 seaPosition;
    private float fishSpawnDirection;
    private Quaternion fishSpawnRotation;
    private Vector3 fishSpawnPosition;
    [SerializeField] GameObject[] Fishes;
    [SerializeField] int spawnDelay;
    [SerializeField] GameObject bait;
    [SerializeField] FishingController fishingControlls;

    private void Start()
    {
        //Get the size and center of the sea object
        seaPosition = transform.position;
        renderer = GetComponent<Renderer>();
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }
    private void Update()
    {
        if (fishingControlls.fishingStatus == FishingController.FishingStatus.StandBy)
        {
            RemoveAllFishes();
        }
    }

    private void SpawnFish()
    {
        if (fishingControlls.fishingStatus != FishingController.FishingStatus.StandBy)
        {
            //Choose random fish from the array
            int randomFishIndex = Random.Range(0, Fishes.Length);

            CalculateSpawnPosition();

            //Instatiate the fish 
            GameObject fish = Instantiate(Fishes[randomFishIndex], fishSpawnPosition, fishSpawnRotation);
            SetDirection(fish);
        }
    }

    //Get the fishmovement component and set the direction based on horizontal positon
    private void SetDirection(GameObject fish)
    {
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        fishMovement.direction = fishSpawnDirection == seaPosition.x ? Vector3.right : Vector3.left;
    }

    private void CalculateSpawnPosition()
    {
        if (fishingControlls.fishingStatus != FishingController.FishingStatus.StandBy)
        {
            //Calculate horizontal and vertical spawn position
            fishSpawnDirection = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + renderer.bounds.extents.y);

            //Calculate spawn rotation based on horizontal position
            fishSpawnRotation = fishSpawnDirection == seaPosition.x ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

            //Calculate spawn position
            fishSpawnPosition = new Vector3(fishSpawnDirection, spawnVertical, bait.transform.position.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            if (other.transform.position.y > seaPosition.y + renderer.bounds.extents.y)
            {
                other.attachedRigidbody.AddForce(Vector3.down * 2, ForceMode.Impulse);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
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


