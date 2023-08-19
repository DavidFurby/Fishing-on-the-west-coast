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
    [SerializeField] DialogManager dialogManager;
    Action advanceHandler;
    private string dialogueLine;
    private Coroutine textRevealCoroutine;
    public static event Action EndDialog;


    private void Start()
    {
        ShowDialog(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && container.gameObject.activeSelf)
        {
            if (textGUI.text.Length == dialogueLine.Length)
            {
                UserRequestedViewAdvancement();
            }
            else
            {
                StopCoroutine(textRevealCoroutine);
                textGUI.text = dialogueLine;
            }
        }
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowDialog(true);
        this.dialogueLine = dialogueLine.TextWithoutCharacterName.Text;
        speakerGUI.text = dialogueLine.CharacterName;
        advanceHandler = requestInterrupt;

        if (!dialogManager.CurrentNodeShouldShowTextDirectly())
        {
            textRevealCoroutine = StartCoroutine(RevealText(this.dialogueLine));
        }
        else
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
        if (container.gameObject.activeSelf)
        {
            advanceHandler?.Invoke();
        }
    }

    public override void DialogueComplete()
    {
        ShowDialog(false);
        Invoke(nameof(ActivateControls), 0.5f);
        base.DialogueComplete();
    }

    private void ActivateControls()
    {
        EndDialog?.Invoke();
    }

    public void ShowDialog(bool active)
    {
        container.gameObject.SetActive(active);
    }

    #region Private Methods

    private IEnumerator RevealText(string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textGUI.text = fullText[..i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    #endregion
}
