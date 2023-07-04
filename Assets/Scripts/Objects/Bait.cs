using System;
using System.Linq;
using UnityEngine;
using static Game;

[Serializable]
public class Bait : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string baitName;
    [SerializeField] private int level;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private ItemTag itemTag = ItemTag.Bait;

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
    public ItemTag ItemTag
    {
        get => itemTag;
        set => itemTag = value;
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
    public static Bait SetBait(Game game, int id, string name, string description, int price)
    {
        Bait bait = game.gameObject.AddComponent<Bait>();
        bait.id = id;
        bait.baitName = name;
        bait.Description = description;
        bait.Price = price;
        return bait;
    }

    public static Bait CreateBait(int id, string name, int price, string description)
    {
        var itemGameObject = new GameObject("Bait");
        var item = itemGameObject.AddComponent<Bait>();
        item.Id = id;
        item.BaitName = name;
        item.Price = price;
        item.Description = description;
        return item;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.game.FoundBaits = MainManager.Instance.game.FoundBaits.Append(this).ToArray();
    }
}