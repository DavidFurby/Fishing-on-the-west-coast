using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fishes;
    public Camera mainCamera;
    public int spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnFish", 2, spawnDelay);
    }

    private void SpawnFish()
    {
        int randomFishIndex = Random.Range(0, Fishes.Length);
        float spawnVertical = Random.Range(0, Screen.height / 2);
        float spawnHorizontal = Random.value < 0.5 ? -20 : Screen.width + 20;
        Quaternion spawnRotation = spawnHorizontal == -20 ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        Vector3 spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(spawnHorizontal, (Screen.height / 2) - spawnVertical, 4));
        GameObject fish = Instantiate(Fishes[randomFishIndex], spawnPosition, spawnRotation);
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        fishMovement.direction = spawnHorizontal == -20 ? Vector3.right : Vector3.left;
    }
}
