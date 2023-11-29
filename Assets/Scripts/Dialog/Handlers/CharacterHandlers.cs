using System;
using System.Collections;
using UnityEngine;

public class CharacterHandlers : MonoBehaviour
{
    private DialogManager dialogManager;
    protected CharacterManager manager;

    internal void Initialize(CharacterManager manager)
    {
        this.manager = manager;
    }
    private void Start()
    {
        dialogManager = FindAnyObjectByType<DialogManager>();
        TriggerGesture();
    }

    public void TriggerGesture()
    {
        dialogManager.RemoveHandler("triggerGesture");
        dialogManager.AddHandler("triggerGesture", (string name) =>
        {
            if (Enum.TryParse(name, out GestureName gesture))
            {
                StartCoroutine(TriggerGestureWithDelay(gesture, 0.5f));
            }
            else
            {
                Debug.Log($"Invalid gesture name: {name}");
            }
        });
    }

    private IEnumerator TriggerGestureWithDelay(GestureName gesture, float delay)
    {
        yield return new WaitForSeconds(delay);

        manager.animations.TriggerGesture(gesture);
    }
}
