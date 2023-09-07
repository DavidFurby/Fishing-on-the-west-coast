using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    #region Fields

    private DialogueRunner dialogueRunner;

    public static event Action OnEndDialog;

    private Dictionary<string, bool> addedHandlers = new();

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        if (TryGetComponent<DialogueRunner>(out dialogueRunner))
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

    #endregion

    #region Private Methods

    private void SetDayHandler()
    {
        int day = MainManager.Instance.Days;
        dialogueRunner.AddCommandHandler(nameof(SetDayHandler), () =>
        {
            dialogueRunner.VariableStorage.SetValue("$day", day);
        });
    }

    #endregion

    #region Public Methods

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

    public void AddCommandHandler(string commandName, Action commandHandler)
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

    public void AddHandler(string handlerName, System.Action handler)
    {
        AddCommandHandler(handlerName, handler);
        addedHandlers[handlerName] = true;
    }

    public void RemoveHandler(string handlerName)
    {
        if (HasHandler(handlerName))
        {
            dialogueRunner.RemoveCommandHandler(handlerName);
            addedHandlers.Remove(handlerName);
        }
    }

    public bool HasHandler(string handlerName)
    {
        return addedHandlers.ContainsKey(handlerName);
    }
    #endregion
}
