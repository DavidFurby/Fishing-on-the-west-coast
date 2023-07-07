using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExplorationController;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Item[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private ExplorationController playerController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private ShopHandlers dialogHandlers;
    [SerializeField] private Item emptySpot;
    private readonly List<Vector3> shopItemPositions = new();

    #endregion

    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
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
            MainManager.Instance.game.AddItem(focusedShopItem);
            ReplaceItemOnSelf();
            FocusItem();
        }
    }

    private void ReplaceItemOnSelf()
    {
        GameObject replacement = Instantiate(emptySpot.gameObject, shopItemPositions[focusedShopItemIndex], focusedShopItem.transform.rotation);
        replacement.transform.parent = focusedShopItem.transform.parent;
        Destroy(focusedShopItem.gameObject);
        shopItems[focusedShopItemIndex] = replacement.GetComponent<Item>();
        focusedShopItem = shopItems[focusedShopItemIndex];
    }

    public void UpdateDialog()
    {
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
                if (MainManager.Instance.game.HasItem(shopItems[i].Id, shopItems[i].itemTag))
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
        shopItems[index] = newObject.GetComponent<Item>();
    }
}
#endregion
