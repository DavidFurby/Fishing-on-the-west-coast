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
