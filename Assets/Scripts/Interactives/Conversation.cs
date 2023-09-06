using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn.Unity;

public class Conversation : MonoBehaviour, IInteractive
{
    [SerializeField] private GameCharacters character;

    private enum GameCharacters
    {
        Player,
        Neighbor,
        NeighborsWife,
        Fisherman,
        Dog,
        ShopKeeper
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
