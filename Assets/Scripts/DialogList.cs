using System.Collections.Generic;
using UnityEngine;

public class DialogList : MonoBehaviour
{
    private int day;
    private void Start()
    {
        day = MainManager.Instance.game.Days;
    }
    public enum GetCharacter
    {
        Player,
        Neighbor,
        NeigborsWife,
        Fisherman,
        Dog
    }
    private readonly Dictionary<GetCharacter, Queue<DialogItem>> characterDialogs = new();

    private void UpdateCharacterDialogs()
    {
        switch (day)
        {
            case 1:
                characterDialogs[GetCharacter.Neighbor] = new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.Neighbor, text = "Day 1 dialog for Neighbor" }, new DialogItem { character = GetCharacter.Player, text = "Day 1 response from Player" } });
                characterDialogs[GetCharacter.NeigborsWife] = new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.NeigborsWife, text = "Day 1 dialog for NeighborsWife" }, new DialogItem { character = GetCharacter.Player, text = "Day 1 response from Player" } });
                return;
            case 2:
                characterDialogs[GetCharacter.Neighbor] = new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.Neighbor, text = "Day 2 dialog for Neighbor" }, new DialogItem { character = GetCharacter.Player, text = "Day 2 response from Player" } });
                characterDialogs[GetCharacter.NeigborsWife] = new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.NeigborsWife, text = "Day 2 dialog for NeighborsWife" }, new DialogItem { character = GetCharacter.Player, text = "Day 2 response from Player" } });
                return;
        }
    }

    public Queue<DialogItem> SelectDialogTree(GetCharacter character)
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
        public GetCharacter character;
        public string text;

    }
}