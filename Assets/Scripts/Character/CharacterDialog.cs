using System;
using UnityEngine;

public class CharacterDialog : MonoBehaviour, IInteractive
{
    internal CharacterManager manager;

    public static event Action<string> OnStartConversation;

    public void Interact()
    {
        OnStartConversation.Invoke(manager.character.name.ToString());
        manager.SetState(new CharacterInDialog(manager));
    }

    internal void Initialize(CharacterManager controller)
    {
        this.manager = controller;
    }
}
