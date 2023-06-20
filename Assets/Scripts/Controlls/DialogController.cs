using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI speakerGUI;
    [SerializeField] private TextMeshProUGUI dialogGUI;
    [SerializeField] private PlayerController playerController;
    private Queue<DialogList.DialogItem> currentDialogQueue;

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && dialogPanel.activeSelf)
        {
            NextDialog();
        }
    }

    private void NextDialog()
    {
        if (currentDialogQueue.Count > 0)
        {
            var nextDialog = currentDialogQueue.Dequeue();
            dialogGUI.text = nextDialog.text;
            speakerGUI.text = nextDialog.character.ToString();
        }
        else
        {
            EndDialog();
        }
    }
    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        ResetDialog();
    }
    private void ResetDialog()
    {
        currentDialogQueue = null;
        speakerGUI.text = null;
        dialogGUI.text = null;
    }


    internal void InitiateDialog(Queue<DialogList.DialogItem> dialogQueue)
    {
        currentDialogQueue = new Queue<DialogList.DialogItem>(dialogQueue);
        var firstDialog = currentDialogQueue.Dequeue();
        speakerGUI.text = firstDialog.character.ToString();
        dialogGUI.text = firstDialog.text;
        dialogPanel.SetActive(true);
    }
}
