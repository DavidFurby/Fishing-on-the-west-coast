using System;
using System.Collections.Generic;
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
    public List<Bait> availableBaits = new();

    public Bait()
    {

    }


    public Bait(BaitData baitData)
    {
        baitName = baitData.baitName;
        level = baitData.level;
        description = baitData.description;
        price = baitData.price;
    }
    public static Bait SetBait(Game game, BaitData baitData)
    {
        Bait bait = game.gameObject.AddComponent<Bait>();
        bait.id = baitData.id;
        bait.baitName = baitData.baitName;
        bait.Description = baitData.description;
        bait.Price = baitData.price;
        bait.level = baitData.level;
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
        MainManager.Instance.game.FoundBaits = MainManager.Instance.game.FoundBaits.Append(this).ToList();
    }

    public static Bait[] SetAvailableBaits(Game game)
    {

        Bait basicBait = game.gameObject.AddComponent<Bait>();
        basicBait.Id = 1;
        basicBait.BaitName = "Basic Bait";
        basicBait.Level = 1;
        basicBait.Description = "A basic bait for catching common fish";
        basicBait.Price = 10;

        Bait advancedBait = game.gameObject.AddComponent<Bait>();
        advancedBait.Id = 2;
        advancedBait.BaitName = "Advanced Bait";
        advancedBait.Level = 2;
        advancedBait.Description = "A more advanced bait for catching rarer fish";
        advancedBait.Price = 20;

        Bait premiumBait = game.gameObject.AddComponent<Bait>();
        premiumBait.Id = 3;
        premiumBait.BaitName = "Premium Bait";
        premiumBait.Level = 3;
        premiumBait.Description = "A premium bait for catching the rarest fish";
        premiumBait.Price = 30;

        return new Bait[] { basicBait, advancedBait, premiumBait };
    }

}