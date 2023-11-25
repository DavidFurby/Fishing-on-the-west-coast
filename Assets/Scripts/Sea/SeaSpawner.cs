using UnityEngine;

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
        seaColliderManager.SetBoxCollider(seaTileManager.seaTileList, seaTileManager.seaSize);
    }

    private void Update()
    {
        SpawnBasedOnCamera();
    }

    private void SpawnBasedOnCamera()
    {
        float cameraX = CameraManager.Instance.transform.position.x;
        float middleTileX = seaTileManager.seaTileList[seaTileManager.seaTileList.Count / 2].transform.position.x;
        if (cameraX > middleTileX + seaTileManager.seaSize.x / 2)
        {
            GameObject removedTile = seaTileManager.seaTileList[0];
            seaTileManager.RemoveSeaTile(removedTile);
            seaTileManager.seaTileList.RemoveAt(0);
            GameObject newTile = seaTileManager.SpawnSeaTile();
            newTile.transform.localPosition = seaTileManager.seaTileList[^1].transform.localPosition + new Vector3(seaTileManager.seaSize.x, 0, 0);
            seaTileManager.seaTileList.Add(newTile);
            seaColliderManager.SetBoxCollider(seaTileManager.seaTileList, seaTileManager.seaSize);

        }
        else if (cameraX < middleTileX - seaTileManager.seaSize.x / 2)
        {
            GameObject removedTile = seaTileManager.seaTileList[^1];
            seaTileManager.RemoveSeaTile(removedTile);
            seaTileManager.seaTileList.RemoveAt(seaTileManager.seaTileList.Count - 1);
            GameObject newTile = seaTileManager.SpawnSeaTile();
            newTile.transform.localPosition = seaTileManager.seaTileList[0].transform.localPosition - new Vector3(seaTileManager.seaSize.x, 0, 0);
            seaTileManager.seaTileList.Insert(0, newTile);
            seaColliderManager.SetBoxCollider(seaTileManager.seaTileList, seaTileManager.seaSize);

        }
    }
}
