using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] Fishes;
    private int randomFish;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        randomFish = Random.Range(0, Fishes.Length);
        spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 2));
        Instantiate(Fishes[randomFish], spawnPosition, spawnRotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
