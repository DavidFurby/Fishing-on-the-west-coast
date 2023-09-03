using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Item[] shopItems;
    [SerializeField] private ShopHandlers dialogHandlers;
    [SerializeField] private Item emptySpot;

    #endregion

    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
    [HideInInspector] public Item focusedShopItem;
    private int focusedShopItemIndex = 0;
    private readonly List<GameObject> shopItemPositions = new();

    private PlayerCamera playerCamera;
    private ExplorationController playerController;
    private DialogManager dialogManager;

    #endregion


    void OnEnable()
    {
        ExplorationController.NavigateShop += HandleShoppingInput;
    }
    #region MonoBehaviour Methods
    private void Start()
    {
        playerCamera = FindObjectOfType<PlayerCamera>();
        playerController = FindObjectOfType<ExplorationController>();
        dialogManager = FindObjectOfType<DialogManager>();
        Transform shopPositions = transform.Find("ShopPositions");
        if (shopPositions != null)
        {
            for (int positionIndex = 0; positionIndex < shopPositions.childCount; positionIndex++)
            {
                Transform childTransform = shopPositions.GetChild(positionIndex);
                if (childTransform != null)
                {
                    GameObject childPosition = childTransform.gameObject;
                    shopItemPositions.Add(childPosition);
                }
            }
        }
        SpawnItems();
    }

    void OnDisable()
    {
        ExplorationController.NavigateShop -= HandleShoppingInput;
    }
    #endregion

    #region Public Methods

    public void FocusItem()
    {
        playerCamera.SetState(new CameraState.ShopItem(playerCamera));
        playerCamera.SetShopItem(shopItemPositions[focusedShopItemIndex].transform.position);
    }

    /// <summary>
    /// Opens the shop.
    /// </summary>
    public IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(0.5f);
        playerController.SetState(new Shopping(playerController));
        FocusItem();
        UpdateDialog();
    }

    /// <summary>
    /// Closes the shop.
    /// </summary>
    public void CloseShop()
    {
        dialogManager.EndDialog();
        playerController.SetState(new ExplorationIdle(playerController));
        playerCamera.SetState(new CameraState.Player(playerCamera));
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
    public void HandleShoppingInput()
    {
        if (pauseShoppingControls) return;

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

    public void BuyItem()
    {
        if (focusedShopItem != null)
        {
            MainManager.Instance.Inventory.AddItem(focusedShopItem);
            ReplaceItemOnSelf();
            FocusItem();
        }
    }

    private void ReplaceItemOnSelf()
    {
        GameObject replacement = Instantiate(emptySpot.model, shopItemPositions[focusedShopItemIndex].transform.position, focusedShopItem.model.transform.rotation);
        replacement.transform.parent = transform;
        Destroy(focusedShopItem.model);
        shopItems[focusedShopItemIndex] = emptySpot;
    }


    public void UpdateDialog()
    {
        dialogHandlers.SetShopItemHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
    }
    private void SpawnItems()
    {
        for (int i = 0; i < shopItemPositions.Count; i++)
        {
            if (i < shopItems.Length && shopItems[i] != null && !MainManager.Instance.Inventory.HasItem(shopItems[i]))
            {
                SpawnShopItem(i);
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

        shopItems[index] = emptySpot;
        GameObject newEmptySpot = Instantiate(shopItems[index].model, shopItemPositions[index].transform.position, Quaternion.identity);
        newEmptySpot.transform.SetParent(shopItemPositions[index].transform, false);
        newEmptySpot.transform.position = shopItemPositions[index].transform.position;
    }
    private void SpawnShopItem(int index)
    {
        GameObject newObject = Instantiate(shopItems[index].model, shopItemPositions[index].transform.position, Quaternion.identity);
        newObject.transform.SetParent(shopItemPositions[index].transform, false);
        newObject.transform.position = shopItemPositions[index].transform.position;

    }
}
#endregion
