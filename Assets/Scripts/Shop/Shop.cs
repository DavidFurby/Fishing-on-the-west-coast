using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShopHandlers))]
public class Shop : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items";

    #region Serialized Fields
    private Item[] shopItems;

    #endregion

    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
    private Item focusedShopItem;
    private int focusedShopItemIndex = 0;
    private readonly List<GameObject> shopItemPositions = new();
    private Item emptySpot;
    private CameraController cameraController;
    private ExplorationController playerController;
    private DialogManager dialogManager;
    private ShopHandlers dialogHandlers;


    #endregion


    void OnEnable()
    {
        ExplorationController.NavigateShop += HandleShoppingInput;
    }
    #region MonoBehaviour Methods
    private void Start()
    {
        emptySpot = Resources.Load<Item>(ItemsPath + "/Empty");
        shopItems = new Item[6];
        shopItems[0] = Resources.Load<Item>(ItemsPath + "/Baits/AdvanceBait");
        shopItems[1] = Resources.Load<Item>(ItemsPath + "/Hats/FancyHat");
        shopItems[2] = Resources.Load<Item>(ItemsPath + "/Rods/AdvanceRod");
        shopItems[3] = Resources.Load<Item>(ItemsPath + "/Baits/RareBait");
        shopItems[4] = Resources.Load<Item>(ItemsPath + "/Hats/PremiumHat");
        shopItems[5] = Resources.Load<Item>(ItemsPath + "/Rods/RareRod");
        InitializeReferences();
        InitializeShopItemPositions();
        SpawnItems();
    }

    private void InitializeReferences()
    {
        dialogHandlers = GetComponent<ShopHandlers>();
        cameraController = FindObjectOfType<CameraController>();
        playerController = FindObjectOfType<ExplorationController>();
        dialogManager = FindObjectOfType<DialogManager>();
    }
    private void InitializeShopItemPositions()
    {
        Transform shopPositions = transform.Find("ShopPositions");
        if (shopPositions != null)
        {
            foreach (Transform childTransform in shopPositions)
            {
                if (childTransform != null)
                {
                    GameObject childPosition = childTransform.gameObject;
                    shopItemPositions.Add(childPosition);
                }
            }
        }
    }
    void OnDisable()
    {
        ExplorationController.NavigateShop -= HandleShoppingInput;
    }
    #endregion

    #region Public Methods

    public void FocusItem()
    {
        cameraController.SetState(new ShopItemCamera(cameraController));
        cameraController.explorationCamera.SetShopItem(shopItemPositions[focusedShopItemIndex].transform.position);
    }

    /// <summary>
    /// Opens the shop.
    /// </summary>
    public IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(0.5f);
        playerController.SetState(new Shopping(playerController));
        FocusItem();
        UpdateShopDialog();
    }

    /// <summary>
    /// Closes the shop.
    /// </summary>
    public void CloseShop()
    {
        dialogManager.EndDialog();
        playerController.SetState(new ExplorationIdle(playerController));
        cameraController.SetState(new PlayerCamera(cameraController));
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
        UpdateShopDialog();
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


    public void UpdateShopDialog()
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
    private void SpawnObject(int index, Item item)
    {
        shopItems[index] = item;
        GameObject newObject = Instantiate(item.model, shopItemPositions[index].transform.position, Quaternion.identity);
        newObject.transform.SetParent(shopItemPositions[index].transform, false);
        newObject.transform.position = shopItemPositions[index].transform.position;
    }

    private void SpawnEmptySpot(int index)
    {
        SpawnObject(index, emptySpot);
    }

    private void SpawnShopItem(int index)
    {
        SpawnObject(index, shopItems[index]);
    }

}
#endregion
