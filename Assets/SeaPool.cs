using System.Collections.Generic;
using UnityEngine;

public class SeaSpawner : MonoBehaviour
{
    private GameObject seaTilePrefab;
    [SerializeField] private int poolSize;
    private float seaWidth;
    private GameObject lastSpawnedTile;
    private Queue<GameObject> seaTileQueue; // Changed from List to Queue for better performance

    private void Start()
    {
        LoadSeaTilePrefab();
        seaTileQueue = new Queue<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject sea = SpawnSeaTile();
            seaTileQueue.Enqueue(sea);
        }
    }

    private void Update()
    {
        SpawnBasedOnCamera();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefab = Resources.Load<GameObject>("GameObjects/Environment/SeaTile");
        seaWidth = seaTilePrefab.transform.localScale.x;
    }

    private GameObject SpawnSeaTile()
    {
        GameObject sea = ObjectPool.Instance.GetFromPool(seaTilePrefab, poolSize);
        sea.transform.SetParent(transform);
        Vector3 seaSpawnPosition = CalculateSpawnPosition();
        sea.transform.localPosition = seaSpawnPosition;
        sea.SetActive(true);
        lastSpawnedTile = sea;
        return sea;
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        if (lastSpawnedTile != null)
        {
            spawnPosition = lastSpawnedTile.transform.localPosition + new Vector3(seaWidth, 0, 0);
        }
        return spawnPosition;
    }

    private void SpawnBasedOnCamera()
    {
        float cameraX = CameraController.Instance.transform.position.x;
        float middleTileX = seaTileQueue.Peek().transform.position.x;
        if (cameraX > middleTileX + seaWidth / 2)
        {
            GameObject removedTile = seaTileQueue.Dequeue();
            RemoveSeaTile(removedTile);
            GameObject newTile = SpawnSeaTile();
            seaTileQueue.Enqueue(newTile);
        }
        else if (cameraX < middleTileX - seaWidth / 2)
        {
            GameObject removedTile = seaTileQueue.Dequeue();
            RemoveSeaTile(removedTile);

            GameObject newTile = SpawnSeaTile();
            newTile.transform.localPosition = seaTileQueue.Peek().transform.localPosition - new Vector3(seaWidth, 0, 0);
            seaTileQueue.Enqueue(newTile);
        }
    }

    private void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
