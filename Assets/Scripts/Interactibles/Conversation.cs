using UnityEngine;
using Yarn.Unity;

public class Conversation : MonoBehaviour, IInteractible
{
    // Start is called before the first frame update
    [SerializeField] GameCharacters character;
    [SerializeField] DialogueRunner runner;

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
        if (!runner.IsDialogueRunning)
        {
            // Start the "Start" node
            runner.StartDialogue(character.ToString());
        }

    }
}
