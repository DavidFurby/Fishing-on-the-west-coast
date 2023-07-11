using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExplorationController;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private ItemDisplay[] shopItems;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private ExplorationController playerController;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private ShopHandlers dialogHandlers;
    [SerializeField] private ItemDisplay emptySpot;
    private readonly List<Vector3> shopItemPositions = new();

    #endregion

    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
    [HideInInspector] public ItemDisplay focusedShopItem;
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
            MainManager.Instance.game.AddItem(focusedShopItem.item);
            ReplaceItemOnSelf();
            FocusItem();
        }
    }

    private void ReplaceItemOnSelf()
    {
        GameObject replacement = Instantiate(emptySpot.gameObject, shopItemPositions[focusedShopItemIndex], focusedShopItem.transform.rotation);
        replacement.transform.parent = transform;
        DestroyImmediate(focusedShopItem.gameObject);
        shopItems[focusedShopItemIndex] = replacement.GetComponent<ItemDisplay>();
    }


    public void UpdateDialog()
    {
        dialogHandlers.SetShopItemHandler(focusedShopItem.item);
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
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            if (i < shopItems.Length && shopItems[i] != null)
            {
                if (MainManager.Instance.game.HasItem(shopItems[i].item))
                {
                    SpawnEmptySpot(i);
                }
                else
                {
                    SpawnShopItem(i);
                }
            }
            else
            {
                SpawnEmptySpot(i);
            }
        }

        focusedShopItem = shopItems[focusedShopItemIndex];
    }

    private void SpawnEmptySpot(int index)
    {
        GameObject gameObject = Instantiate(emptySpot.gameObject, shopItemPositions[index], Quaternion.identity);
        gameObject.transform.parent = transform;
    }
    private void SpawnShopItem(int index)
    {
        GameObject newObject = Instantiate(shopItems[index].gameObject, shopItemPositions[index], Quaternion.identity);
        newObject.transform.parent = transform;
        shopItems[index] = newObject.GetComponent<ItemDisplay>();
    }
}
#endregion
