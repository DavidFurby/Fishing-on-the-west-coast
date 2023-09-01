using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    // Start is called before the first frame update

    //Nodes that should present text instantly
    public enum InstantTextNodes
    {
        ShopItem,
    }

    void Start()
    {
        Conversation.StartConversation += StartDialog;
        SetDayHandler();
    }

    private void SetDayHandler()
    {
        int day = MainManager.Instance.Days;
        dialogueRunner.AddCommandHandler("getDayOfMonth", () =>
        {
            dialogueRunner.VariableStorage.SetValue("$day", day);
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
        dialogueRunner.Stop();
    }
    public bool CurrentNodeShouldShowTextDirectly()
    {
        return System.Enum.GetNames(typeof(InstantTextNodes)).Contains(dialogueRunner.CurrentNodeName);
    }
    public void AddCommandHandler(string commandName, System.Action commandHandler)
    {
        dialogueRunner.AddCommandHandler(commandName, commandHandler);
    }
    public void SetVariableValue(string variableName, string value)
    {
        dialogueRunner.VariableStorage.SetValue(variableName, value);
    }

    public void SetVariableValue(string variableName, int value)
    {
        dialogueRunner.VariableStorage.SetValue(variableName, value);
    }

}
