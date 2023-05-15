using UnityEngine;

public class Conversation : MonoBehaviour, IInteractible
{
    // Start is called before the first frame update
    [SerializeField] DialogController dialogController;
    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        string[] dialogArray = { "Jararararar", "Har du f�tt n�n fisk?", "Bjud lite kanske?" };
        dialogController.InitiateDialog("Harry", dialogArray);
    }
}
