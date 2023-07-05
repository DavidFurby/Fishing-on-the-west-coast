using System;
using System.Linq;
using UnityEngine;
using static Game;

[Serializable]
public class Bait : MonoBehaviour
{
    [SerializeField] private BaitTag baitTag;

    public enum BaitTag
    {
        BasicBait,
        AdvanceBait,
        RareBait,
    }

    public int Id { get; set; }
    public string BaitName { get; set; }
    public int Level { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public ItemTag ItemTag { get; set; } = ItemTag.Bait;


    public Bait(BaitTag baitTag)
    {
        this.baitTag = baitTag;
        SetBaitVariables();
    }

    private void Start()
    {
        SetBaitVariables();
    }

    private void SetBaitVariables()
    {
        switch (baitTag)
        {
            case BaitTag.BasicBait:
                Id = 1;
                BaitName = "Basic Bait";
                Level = 1;
                Description = "A basic bait given at childbirth";
                Price = 0;
                break;
            case BaitTag.AdvanceBait:
                Id = 2;
                BaitName = "Advanced Bait";
                Level = 2;
                Description = "A more advanced bait not as easily found";
                Price = 5;
                break;
            case BaitTag.RareBait:
                Id = 3;
                BaitName = "Premium Bait";
                Level = 3;
                Description = "A premium bait very rarely found";
                Price = 10;
                break;
        }
    }

    public Bait()
    {

    }

    public Bait(BaitData baitData)
    {
        BaitName = baitData.baitName;
        Level = baitData.level;
        Description = baitData.description;
        Price = baitData.price;
    }

    public static Bait SetBait(Game game, BaitData baitData)
    {
        Bait bait = game.gameObject.AddComponent<Bait>();
        bait.Id = baitData.id;
        bait.BaitName = baitData.baitName;
        bait.Description = baitData.description;
        bait.Price = baitData.price;
        bait.Level = baitData.level;

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
        MainManager.Instance.game.FoundBaits =
            MainManager.Instance.game.FoundBaits.Append(this).ToList();
    }

    public static Bait[] SetAvailableBaits(Game game)
    {
        Bait basicBait = game.gameObject.AddComponent<Bait>();
        basicBait.baitTag = BaitTag.BasicBait;
        basicBait.SetBaitVariables();

        Bait advancedBait = game.gameObject.AddComponent<Bait>();
        advancedBait.baitTag = BaitTag.AdvanceBait;
        advancedBait.SetBaitVariables();

        Bait rareBait = game.gameObject.AddComponent<Bait>();
        rareBait.baitTag = BaitTag.RareBait;
        rareBait.SetBaitVariables();
        return new Bait[] { basicBait, advancedBait, rareBait };
    }
}