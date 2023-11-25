using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogManager : MonoBehaviour
{
    #region Fields

    private DialogueRunner dialogueRunner;
    public static event Action OnEndDialog;
    private readonly Dictionary<string, bool> addedHandlers = new();

    #endregion

    #region Unity Methods

    private void Start()
    {
        SetDayHandler();
    }

    private void OnEnable()
    {
        if (TryGetComponent(out dialogueRunner))
        {
            SubscribeToEvents();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    #endregion

    #region Event Subscription

    private void SubscribeToEvents()
    {
        CharacterDialog.OnStartConversation += StartDialog;
        dialogueRunner.onDialogueComplete.AddListener(EndDialog);
    }

    private void UnsubscribeFromEvents()
    {
        if (dialogueRunner != null)
        {
            CharacterDialog.OnStartConversation -= StartDialog;
            dialogueRunner.onDialogueComplete.RemoveListener(EndDialog);
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

    #region Dialogue Control

    public void StartDialog(string node)
    {
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        dialogueRunner.StartDialogue(node);
    }

    public void EndDialog()
    {
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
        OnEndDialog?.Invoke();
    }

    #endregion

    #region Dialogue Utilities

    public void SetVariableValue(string variableName, string value)
    {
        dialogueRunner.VariableStorage.SetValue(variableName, value);
    }

    public void SetVariableValue(string variableName, int value)
    {
        dialogueRunner.VariableStorage.SetValue(variableName, value);
    }

    public void AddHandler(string handlerName, Action handler)
    {
        dialogueRunner.AddCommandHandler(handlerName, handler);
        addedHandlers[handlerName] = true;
    }

    public void AddHandler(string handlerName, Action<string> handler)
    {
        dialogueRunner.AddCommandHandler(handlerName, handler);
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

    private bool HasHandler(string handlerName)
    {
        return addedHandlers.ContainsKey(handlerName);
    }

    #endregion
}
