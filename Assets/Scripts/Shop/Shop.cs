using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ExplorationController;
using static Game;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Item[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private ExplorationController playerController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private ShopHandlers dialogHandlers;
    [SerializeField] Item emptySpot;
    private readonly List<Vector3> shopItemPositions = new();
    [HideInInspector] public bool pauseShoppingControls;

    #endregion

    #region Private Fields
    [HideInInspector] public Item focusedShopItem;
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
    private void Update()
    {
        HandleShoppingInput();
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Focuses on an item in the shop.
    /// </summary>
    /// 

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
        playerController.SetPlayerStatus(PlayerStatus.Shopping);
        playerController.gameObject.SetActive(false);
        FocusItem();
        UpdateDialog();
    }

    /// <summary>
    /// Closes the shop.
    /// </summary>
    public void CloseShop()
    {
        dialogManager.EndDialog();
        playerController.SetPlayerStatus(PlayerStatus.StandBy);
        playerCamera.SetCameraStatus(PlayerCamera.CameraStatus.Player);
        playerController.gameObject.SetActive(true);
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
            if (IsFishingRod(focusedShopItem))
            {
                FishingRod fishingRod = focusedShopItem.gameObject.GetComponent<FishingRod>();
                fishingRod.AddFishingRodToInstance();
            }
            ReplaceItemOnSelf();
            FocusItem();
        }
    }

    private void ReplaceItemOnSelf()
    {
        GameObject replacement = Instantiate(gameObject, shopItemPositions[focusedShopItemIndex], focusedShopItem.transform.rotation);
        replacement.transform.parent = transform.parent;
        shopItems[focusedShopItemIndex] = replacement.GetComponent<Item>();
        focusedShopItem = replacement.GetComponent<Item>();
    }

    public void UpdateDialog()
    {
        dialogManager.EndDialog();
        dialogHandlers.SetShopItemHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
    }
    private void HandleShoppingInput()
    {
        if (playerController.playerStatus == PlayerStatus.Shopping && !pauseShoppingControls)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ScrollBetweenItems(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ScrollBetweenItems(true);

            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                CloseShop();
            }
        }
    }
    private void SpawnItems()
    {
        // Loop through all shop item positions
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            if (i < shopItems.Length && shopItems[i] != null)
            {
                // Check if the current shop item is a fishing rod and is already in the player's inventory
                if (IsFishingRod(shopItems[i]) && shopItems[i].gameObject.TryGetComponent<FishingRod>(out FishingRod fishingRod) && !IsFishingRodInInventory(fishingRod))
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
            else
            {
                SpawnEmptySpot(i);
            }
        }

        // Set the initial focused shop item
        focusedShopItem = shopItems[focusedShopItemIndex];
    }

    private bool IsFishingRod(Item shopItem)
    {
        if (shopItem.gameObject.TryGetComponent<FishingRod>(out _))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private static bool IsFishingRodInInventory(FishingRod fishingRod)
    {
        return MainManager.Instance.game.FoundFishingRods.Any(f => f.Id == fishingRod.Id);
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
