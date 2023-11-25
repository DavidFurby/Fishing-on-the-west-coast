using UnityEngine;

public class ExplorationCamera : MonoBehaviour
{
    PlayerManager player;
    private Vector3 shopItemPosition;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerManager>();
    }

    internal void FollowPlayer()
    {
        CameraManager.Instance.movement.SetCameraToTarget(player.transform.position, 2);
        CameraManager.Instance.movement.MoveCameraToTarget(player.transform.position, 1, 6, true);
    }
    internal void FollowShopItem()
    {
        transform.position = new Vector3(shopItemPosition.x, shopItemPosition.y + 0.5f, CameraManager.Instance.cameraDistance);

        float targetDistance = shopItemPosition.z - 1.5f;
        CameraManager.Instance.cameraDistance += (targetDistance - CameraManager.Instance.cameraDistance) * 0.1f;

    }

    internal void UpdateCameraDuringDialog()
    {
        Vector3 position = SetBetweenPositions(player.transform.position, player.interactive.transform.position);
        position.y = transform.position.y;
        CameraManager.Instance.movement.SetCameraToTarget(position);
        CameraManager.Instance.movement.MoveCameraToTarget(position, 0.05f, 4);
        CameraManager.Instance.movement.RotateTowardsTarget(position); 
    }
    internal Vector3 SetBetweenPositions(Vector3 first, Vector3 second)
    {
        return (first + second) / 2;
    }
    public void SetShopItem(Vector3 shopItemPosition)
    {
        this.shopItemPosition = shopItemPosition;
    }
}