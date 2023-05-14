using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fishes;
    private int randomFish;
    private Vector3 spawnPosition;
    private int spawnVertical;
    private Quaternion spawnRotation;
    public Camera mainCamera;
    public int spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnFish", 2, spawnDelay);
    }

    private void SpawnFish()
    {
        randomFish = Random.Range(0, Fishes.Length);
        spawnVertical = Random.Range(0, Screen.height / 2);
        float spawnHorizontal = Random.value < 0.5 ? -20 : Screen.width + 20;
        spawnRotation = spawnHorizontal == -20 ? Quaternion.Euler(0, 180, 180) : Quaternion.Euler(0, 180, 0);
        spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(spawnHorizontal, (Screen.height / 2) - spawnVertical, 2));
        Instantiate(Fishes[randomFish], spawnPosition, spawnRotation);
    }
}
