using UnityEngine;
using Yarn.Unity;
using System;
using TMPro;
using System.Collections;

public class DialogView : DialogueViewBase
{
    [SerializeField] RectTransform container;
    [SerializeField] TextMeshProUGUI speakerGUI;
    [SerializeField] TextMeshProUGUI textGUI;
    private DialogManager dialogManager;
    Action advanceHandler;
    private string dialogueLine;

    private void OnEnable()
    {

        DialogManager.OnEndDialog += () => ShowDialog(false);

    }

    private void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
        ShowDialog(false);
    }

    private void OnDisable()
    {

        DialogManager.OnEndDialog -= () => ShowDialog(true);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && container != null && container.gameObject.activeSelf)
        {
            if (textGUI != null && textGUI.text.Length == dialogueLine.Length)
            {
                UserRequestedViewAdvancement();
            }
            else
            {
                if (textGUI != null)
                {
                    textGUI.text = dialogueLine;
                }
            }
        }
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowDialog(true);
        this.dialogueLine = dialogueLine.TextWithoutCharacterName.Text;
        if (speakerGUI != null)
        {
            speakerGUI.text = dialogueLine.CharacterName;
        }
        advanceHandler = requestInterrupt;
        if (textGUI != null)
        {
            textGUI.text = this.dialogueLine;
        }

    }

    public override void DismissLine(Action onDismissalComplete)
    {
        onDismissalComplete();
    }

    public override void UserRequestedViewAdvancement()
    {
        if (container != null && container.gameObject.activeSelf)
        {
            advanceHandler?.Invoke();
        }
    }

    public override void DialogueComplete()
    {
        StartCoroutine(EndDialogAfterDelay(0.5f));
        base.DialogueComplete();
    }

    public void ShowDialog(bool active)
    {
        if (container != null)
        {
            container.gameObject.SetActive(active);
        }
    }

    #region Private Methods

    private IEnumerator EndDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (dialogManager != null)
        {
            dialogManager.EndDialog();
        }
    }

    #endregion
}
