using System.Collections.Generic;
using UnityEngine;

public class SeaPool : MonoBehaviour
{
    private GameObject seaTilePrefab;
    [SerializeField] private int poolSize;
    private float seaWidth;
    private GameObject lastSpawnedTile;
    private List<GameObject> seaTileArray;

    private void Start()
    {
        LoadSeaTilePrefab();
        seaTileArray = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject sea = SpawnSeaTile();
            seaTileArray.Add(sea);
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
        float middleTileX = seaTileArray[1].transform.position.x;
        if (cameraX > middleTileX + seaWidth / 2)
        {
          GameObject removedTile =  seaTileArray[0];
          RemoveSeaTile(removedTile);
          seaTileArray.RemoveAt(0);
          GameObject newTile = SpawnSeaTile();
          seaTileArray.Add(newTile);
        }
        else if (cameraX < middleTileX - seaWidth / 2)
        {
            GameObject removedTile = seaTileArray[2];
            RemoveSeaTile(removedTile);
            seaTileArray.RemoveAt(2);

            GameObject newTile = SpawnSeaTile();
            newTile.transform.localPosition = seaTileArray[0].transform.localPosition - new Vector3(seaWidth, 0, 0);
            seaTileArray.Insert(0, newTile);
        }
    }
    private void RemoveSeaTile(GameObject seaTile)
    {
        ObjectPool.Instance.ReturnToPool(seaTile);
    }
}
