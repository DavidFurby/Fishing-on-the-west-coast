using UnityEngine;

public class PlayerCamera : CameraStateMachine
{
    [SerializeField] GameObject player;
    private Vector3 shopItemPosition;
    private float cameraDistance;
    private float originalCameraDistance;

    private void Start()
    {
        SetState(new CameraState.Player(this));
        originalCameraDistance = 0;
        cameraDistance = originalCameraDistance;
    }

    internal void FollowPlayer()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, cameraDistance - 2);
        if (cameraDistance != originalCameraDistance)
        {
            cameraDistance = originalCameraDistance;
        }

    }
    internal void FollowShopItem()
    {
        transform.position = new Vector3(shopItemPosition.x, shopItemPosition.y + 0.5f, cameraDistance);

        float targetDistance = shopItemPosition.z - 1.5f;
        cameraDistance += (targetDistance - cameraDistance) * 0.1f;

    }

    public void SetShopItem(Vector3 shopItemPosition)
    {
        this.shopItemPosition = shopItemPosition;
    }
}