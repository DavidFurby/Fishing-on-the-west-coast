using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour, IInteractible
{
    // Start is called before the first frame update
    [SerializeField] DialogController dialogController;
    [SerializeField] DialogList.GetCharacter character;
    public void Interact()
    {
        StartDialog();
    }
    public void StartDialog()
    {
        Debug.Log(character);
        Queue<DialogList.DialogItem> dialog = DialogList.Instance.SelectDialogTree(character);
        dialogController.InitiateDialog(dialog);


    }
}
