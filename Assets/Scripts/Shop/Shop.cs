using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ShopHandlers))]
[RequireComponent(typeof(ShopInputManager))]
[RequireComponent(typeof(ShopItemSpawner))]
public class Shop : MonoBehaviour
{


    #region Private Fields
    [HideInInspector] public bool pauseShoppingControls;
    private Item focusedShopItem;
    internal int focusedShopItemIndex = 0;
    internal CameraController cameraController;
    internal DialogManager dialogManager;
    private ShopHandlers dialogHandlers;
    internal ShopItemSpawner itemSpawner;


    #endregion
    #region MonoBehaviour Methods
    private void Start()
    {
        InitializeReferences();
        itemSpawner = GetComponent<ShopItemSpawner>();
        itemSpawner.InitializeShopItemPositions(transform.Find("ShopPositions"));
        itemSpawner.SpawnItems();
        focusedShopItem = itemSpawner.ShopItems[focusedShopItemIndex];
    }

    private void InitializeReferences()
    {
        if (!TryGetComponent(out dialogHandlers))
        {
            Debug.LogError("ShopHandlers component is missing.");
        }
        cameraController = FindObjectOfType<CameraController>();
        dialogManager = FindObjectOfType<DialogManager>();
    }
    #endregion

    #region Public Methods

    public void FocusItem()
    {
        cameraController.SetState(new ShopItemCamera());
        cameraController.explorationCamera.SetShopItem(itemSpawner.shopItemPositions[focusedShopItemIndex].transform.position);
    }

    public IEnumerator OpenShop()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerController.Instance.SetState(new Shopping());
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
        Destroy(focusedShopItem.model);
        GameObject replacementModel = Instantiate(itemSpawner.emptySpot.model, itemSpawner.shopItemPositions[focusedShopItemIndex].transform.position, focusedShopItem.model.transform.rotation);
        replacementModel.transform.parent = transform;
        replacementModel.transform.localScale = itemSpawner.shopItemPositions[focusedShopItemIndex].transform.localScale;
        Item replacementItem = Instantiate(itemSpawner.emptySpot, replacementModel.transform);
        itemSpawner.ShopItems[focusedShopItemIndex] = replacementItem;
        focusedShopItem = replacementItem;
    }



    public void UpdateShopDialog()
    {
        dialogHandlers.SetShopItemHandler(focusedShopItem);
        dialogManager.StartDialog("ShopItem");
    }

    public void SetFocusedShopItemIndex(int newIndex)
    {
        focusedShopItemIndex = newIndex;
        focusedShopItem = itemSpawner.ShopItems[focusedShopItemIndex];
    }
}
#endregion
