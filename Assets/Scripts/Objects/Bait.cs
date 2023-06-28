using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Bait : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string baitName;
    [SerializeField] private int level;
    [SerializeField] private string description;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string BaitName
    {
        get => baitName;
        set => baitName = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }

    public Bait()
    {
        baitName = "Basic bait";
        level = 1;
        description = "A basic bait made for basic fishermen";
    }

    public Bait(BaitData baitData)
    {
        baitName = baitData.baitName;
        level = baitData.level;
        description = baitData.description;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.game.FoundBaits = MainManager.Instance.game.FoundBaits.Append(this).ToArray();
    }
}