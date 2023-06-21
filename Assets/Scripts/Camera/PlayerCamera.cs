using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    public CameraStatus cameraStatus;
    private ShopItem shopItem;
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
        if (cameraStatus == CameraStatus.Player)
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
            transform.position = new Vector3(shopItem.transform.position.x, shopItem.transform.position.y + 0.5f, cameraDistance);

            float targetDistance = shopItem.transform.position.z - 1.5f;
            cameraDistance += (targetDistance - cameraDistance) * 0.1f;
        }
    }

    public void SetShopItem(ShopItem shopItem)
    {
        this.shopItem = shopItem;
    }
    public void SetCameraStatus(CameraStatus cameraStatus)
    {
        this.cameraStatus = cameraStatus;
    }
}