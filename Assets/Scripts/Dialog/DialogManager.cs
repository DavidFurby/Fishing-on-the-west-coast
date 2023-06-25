using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private Shop shop;
    [SerializeField] private DialogView view;
    [SerializeField] private DialogListView listView;
    // Start is called before the first frame update
    void Start()
    {
        SetDayHandler();
        OpenShopHandler();
        BuyShopItem();
    }

    private void SetDayHandler()
    {
        dialogueRunner.AddCommandHandler("getDayOfMonth", () =>
        {
            int day = MainManager.Instance.game.Days;
            dialogueRunner.VariableStorage.SetValue("$day", day);
        });
    }
    private void OpenShopHandler()
    {
        dialogueRunner.AddCommandHandler("openShop", () =>
        {
            shop.OpenShop();
        });
    }
    public void SetShopItemNameHandler(ShopItem shopItem)
    {
        SetTextRevealCoroutine();
        RemoveHandler("setShopItemName");
        dialogueRunner.AddCommandHandler("setShopItemName", () =>
        {
            dialogueRunner.VariableStorage.SetValue("$shopItemName", shopItem.name);
            dialogueRunner.VariableStorage.SetValue("$shopItemPrice", shopItem.Price);
            dialogueRunner.VariableStorage.SetValue("$shopItemDescription", shopItem.Description);
        });
    }
    public void SetTextRevealCoroutine()
    {
        RemoveHandler("setTextRevealCoroutine");
        dialogueRunner.AddCommandHandler("setTextRevealCoroutine", () =>
        {
            view.SetTextRevealCoroutine();
        });
    }
    private void BuyShopItem()
    {
        dialogueRunner.AddCommandHandler("buyShopItem", () =>
        {
            shop.BuyItem();
        });
    }
    public void RemoveHandler(string handlerName)
    {
        dialogueRunner.RemoveCommandHandler(handlerName);
    }
    public void StartDialog(string node)
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        dialogueRunner.StartDialogue(node);
    }
    public void EndDialog()
    {
        view.ShowDialog(false);
        listView.CloseListView();
        dialogueRunner.Stop();
    }
}
