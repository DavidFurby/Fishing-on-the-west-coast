using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SeaTileManager))]
[RequireComponent(typeof(SeaColliderController))]
public class SeaSpawner : MonoBehaviour
{
    [SerializeField] private SeaTileManager seaTileManager;
    [SerializeField] private SeaColliderController seaColliderManager;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject sea = seaTileManager.SpawnSeaTile();
            seaTileManager.seaTileList.Add(sea);
        }
    }

    private void Update()
    {
        SpawnBasedOnCamera();
        seaColliderManager.UpdateSeaColliders(seaTileManager.seaTileList);
    }

    private void SpawnBasedOnCamera()
    {
        float cameraX = CameraController.Instance.transform.position.x;
        float middleTileX = seaTileManager.seaTileList[seaTileManager.seaTileList.Count / 2].transform.position.x;
        if (cameraX > middleTileX + seaTileManager.seaWidth / 2)
        {
            GameObject removedTile = seaTileManager.seaTileList[0];
            seaTileManager.RemoveSeaTile(removedTile);
            seaTileManager.seaTileList.RemoveAt(0);
            GameObject newTile = seaTileManager.SpawnSeaTile();
            seaTileManager.seaTileList.Add(newTile);
        }
        else if (cameraX < middleTileX - seaTileManager.seaWidth / 2)
        {
            GameObject removedTile = seaTileManager.seaTileList[seaTileManager.seaTileList.Count - 1];
            seaTileManager.RemoveSeaTile(removedTile);
            seaTileManager.seaTileList.RemoveAt(seaTileManager.seaTileList.Count - 1);
            GameObject newTile = seaTileManager.SpawnSeaTile();
            newTile.transform.localPosition = seaTileManager.seaTileList[0].transform.localPosition - new Vector3(seaTileManager.seaWidth, 0, 0);
            seaTileManager.seaTileList.Insert(0, newTile);
        }
    }
}
