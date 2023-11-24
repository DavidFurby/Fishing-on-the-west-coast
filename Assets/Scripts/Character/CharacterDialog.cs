using System;
using UnityEngine;

public class CharacterDialog : MonoBehaviour, IInteractive
{
    internal CharacterManager controller;

    public static event Action<string> OnStartConversation;

    public void Interact()
    {
        OnStartConversation.Invoke(controller.character.name.ToString());
        controller.SetState(new CharacterInDialog(controller));
    }

    internal void Initialize(CharacterManager controller)
    {
        this.controller = controller;
    }
}
