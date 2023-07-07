using UnityEngine;

public class ShopHandlers : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private Shop shop;

    private void Start()
    {
        OpenShopHandler();
        SetTokens();
        LockShopControls();
        BuyShopItem();
    }

    private void OpenShopHandler()
    {
        dialogManager.AddCommandHandler("openShop", () =>
        {
            StartCoroutine(shop.OpenShop());
        });
    }
    public void SetShopItemHandler(Item shopItem)
    {
        dialogManager.RemoveHandler("setShopItem");
        dialogManager.AddCommandHandler("setShopItem", () =>
        {
            dialogManager.SetVariableValue("$shopItemId", shopItem.Id);
            dialogManager.SetVariableValue("$shopItemName", shopItem.Name);
            dialogManager.SetVariableValue("$shopItemPrice", shopItem.Price);
            dialogManager.SetVariableValue("$shopItemDescription", shopItem.Description);

        });
    }
    public void SetTokens()
    {
        dialogManager.RemoveHandler("setTokens");
        dialogManager.AddCommandHandler("setTokens", () => dialogManager.SetVariableValue("$currentTokens", MainManager.Instance.game.TotalCatches));
    }
    //lock controls if item has been selected in shop
    public void LockShopControls()
    {
        dialogManager.RemoveHandler("lockShopControls");
        dialogManager.AddCommandHandler("lockShopControls", () =>
        {
            shop.pauseShoppingControls = !shop.pauseShoppingControls;
        });
    }
    private void BuyShopItem()
    {
        dialogManager.RemoveHandler("buyShopItem");
        dialogManager.AddCommandHandler("buyShopItem", () =>
        {
            shop.BuyItem();
        });
    }
}
