using System.Collections.Generic;
using System.Linq;
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
        InitializeSeaTileQueue();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefab = Resources.Load<GameObject>("GameObjects/Environment/Ocean");
        seaWidth = GetSeaWidth(seaTilePrefab);
    }

    private float GetSeaWidth(GameObject seaTile)
    {
        return seaTile.GetComponentsInChildren<Transform>().First(r => r.CompareTag("Sea")).GetComponentInChildren<Renderer>().transform.localScale.x;
    }

    private void InitializeSeaTileQueue()
    {
        seaTileQueue = new Queue<GameObject>();
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

        return lastSpawnedTile.transform.localPosition + new Vector3(seaWidth, 0, 0);
    }

    public void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
