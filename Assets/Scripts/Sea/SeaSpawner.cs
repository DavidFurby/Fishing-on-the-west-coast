using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeaSpawner : MonoBehaviour
{
    private GameObject seaTilePrefab;
    [SerializeField] private int poolSize;
    private float seaWidth;
    private GameObject lastSpawnedTile;
    private Queue<GameObject> seaTileQueue;
    private BoxCollider seaGroupCollider;

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
        SetBoxCollider();
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

    private void SetBoxCollider()
    {
        float xMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float zMin = Mathf.Infinity;
        float zMax = -Mathf.Infinity;
        foreach (GameObject seaTile in seaTileQueue)
        {
            Bounds bounds = seaTile.GetComponentInChildren<Renderer>().bounds;
            xMin = Mathf.Min(xMin, bounds.min.x);
            xMax = Mathf.Max(xMax, bounds.max.x);
            zMin = Mathf.Min(zMin, bounds.min.z);
            zMax = Mathf.Max(zMax, bounds.max.z);
        }
        Vector3 colliderCenter = new((xMin + xMax) / 2, transform.position.y, (zMin + zMax) /2);
        Vector3 colliderSize = new(xMax - xMin, transform.position.y, zMax- zMin);
        if(seaGroupCollider == null) {
            GameObject collider = new("SeaGroupCollider");
            collider.transform.SetParent(transform);
            seaGroupCollider = collider.AddComponent<BoxCollider>();
            seaGroupCollider.AddComponent<WaterCollision>();
        }
        seaGroupCollider.center = colliderCenter;
        seaGroupCollider.size = colliderSize; 
    }
}
