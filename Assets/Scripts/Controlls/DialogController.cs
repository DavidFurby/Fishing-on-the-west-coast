using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogPanel;
    [SerializeField] TextMeshProUGUI speaker;
    [SerializeField] TextMeshProUGUI dialog;
    [SerializeField] PlayerController playerController;
    private List<DialogList.DialogItem> currentDialogList;
    private int currentDialogIndex = 0;

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && dialogPanel.activeSelf)
        {
            NextDialog();
        }
    }

    private void NextDialog()
    {
        if (currentDialogIndex < currentDialogList.Count - 1)
        {
            currentDialogIndex++;
            dialog.text = currentDialogList[currentDialogIndex].text;
            speaker.text = currentDialogList[currentDialogIndex].character.ToString();
        }
        else
        {
            EndDialog();
        }
    }
    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        playerController.pauseControlls = false;
        ResetDialog();
    }
    private void ResetDialog()
    {
        currentDialogIndex = 0;
        currentDialogList = null;
        speaker.text = null;
        dialog.text = null;
    }

    internal void InitiateDialog(List<DialogList.DialogItem> dialog)
    {
        speaker.text = dialog[0].character.ToString();
        this.dialog.text = dialog[0].text;
        currentDialogList = dialog;
        dialogPanel.SetActive(true);
        playerController.pauseControlls = true;
    }
}
