using System.Collections.Generic;
using static DialogList;

public static class DialogDataManager
{
    private static readonly Dictionary<(int day, GameCharacters character), List<DialogItem>> dialogData = new();

    static DialogDataManager()
    {
        // Initialize the dialog data for each day and character
        // For example:
        dialogData[(1, GameCharacters.Neighbor)] = new List<DialogItem>
        {
            new DialogItem { character = GameCharacters.Neighbor, text = "Have you caught some fish yet?" },
            new DialogItem { character = GameCharacters.Player, text = "Nope to earlt still" }
        };
        dialogData[(1, GameCharacters.NeigborsWife)] = new List<DialogItem>
        {
            new DialogItem { character = GameCharacters.NeigborsWife, text = "H�rde fr�n gubben att du hade b�rjat fiska igen?" },
            new DialogItem { character = GameCharacters.Player, text = "Ja m�ste ju ta f�r mig?" }
        };
        // ...
    }


    internal static List<DialogItem> GetDialogData(int day, GameCharacters character)
    {
        if (dialogData.TryGetValue((day, character), out var data))
        {
            return data;
        }
        return null;
    }
}