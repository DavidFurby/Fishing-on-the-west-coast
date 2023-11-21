using System;
using UnityEngine;

public class CharacterDialog : MonoBehaviour, IInteractive
{
    [SerializeField] private GameCharacters character;
    internal CharacterController controller;
    private enum GameCharacters
    {
        Player,
        Lotta,
        Lars,
        NeighborsWife,
        Fisherman,
        Dog,
        ShopKeeper,
    }
    public static event Action<string> OnStartConversation;

    public void Interact()
    {
        OnStartConversation.Invoke(character.ToString());
        controller.SetState(new CharacterInDialog(controller));
    }

    internal void Initialize(CharacterController controller)
    {
        this.controller = controller;
    }
}
