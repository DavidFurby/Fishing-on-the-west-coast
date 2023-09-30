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
    public static event Action<string> StartConversation;

    public void Interact()
    {
        StartConversation.Invoke(character.ToString());
        PlayerController.Instance.SetState(new Conversing());
    }

}
