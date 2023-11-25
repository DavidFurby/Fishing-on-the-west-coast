using System;
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
        SetGesture();
    }

    public void SetGesture()
    {
        dialogManager.RemoveHandler("setGesture");
        dialogManager.AddHandler("setGesture", (string name) =>
        {
            if (Enum.TryParse(name, out GestureName gesture))
            {
                manager.animations.TriggerGesture(gesture, true);
            }
            else
            {
                Debug.Log($"Invalid gesture name: {name}");
            }
        });
    }

}
