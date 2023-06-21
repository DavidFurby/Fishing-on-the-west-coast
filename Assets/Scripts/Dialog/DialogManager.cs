using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private Shop shop;
    // Start is called before the first frame update
    void Start()
    {
        SetDay();
        OpenShop();
    }

    private void SetDay()
    {
        dialogueRunner.AddCommandHandler("getDayOfMonth", () =>
        {
            int day = MainManager.Instance.game.Days;
            dialogueRunner.VariableStorage.SetValue("$day", day);
        });
    }
    private void OpenShop()
    {
        dialogueRunner.AddCommandHandler("openShop", () =>
        {
            dialogueRunner.Stop();
            SetShopItemName();
            shop.OpenShop();
            dialogueRunner.StartDialogue("ShopItem");
        });
    }
    public void SetShopItemName()
    {
        dialogueRunner.AddCommandHandler("setShopItemName", () =>
        {
            ShopItem shopItem = shop.focusedShopItem;
            dialogueRunner.VariableStorage.SetValue("$shopItemName", shopItem.name);
            dialogueRunner.VariableStorage.SetValue("$shopItemPrice", shopItem.Price);
            dialogueRunner.VariableStorage.SetValue("$shopItemDescription", shopItem.Description);
        });
    }
    public void RemoveHandler(string handlerName)
    {
        dialogueRunner.RemoveCommandHandler(handlerName);
    }
    public void RestartDialog(string node)
    {
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue(node);
    }
}
