using UnityEngine;
using Yarn.Unity;
using System;
using TMPro;
using System.Collections;

public class DialogView : DialogueViewBase
{

    [SerializeField] RectTransform container;
    Action advanceHandler;
    [SerializeField] TextMeshProUGUI speakerGUI;
    [SerializeField] TextMeshProUGUI textGUI;
    [SerializeField] PlayerController playerController;
    private string dialogueLine;
    private Coroutine textRevealCoroutine;
    private void Start()
    {
        ShowDialog(false);
    }
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowDialog(true);
        this.dialogueLine = dialogueLine.TextWithoutCharacterName.Text;
        speakerGUI.text = dialogueLine.CharacterName;
        advanceHandler = requestInterrupt;
        textRevealCoroutine = StartCoroutine(RevealText(this.dialogueLine));
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        ShowDialog(false);
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
        Invoke(nameof(ActivateControls), 0.5f);
        base.DialogueComplete();
    }

    private void ActivateControls()
    {
        if (playerController.playerStatus == PlayerController.PlayerStatus.Interacting)
        {
            playerController.SetPlayerStatus(PlayerController.PlayerStatus.StandBy);
        }
    }
    public void ShowDialog(bool active)
    {
        container.gameObject.SetActive(active);
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

    private IEnumerator RevealText(string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textGUI.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
