using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DialogManager dialogManager;
    #endregion

    #region Private Fields
    public ShopItem focusedShopItem;
    private int focusedShopItemIndex = 0;
    [SerializeField] ShopItem emptySpot;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        focusedShopItem = shopItems[focusedShopItemIndex];
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Focuses on an item in the shop.
    /// </summary>
    public void FocusItem()
    {
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.ShoppingItem);
        playerCamera.SetShopItem(focusedShopItem);
    }

    /// <summary>
    /// Opens the shop.
    /// </summary>
    public void OpenShop()
    {
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.Shopping);
        FocusItem();
        dialogManager.SetShopItemNameHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
    }

    /// <summary>
    /// Closes the shop.
    /// </summary>
    public void CloseShop()
    {
        dialogManager.EndDialog();
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.StandBy);
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.Player);
        focusedShopItemIndex = 0;
    }

    /// <summary>
    /// Scrolls between items in the shop.
    /// </summary>
    /// <param name="forward">If set to <c>true</c> forward.</param>
    public void ScrollBetweenItems(bool forward)
    {
        focusedShopItemIndex = (focusedShopItemIndex + (forward ? 1 : -1) + shopItems.Length) % shopItems.Length;
        focusedShopItem = shopItems[focusedShopItemIndex];
        dialogManager.EndDialog();
        dialogManager.SetShopItemNameHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
        FocusItem();
    }
    public void BuyItem()
    {
        if (focusedShopItem != null)
        {
            GameObject replacement = Instantiate(emptySpot.gameObject, focusedShopItem.transform.position, focusedShopItem.transform.rotation);
            replacement.transform.parent = focusedShopItem.transform.parent;
            shopItems[focusedShopItemIndex] = replacement.GetComponent<ShopItem>();
            var prevItem = focusedShopItem;
            focusedShopItem = replacement.GetComponent<ShopItem>();
            FocusItem();
            Destroy(prevItem.gameObject);
        }
    }
}
#endregion
