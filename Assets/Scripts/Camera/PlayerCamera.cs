using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    public CameraStatus cameraStatus;
    private Vector3 shopItemPosition;
    private float cameraDistance;
    private float originalCameraDistance;
    public enum CameraStatus
    {
        Player,
        ShoppingItem
    }
    private void LateUpdate()
    {
        FollowPlayer();
        FollowShopItem();
    }

    private void Start()
    {
        originalCameraDistance = 0;
        cameraDistance = originalCameraDistance;
    }

    private void FollowPlayer()
    {
        if (cameraStatus == CameraStatus.Player && player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 4, cameraDistance);
            if (cameraDistance != originalCameraDistance)
            {
                cameraDistance = originalCameraDistance;
            }
        }
    }
    private void FollowShopItem()
    {
        if (cameraStatus == CameraStatus.ShoppingItem)
        {
            transform.position = new Vector3(shopItemPosition.x, shopItemPosition.y + 0.5f, cameraDistance);

            float targetDistance = shopItemPosition.z - 1.5f;
            cameraDistance += (targetDistance - cameraDistance) * 0.1f;
        }
    }

    public void SetShopItem(Vector3 shopItemPosition)
    {
        this.shopItemPosition = shopItemPosition;
    }
    public void SetCameraStatus(CameraStatus cameraStatus)
    {
        this.cameraStatus = cameraStatus;
    }
}