using System;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    public static event Action OnEndDialog;

    [Flags]
    public enum InstantTextNodes
    {
        None = 0,
        ShopItem = 1 << 0,
    }

    private void OnEnable()
    {
        if (dialogueRunner != null)
        {
            Conversation.StartConversation += StartDialog;
            OnEndDialog += dialogueRunner.Stop;
        }
    }

    private void Start()
    {
        SetDayHandler();
    }

    private void OnDisable()
    {
        if (dialogueRunner != null)
        {
            Conversation.StartConversation -= StartDialog;
            OnEndDialog -= dialogueRunner.Stop;
        }
    }

    private void SetDayHandler()
    {
        int day = MainManager.Instance.Days;
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler("getDayOfMonth", () =>
            {
                dialogueRunner.VariableStorage.SetValue("$day", day);
            });
        }
    }

    public void RemoveHandler(string handlerName)
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.RemoveCommandHandler(handlerName);
        }
    }

    public void StartDialog(string node)
    {
        if (dialogueRunner != null)
        {
            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
            }
            dialogueRunner.StartDialogue(node);
        }
    }

    public void EndDialog()
    {
        OnEndDialog?.Invoke();
    }

    public bool CurrentNodeShouldShowTextDirectly()
    {
        if (dialogueRunner != null)
        {
            return Enum.GetValues(typeof(InstantTextNodes))
                .Cast<InstantTextNodes>()
                .Any(value => value != InstantTextNodes.None && dialogueRunner.CurrentNodeName.Contains(value.ToString()));
        }
        return false;
    }

    public void AddCommandHandler(string commandName, Action commandHandler)
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.AddCommandHandler(commandName, commandHandler);
        }
    }

    public void SetVariableValue(string variableName, string value)
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.VariableStorage.SetValue(variableName, value);
        }
    }

    public void SetVariableValue(string variableName, int value)
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.VariableStorage.SetValue(variableName, value);
        }
    }
}
