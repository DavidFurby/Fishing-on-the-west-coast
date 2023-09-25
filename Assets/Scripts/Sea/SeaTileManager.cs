using System.Collections.Generic;
using UnityEngine;

public class SeaTileManager : MonoBehaviour
{
    internal GameObject seaTilePrefab;
    public int poolSize;
    internal Vector3 seaSize;
    internal GameObject lastSpawnedTile;

    internal List<GameObject> seaTileList;

    private void Awake()
    {
        LoadSeaTilePrefab();
        InitializeSeaTileQueue();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefab = Resources.Load<GameObject>("GameObjects/Environment/Ocean");
        seaSize = seaTilePrefab.GetComponent<BoxCollider>().size;
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
        return ObjectPool.Instance.GetFromPool(seaTilePrefab, poolSize);
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
            return Vector3.zero;
        return lastSpawnedTile.transform.localPosition + new Vector3(seaSize.x, 0, 0);
    }

    public void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
