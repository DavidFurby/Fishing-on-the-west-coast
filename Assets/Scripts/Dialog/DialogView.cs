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
    [SerializeField] PlayerController playerController;
    [SerializeField] DialogManager dialogManager;
    Action advanceHandler;
    private string dialogueLine;
    private Coroutine textRevealCoroutine;
    private bool useTextRevealCoroutine;
    private void Start()
    {
        useTextRevealCoroutine = false;
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



    private IEnumerator RevealText(string fullText)
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textGUI.text = fullText[..i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
