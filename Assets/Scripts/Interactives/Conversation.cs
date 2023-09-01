using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn.Unity;

public class Conversation : MonoBehaviour, IInteractive
{
    // Start is called before the first frame update
    [SerializeField] private GameCharacters character;

    public enum GameCharacters
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
