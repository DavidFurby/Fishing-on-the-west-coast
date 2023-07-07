using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Bait : Item
{
    [SerializeField] private BaitTag baitTag;

    public enum BaitTag
    {
        BasicBait,
        AdvanceBait,
        RareBait,
    }
    public int Level { get; set; }

    private void Awake()
    {
        SetBaitVariables();
    }

    private void SetBaitVariables()
    {
        itemTag = ItemTag.Bait;
        switch (baitTag)
        {
            case BaitTag.BasicBait:
                Id = 1;
                Name = "Basic Bait";
                Level = 1;
                Description = "A basic bait given at childbirth";
                Price = 0;
                break;
            case BaitTag.AdvanceBait:
                Id = 2;
                Name = "Advanced Bait";
                Level = 2;
                Description = "A more advanced bait not as easily found";
                Price = 5;
                break;
            case BaitTag.RareBait:
                Id = 3;
                Name = "Premium Bait";
                Level = 3;
                Description = "A premium bait very rarely found";
                Price = 10;
                break;
        }
    }

    public Bait(BaitData baitData)
    {
        Id = baitData.id;
        Name = baitData.baitName;
        Level = baitData.level;
        Description = baitData.description;
        Price = baitData.price;
    }

    public static Bait SetBait(Game game, BaitData baitData)
    {
        Bait bait = game.gameObject.AddComponent<Bait>();
        bait.Id = baitData.id;
        bait.Name = baitData.baitName;
        bait.Description = baitData.description;
        bait.Price = baitData.price;
        bait.Level = baitData.level;

        return bait;
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