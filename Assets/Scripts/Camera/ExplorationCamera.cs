using UnityEngine;

public class ExplorationCamera : MonoBehaviour
{
    ExplorationController player;
    private Vector3 shopItemPosition;

    private void Start()
    {
        player = FindAnyObjectByType<ExplorationController>();
    }

    internal void FollowPlayer()
    {
        CameraController.Instance.SetCameraToTarget(player.transform, 2);
        CameraController.Instance.MoveCameraToTarget(player.transform, 1, 6);
    }
    internal void FollowShopItem()
    {
        transform.position = new Vector3(shopItemPosition.x, shopItemPosition.y + 0.5f, CameraController.Instance.cameraDistance);

        float targetDistance = shopItemPosition.z - 1.5f;
        CameraController.Instance.cameraDistance += (targetDistance - CameraController.Instance.cameraDistance) * 0.1f;

    }


    public void SetShopItem(Vector3 shopItemPosition)
    {
        this.shopItemPosition = shopItemPosition;
    }
}