using System;
using UnityEngine;

public class Conversation : MonoBehaviour, IInteractive
{
    [SerializeField] private GameCharacters character;

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
    public static event Action<String> StartConversation;

    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        StartConversation.Invoke(character.ToString());
    }
}
