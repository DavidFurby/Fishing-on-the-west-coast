using System;
using UnityEngine;
using Yarn.Unity;

public class Conversation : MonoBehaviour, IInteractive
{
    // Start is called before the first frame update
    [SerializeField] private GameCharacters character;
    [SerializeField] private DialogManager dialogManager;

    public enum GameCharacters
    {
        Player,
        Neighbor,
        NeighborsWife,
        Fisherman,
        Dog,
        ShopKeeper
    }
        public static event Action StartConversation;

    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        dialogManager.StartDialog(character.ToString());

    }
}
