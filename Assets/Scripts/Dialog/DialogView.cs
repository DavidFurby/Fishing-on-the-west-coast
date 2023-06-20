using UnityEngine;
using Yarn.Unity;
using System;
using TMPro;

public class DialogView : DialogueViewBase
{

    [SerializeField] RectTransform container;
    Action advanceHandler;
    [SerializeField] TextMeshProUGUI speaker;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] PlayerController playerController;

    private void Start()
    {
        ShowDialog(false);
    }
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowDialog(true);
        speaker.text = dialogueLine.CharacterName;
        text.text = dialogueLine.Text.Text;
        advanceHandler = requestInterrupt;
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
        playerController.SetPlayerStatus(PlayerController.PlayerStatus.StandBy);
    }
    public void ShowDialog(bool active)
    {
        container.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserRequestedViewAdvancement();
        }
    }
}
