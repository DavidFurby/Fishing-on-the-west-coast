
using UnityEngine;

[RequireComponent(typeof(SeaTileManager))]
[RequireComponent(typeof(SeaColliderManager))]
public class SeaSpawner : MonoBehaviour
{
    [SerializeField] private SeaTileManager seaTileManager;
    [SerializeField] private SeaColliderManager seaColliderManager;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject sea = seaTileManager.SpawnSeaTile();
            seaTileManager.seaTileQueue.Enqueue(sea);
        }
    }

    private void Update()
    {
        SpawnBasedOnCamera();
        seaColliderManager.UpdateColliders(seaTileManager.seaTileQueue);
    }


    private void SpawnBasedOnCamera()
    {
        float cameraX = CameraController.Instance.transform.position.x;
        float middleTileX = seaTileManager.seaTileQueue.Peek().transform.position.x;
        if (cameraX > middleTileX + seaTileManager.seaWidth / 2)
        {
            GameObject removedTile = seaTileManager.seaTileQueue.Dequeue();
            seaTileManager.RemoveSeaTile(removedTile);
            GameObject newTile = seaTileManager.SpawnSeaTile();
            seaTileManager.seaTileQueue.Enqueue(newTile);
        }
        else if (cameraX < middleTileX - seaTileManager.seaWidth / 2)
        {
            GameObject removedTile = seaTileManager.seaTileQueue.Dequeue();
            seaTileManager.RemoveSeaTile(removedTile);

            GameObject newTile = seaTileManager.SpawnSeaTile();
            newTile.transform.localPosition = seaTileManager.seaTileQueue.Peek().transform.localPosition - new Vector3(seaTileManager.seaWidth, 0, 0);
            seaTileManager.seaTileQueue.Enqueue(newTile);
        }
    }
}


