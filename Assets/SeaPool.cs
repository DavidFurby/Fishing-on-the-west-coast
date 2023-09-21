using UnityEngine;

public class SeaPool : MonoBehaviour
{
    private GameObject seaTilePrefab;
    [SerializeField] private int poolSize;
    private float seaWidth;
    private GameObject lastSpawnedTile;

    private void Start()
    {
        LoadSeaTilePrefab();
        SpawnSeaTile();
        SpawnSeaTile();
    }

    private void LoadSeaTilePrefab()
    {
        seaTilePrefab = Resources.Load<GameObject>("GameObjects/Environment/SeaTile");

        Vector3 newScale = new(100, 5, 50);
        seaTilePrefab.transform.localScale = newScale;
        CalculateSeaWidth(newScale.x);
    }

    private void SpawnSeaTile()
    {
        GameObject sea = ObjectPool.Instance.GetFromPool(seaTilePrefab, poolSize);
        sea.transform.SetParent(transform);
        Vector3 seaSpawnPosition = CalculateSpawnPosition();
        sea.transform.localPosition = seaSpawnPosition;
        sea.SetActive(true);
        lastSpawnedTile = sea;
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

    private void CalculateSeaWidth(float scaleFactor)
    {
        Renderer renderer = seaTilePrefab.GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            float originalWidth = renderer.bounds.size.x;
            seaWidth = originalWidth * scaleFactor;
            print(seaWidth);
        }
        else
        {
            Debug.LogError("Renderer component not found on Sea object or its children.");
        }
    }
}
