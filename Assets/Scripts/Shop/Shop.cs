using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                if (!IsFishingRodInInventory(focusedShopItem))
                {
                    FishingRod fishingRod = focusedShopItem.gameObject.GetComponent<FishingRod>();
                    fishingRod.AddFishingRodToInstance();
                }
                GameObject replacement = Instantiate(emptySpot.gameObject, shopItemPositions[focusedShopItemIndex], focusedShopItem.transform.rotation);
                replacement.transform.parent = focusedShopItem.transform.parent;
                shopItems[focusedShopItemIndex] = replacement.GetComponent<ShopItem>();
                focusedShopItem = replacement.GetComponent<ShopItem>();
                FocusItem();
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
        // Loop through all shop item positions
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            // Check if the current shop item is a fishing rod and is already in the player's inventory
            if (IsFishingRodInInventory(shopItems[i]))
            {
                // If the fishing rod is already in the player's inventory, spawn an empty spot instead
                SpawnEmptySpot(i);
            }
            else
            {
                // Otherwise, spawn the shop item
                SpawnShopItem(i);
            }
        }

        // Set the initial focused shop item
        focusedShopItem = shopItems[focusedShopItemIndex];
    }

    private bool IsFishingRodInInventory(ShopItem shopItem)
    {
        if (shopItem.gameObject.TryGetComponent<FishingRod>(out var fishingRod))
        {
            return MainManager.Instance.game.FishingRods.Any(f => f.Id == fishingRod.Id);
        }
        else
        {
            return false;
        }

    }

    private void SpawnEmptySpot(int index)
    {
        shopItems[index] = emptySpot;
        GameObject gameObject = Instantiate(shopItems[index].gameObject, shopItemPositions[index], Quaternion.identity);
        gameObject.transform.parent = transform;
    }

    private void SpawnShopItem(int index)
    {
        GameObject newObject = Instantiate(shopItems[index].gameObject, shopItemPositions[index], Quaternion.identity);
        newObject.transform.parent = transform;
    }
}
#endregion