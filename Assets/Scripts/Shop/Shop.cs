using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ShopHandlers), typeof(ShopInputManager))]
public class Shop : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items";

    #region Serialized Fields
    internal List<Item> shopItems;

    #endregion

    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
    private Item focusedShopItem;
    internal int focusedShopItemIndex = 0;
    private readonly List<GameObject> shopItemPositions = new();
    private Item emptySpot;
    internal CameraController cameraController;
    internal ExplorationController playerController;
    internal DialogManager dialogManager;
    private ShopHandlers dialogHandlers;


    #endregion
    #region MonoBehaviour Methods
    private void Awake()
    {
        Item originalEmptySpot = Resources.Load<Item>(ItemsPath + "/Empty");
        emptySpot = originalEmptySpot.CloneItem();
        InitializeShopItems();
        InitializeReferences();
        InitializeShopItemPositions();
        SpawnItems();
    }

private void InitializeShopItems()
{
    shopItems = new List<Item>();
    string[] itemNames = { "/Baits/AdvanceBait", "/Hats/FancyHat", "/Rods/AdvanceRod", "/Baits/RareBait", "/Hats/PremiumHat", "/Rods/RareRod" };
    for (int i = 0; i < itemNames.Length; i++)
    {
        Item originalItem = Resources.Load<Item>(ItemsPath + itemNames[i]);
        shopItems.Add(originalItem.CloneItem());
    }
}


    private void InitializeReferences()
    {
        if (!TryGetComponent(out dialogHandlers))
        {
            Debug.LogError("ShopHandlers component is missing.");
        }
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

    #endregion

    #region Public Methods

    public void FocusItem()
    {
        cameraController.SetState(new ShopItemCamera(cameraController));
        cameraController.explorationCamera.SetShopItem(shopItemPositions[focusedShopItemIndex].transform.position);
    }

    public IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(0.5f);
        playerController.SetState(new Shopping(playerController));
        FocusItem();
        UpdateShopDialog();
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
        focusedShopItem.model = replacement;
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
            if (i < shopItems.Count && shopItems[i] != null && !MainManager.Instance.Inventory.HasItem(shopItems[i]))
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
    public void SetFocusedShopItemIndex(int newIndex)
    {
        focusedShopItemIndex = newIndex;
        focusedShopItem = shopItems[focusedShopItemIndex];
    }
}
#endregion
