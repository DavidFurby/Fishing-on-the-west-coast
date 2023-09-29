using UnityEngine;

public class ShopHandlers : MonoBehaviour
{
    private DialogManager dialogManager;
    private Shop shop;

    private void Start()
    {
        shop = GetComponent<Shop>();
        dialogManager = FindObjectOfType<DialogManager>();
        OpenShopHandler();
        SetTokens();
        LockShopControls();
        BuyShopItem();
    }

    private void OpenShopHandler()
    {
        dialogManager.AddHandler("openShop", () =>
        {
            StartCoroutine(shop.OpenShop());
        });
    }

    public void SetShopItemHandler(Item shopItem)
    {
        dialogManager.RemoveHandler("setShopItem");
        dialogManager.AddHandler("setShopItem", () =>
        {
            dialogManager.SetVariableValue("$shopItemId", shopItem.id);
            dialogManager.SetVariableValue("$shopItemName", shopItem.itemName);
            dialogManager.SetVariableValue("$shopItemPrice", shopItem.price);
            dialogManager.SetVariableValue("$shopItemDescription", shopItem.description);
        });
    }

    public void SetTokens()
    {
        dialogManager.RemoveHandler("setTokens");
        dialogManager.AddHandler("setTokens", () => dialogManager.SetVariableValue("$currentTokens", MainManager.Instance.TotalCatches));
    }

    public void LockShopControls()
    {
        dialogManager.RemoveHandler("lockShopControls");
        dialogManager.AddHandler("lockShopControls", () =>
        {
            shop.pauseShoppingControls = !shop.pauseShoppingControls;
        });
    }

    private void BuyShopItem()
    {
        dialogManager.AddHandler("buyShopItem", () =>
        {
            shop.BuyItem();
        });
    }

}
