using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogPanel;
    [SerializeField] TextMeshProUGUI speaker;
    [SerializeField] TextMeshProUGUI dialog;
    [SerializeField] PlayerController playerController;
    private string[] currentDialogList;
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
        if (currentDialogIndex < currentDialogList.Length - 1)
        {
            currentDialogIndex++;
            dialog.text = currentDialogList[currentDialogIndex];
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
    public void InitiateDialog(string speakerText, string[] dialogText)
    {
        speaker.text = speakerText;
        dialog.text = dialogText[0];
        currentDialogList = dialogText;
        dialogPanel.SetActive(true);
        playerController.pauseControlls = true;
    }
}
