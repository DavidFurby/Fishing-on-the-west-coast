using System.Collections.Generic;
using UnityEngine;

public class SeaTileManager : MonoBehaviour
{
    internal GameObject seaTilePrefab;
    public int poolSize;
    internal float seaWidth;
    internal GameObject lastSpawnedTile;

    internal Queue<GameObject> seaTileQueue;

    private void Awake()
    {
        LoadSeaTilePrefab();
        seaTileQueue = new Queue<GameObject>();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefab = Resources.Load<GameObject>("GameObjects/Environment/SeaTile");
        seaWidth = seaTilePrefab.transform.localScale.x;
    }

    public GameObject SpawnSeaTile()
    {
        print(seaTilePrefab);
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

    public void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
