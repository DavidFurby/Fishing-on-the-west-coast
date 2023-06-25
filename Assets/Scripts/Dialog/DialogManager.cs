using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private Shop shop;
    [SerializeField] private DialogView view;
    [SerializeField] private DialogListView listView;
    [SerializeField] private PlayerController playerController;
    // Start is called before the first frame update

    //Nodes that should present text instantly
    public enum InstantTextNodes
    {
        ShopItem,
    }

    void Start()
    {
        SetDayHandler();
        OpenShopHandler();
        BuyShopItem();
        SetShopItemNameHandler(shop.focusedShopItem);
        LockControls();
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
            StartCoroutine(shop.OpenShop());
        });
    }
    public void SetShopItemNameHandler(ShopItem shopItem)
    {
        RemoveHandler("setShopItemName");
        dialogueRunner.AddCommandHandler("setShopItemName", () =>
        {
            dialogueRunner.VariableStorage.SetValue("$shopItemName", shopItem.name);
            dialogueRunner.VariableStorage.SetValue("$shopItemPrice", shopItem.Price);
            dialogueRunner.VariableStorage.SetValue("$shopItemDescription", shopItem.Description);
        });
    }
    public void LockControls()
    {
        dialogueRunner.AddCommandHandler("lockControls", () =>
        {
            playerController.selectedItem = !playerController.selectedItem;
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
        dialogueRunner.Stop();
    }
    public bool CurrentNodeShouldShowTextDirectly()
    {
        return System.Enum.GetNames(typeof(InstantTextNodes)).Contains(dialogueRunner.CurrentNodeName);
    }

}
