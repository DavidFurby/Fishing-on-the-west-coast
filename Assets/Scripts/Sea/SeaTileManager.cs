using System.Collections.Generic;
using UnityEngine;

public class SeaTileManager : MonoBehaviour
{
    [SerializeField] DistanceRecord distanceRecord;
    internal GameObject[] seaTilePrefabs;
    internal GameObject currentSeaTilePrefab;
    public int poolSize;
    internal Vector3 seaSize;
    internal GameObject lastSpawnedTile;
    internal List<GameObject> seaTileList;
    private int currentTileIndex = -1;
    private int lastDistancePos;

    private void Awake()
    {
        LoadSeaTilePrefab();
        InitializeSeaTileQueue();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefabs = Resources.LoadAll<GameObject>("GameObjects/Environment/Oceans");
        currentSeaTilePrefab = seaTilePrefabs[0];
        seaSize = currentSeaTilePrefab.GetComponent<BoxCollider>().size;
    }

    private void InitializeSeaTileQueue()
    {
        seaTileList = new List<GameObject>();
    }

    public GameObject SpawnSeaTile()
    {
        GameObject sea = GetSeaFromPool();
        SetSeaPosition(sea);
        sea.SetActive(true);
        lastSpawnedTile = sea;
        return sea;
    }

    private GameObject GetSeaFromPool()
    {
        if (distanceRecord.distance / 100 >= lastDistancePos)
        {
            currentTileIndex++;

            if (currentTileIndex >= seaTilePrefabs.Length)
            {
                currentTileIndex = seaTilePrefabs.Length - 1;
            }
        }
        else if (distanceRecord.distance / 100 <= lastDistancePos)
        {
            if (currentTileIndex <= 0)
            {
                currentTileIndex--;

            }
        }

        currentSeaTilePrefab = seaTilePrefabs[currentTileIndex];
        lastDistancePos = (int)distanceRecord.distance / 100;

        return ObjectPool.Instance.GetFromPool(currentSeaTilePrefab, poolSize);
    }

    private void SetSeaPosition(GameObject sea)
    {
        sea.transform.SetParent(transform);
        Vector3 seaSpawnPosition = CalculateSpawnPosition();
        sea.transform.localPosition = seaSpawnPosition;
    }

    private Vector3 CalculateSpawnPosition()
    {
        if (lastSpawnedTile == null)
            return new Vector3(0, 0, 390);
        return lastSpawnedTile.transform.localPosition + new Vector3(seaSize.x, 0, 0);
    }

    public void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
