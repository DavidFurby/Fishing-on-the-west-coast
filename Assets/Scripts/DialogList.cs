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
    public GetCharacter character;

    private readonly Dictionary<GetCharacter, List<DialogItem>> characterDialogs = new()
    {
        { GetCharacter.Neighbor, new List<DialogItem> { new DialogItem { character = GetCharacter.Neighbor, text = "Haru n�n fisk?" }, new DialogItem { character = GetCharacter.Player, text = "Nope inget idag" } } },
        { GetCharacter.NeigborsWife, new List<DialogItem> { new DialogItem { character = GetCharacter.NeigborsWife, text = "H�rde fr�n gubben att du b�rjat fiska igen?" }, new DialogItem { character = GetCharacter.Player, text = "Ja jo de �r ju s�song igen s� de �r ju att ta f�r sig" } } }
    };


    public List<DialogItem> SelectDialogTree(GetCharacter character)
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

