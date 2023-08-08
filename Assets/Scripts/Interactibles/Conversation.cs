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
        Neighbour,
        NeigborsWife,
        Fisherman,
        Dog,
        ShopKeeper
    }
    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        dialogManager.StartDialog(character.ToString());

    }
}
