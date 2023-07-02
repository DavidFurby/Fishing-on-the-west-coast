using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private DialogView view;
    // Start is called before the first frame update

    //Nodes that should present text instantly
    public enum InstantTextNodes
    {
        ShopItem,
    }

    void Start()
    {
        SetDayHandler();
    }

    private void SetDayHandler()
    {
        dialogueRunner.AddCommandHandler("getDayOfMonth", () =>
        {
            int day = MainManager.Instance.game.Days;
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
        view.ShowDialog(false);
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

}
