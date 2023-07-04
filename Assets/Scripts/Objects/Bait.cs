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
    [SerializeField] private int price;

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
    public int Price
    {
        get => price;
        set => price = value;
    }

    public Bait()
    {
        baitName = "Basic bait";
        level = 1;
        description = "A basic bait made for basic fishermen";
        price = 0;
    }

    public Bait(BaitData baitData)
    {
        baitName = baitData.baitName;
        level = baitData.level;
        description = baitData.description;
        price = baitData.price;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.game.FoundBaits = MainManager.Instance.game.FoundBaits.Append(this).ToArray();
    }
}