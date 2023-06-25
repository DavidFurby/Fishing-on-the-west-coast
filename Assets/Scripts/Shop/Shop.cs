using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] ShopItem emptySpot;
    private readonly List<Vector3> shopItemPositions = new();

    #endregion

    #region Private Fields
    public ShopItem focusedShopItem;
    private int focusedShopItemIndex = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        for (int positionIndex = 1; positionIndex <= 6; positionIndex++)
        {
            string positionName = "Position " + positionIndex;
            Transform childTransform = transform.Find(positionName);
            if (childTransform != null)
            {
                Vector3 childPosition = childTransform.position;
                shopItemPositions.Add(childPosition);
            }
        }
        SpawnItems();
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Focuses on an item in the shop.
    /// </summary>
    public void FocusItem()
    {
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.ShoppingItem);
        playerCamera.SetShopItem(shopItemPositions[focusedShopItemIndex]);
    }

    /// <summary>
    /// Opens the shop.
    /// </summary>
    public IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(0.5f);
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.Shopping);
        FocusItem();
        UpdateDialog();
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
        FocusItem();
        UpdateDialog();
    }
    public void BuyItem()
    {
        if (focusedShopItem != null)
        {
            if (MainManager.Instance.game.Fishes >= focusedShopItem.Price)
            {
                GameObject replacement = Instantiate(emptySpot.gameObject, focusedShopItem.transform.position, focusedShopItem.transform.rotation);
                replacement.transform.parent = focusedShopItem.transform.parent;
                shopItems[focusedShopItemIndex] = replacement.GetComponent<ShopItem>();
                var prevItem = focusedShopItem;
                focusedShopItem = replacement.GetComponent<ShopItem>();
                FocusItem();
                Destroy(prevItem.gameObject);
            }
            else
            {
                dialogManager.StartDialog("InsufficentPayment");
            }
        }
    }
    public void UpdateDialog()
    {
        dialogManager.EndDialog();
        dialogManager.SetShopItemNameHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
    }
    private void SpawnItems()
    {
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            GameObject newObject = Instantiate(shopItems[i].gameObject, shopItemPositions[i], Quaternion.identity);
            newObject.transform.parent = transform;
        }
        focusedShopItem = shopItems[focusedShopItemIndex];
    }
}
#endregion
