using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Fishes;
    [SerializeField] int spawnDelay;
    new Renderer renderer;
    private Vector3 seaPosition;
    private float spawnHorizontal;
    private Quaternion spawnRotation;
    private Vector3 spawnPosition;
    [SerializeField] GameObject bait;
    [SerializeField] FishingControls fishingControlls;

    private void Start()
    {
        //Get the size and center of the sea object
        seaPosition = transform.position;
        renderer = GetComponent<Renderer>();
        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    private void SpawnFish()
    {
        //Choose random fish from the array
        int randomFishIndex = Random.Range(0, Fishes.Length);

        CalculateSpawnPosition();

        //Instatiate the fish 
        GameObject fish = Instantiate(Fishes[randomFishIndex], spawnPosition, spawnRotation);
        SetDirection(fish);
    }

    //Get the fishmovement component and set the direction based on horizontal positon
    private void SetDirection(GameObject fish)
    {
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        fishMovement.direction = spawnHorizontal == seaPosition.x ? Vector3.right : Vector3.left;
    }

    private void CalculateSpawnPosition()
    {
        if (fishingControlls.fishingStatus == FishingControls.GetFishingStatus.Fishing)
        {
            //Calculate horizontal and vertical spawn position
            spawnHorizontal = Random.value < 0.5 ? bait.transform.position.x - 5 : bait.transform.position.x + 5;
            float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + renderer.bounds.extents.y);

            //Calculate spawn rotation based on horizontal position
            spawnRotation = spawnHorizontal == seaPosition.x ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

            //Calculate spawn position
            spawnPosition = new Vector3(spawnHorizontal, spawnVertical, bait.transform.position.z);
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
}


