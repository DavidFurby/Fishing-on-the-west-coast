using System.Collections.Generic;
using UnityEngine;

public class DialogList : MonoBehaviour
{
    public static DialogList Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public enum GetCharacter
    {
        Player,
        Neighbor,
        NeigborsWife,
        Fisherman,
        Dog
    }
    private readonly Dictionary<GetCharacter, Queue<DialogItem>> characterDialogs = new()
{
    { GetCharacter.Neighbor, new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.Neighbor, text = "Haru nån fisk?" }, new DialogItem { character = GetCharacter.Player, text = "Nope inget idag" } }) },
    { GetCharacter.NeigborsWife, new Queue<DialogItem>(new[] { new DialogItem { character = GetCharacter.NeigborsWife, text = "Hörde från gubben att du börjat fiska igen?" }, new DialogItem { character = GetCharacter.Player, text = "Ja jo de är ju säsong igen så de är ju att ta för sig" } }) }
};


    public Queue<DialogItem> SelectDialogTree(GetCharacter character)
    {
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

