using System.Collections.Generic;
using UnityEngine;

public class DialogList : MonoBehaviour
{
    private int day;
    private void Start()
    {
        day = MainManager.Instance.game.Days;
    }
    public enum GameCharacters
    {
        Player,
        Neighbor,
        NeigborsWife,
        Fisherman,
        Dog
    }
    private readonly Dictionary<GameCharacters, Queue<DialogItem>> characterDialogs = new();

    private void UpdateCharacterDialogs()
    {
        characterDialogs[GameCharacters.Neighbor] = GetDialogForCharacter(day, GameCharacters.Neighbor);
        characterDialogs[GameCharacters.NeigborsWife] = GetDialogForCharacter(day, GameCharacters.NeigborsWife);
    }

    private Queue<DialogItem> GetDialogForCharacter(int day, GameCharacters character)
    {
        var dialog = new Queue<DialogItem>();
        var dialogData = GetDialogDataForDayAndCharacter(day, character);
        foreach (var item in dialogData)
        {
            SetText(item.character, dialog, item.text);
        }
        return dialog;
    }
    private List<DialogItem> GetDialogDataForDayAndCharacter(int day, GameCharacters character)
    {
        // This method could retrieve the dialog data from a separate class or data structure
        // For example:
        return DialogDataManager.GetDialogData(day, character);
    }

    private static void SetText(GameCharacters character, Queue<DialogItem> dialog, string text)
    {
        dialog.Enqueue(new DialogItem { character = character, text = text });
    }

    public Queue<DialogItem> SelectDialogTree(GameCharacters character)
    {
        UpdateCharacterDialogs();
        if (characterDialogs.ContainsKey(character))
        {
            return characterDialogs[character];
        }
        return null;
    }

    public class DialogItem
    {
        public GameCharacters character;
        public string text;

    }
}