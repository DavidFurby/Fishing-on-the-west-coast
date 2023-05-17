using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fishes;
    public int spawnDelay;
    [SerializeField] GameObject sea;
    new Renderer renderer;
    private Vector3 seaPosition;
    private float spawnHorizontal;
    private Quaternion spawnRotation;
    private Vector3 spawnPosition;
    private void Start()
    {
        //Get the size and center of the sea object
        seaPosition = sea.transform.position;
        renderer = sea.GetComponent<Renderer>();

        InvokeRepeating(nameof(SpawnFish), 2, spawnDelay);
    }

    private void SpawnFish()
    {
        //Choose random fish from the array
        int randomFishIndex = Random.Range(0, Fishes.Length);

        CalculateSpawnPosition(out spawnHorizontal, out spawnRotation, out spawnPosition);

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

    private void CalculateSpawnPosition(out float spawnHorizontal, out Quaternion spawnRotation, out Vector3 spawnPosition)
    {
        //Calculate horizontal and vertical spawn position
        spawnHorizontal = Random.value < 0.5 ? seaPosition.x : seaPosition.x + renderer.bounds.extents.x * 2;
        float spawnVertical = Random.Range(seaPosition.y, seaPosition.y + renderer.bounds.extents.y);

        //Calculate spawn rotation based on horizontal position
        spawnRotation = spawnHorizontal == seaPosition.x ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);

        //Calculate spawn position
        spawnPosition = new Vector3(spawnHorizontal, spawnVertical, 5);
    }
}
