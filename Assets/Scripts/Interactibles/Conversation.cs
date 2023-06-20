using UnityEngine;
using Yarn.Unity;

public class Conversation : MonoBehaviour, IInteractible
{
    // Start is called before the first frame update
    [SerializeField] DialogList dialogList;
    [SerializeField] DialogList.GameCharacters character;
    [SerializeField] DialogueRunner runner;
    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        if (!runner.IsDialogueRunning)
        {
            // Start the "Start" node
            runner.StartDialogue("Start");
        }

    }
}
