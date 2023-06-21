using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    public bool shopEnabled = false;
    public ShopItem focusedShopItem;
    private int focusedShopItemIndex = 0;
    [SerializeField] PlayerController playerController;
    private void Start()
    {
        focusedShopItem = shopItems[focusedShopItemIndex];
    }
    public void SetShopStatus(bool active)
    {
        shopEnabled = active;
    }
    public void FocusItem()
    {
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.ShoppingItem);
        playerCamera.SetShopItem(focusedShopItem);
    }

    public void OpenShop()
    {
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.Shopping);
        SetShopStatus(true);
        FocusItem();
    }
    public void CloseShop()
    {
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.StandBy);
        SetShopStatus(false);
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.Player);
    }
    public void ScrollBetweenItems(bool forward)
    {
        if (forward)
        {
            if (focusedShopItemIndex >= shopItems.Length - 1)
            {
                focusedShopItemIndex = 0;
            }
            else
            {
                focusedShopItemIndex++;
            }
        }
        else
        {
            if (focusedShopItemIndex <= 0)
            {
                focusedShopItemIndex = shopItems.Length - 1;
            }
            else
            {
                focusedShopItemIndex--;
            }
        }
        focusedShopItem = shopItems[focusedShopItemIndex];
        FocusItem();
    }
}
