using System;
using System.Linq;
using UnityEngine;
using static Game;

[Serializable]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private RodTag rodTag;

    public enum RodTag
    {
        BasicRod,
        AdvancedRod,
        RareRod
    }
    public int Id
    {
        get;
        set;
    }

    public string FishingRodName
    {
        get;
        set;
    }
    public string Description
    {
        get;
        set;
    }
    public int Strength
    {
        get;
        set;
    }

    public int ThrowRange
    {
        get;
        set;
    }
    public int Price
    {
        get;
        set;
    }

    public ItemTag ItemTag
    {
        get;
        set;
    } = ItemTag.FishingRod;

    public FishingRod(RodTag rodTag)
    {
        this.rodTag = rodTag;
        SetRodVariables();
    }


    private void Start()
    {
        SetRodVariables();
    }
    private void SetRodVariables()
    {
        switch (rodTag)
        {
            case RodTag.BasicRod:
                Id = 1;
                FishingRodName = "Basic Rod";
                Description = "A basic rod given at childbirth";
                Price = 0;
                Strength = 20;
                ThrowRange = 100;
                break;
            case RodTag.AdvancedRod:
                Id = 2;
                FishingRodName = "Advanced Rod";
                Description = "A more advanced rod not as easily found";
                Price = 5;
                Strength = 50;
                ThrowRange = 200;
                break;
            case RodTag.RareRod:
                Id = 3;
                FishingRodName = "Premium Rod";
                Description = "A premium rod very rarely found";
                Price = 10;
                Strength = 100;
                ThrowRange = 300;
                break;
        }
    }
    public FishingRod()
    {

    }


    public FishingRod(FishingRodData fishingRodData)
    {
        FishingRodName = fishingRodData.fishingRodName;
        Strength = fishingRodData.strength;
        ThrowRange = fishingRodData.throwRange;
        Description = fishingRodData.description;
        Price = fishingRodData.price;
    }
    public static FishingRod SetFishingRod(Game game, FishingRodData fishingRodData)
    {
        FishingRod fishingRod = game.gameObject.AddComponent<FishingRod>();
        fishingRod.Id = fishingRodData.id;
        fishingRod.FishingRodName = fishingRodData.fishingRodName;
        fishingRod.Description = fishingRodData.description;
        fishingRod.Price = fishingRodData.price;
        fishingRod.Strength = fishingRodData.strength;
        fishingRod.ThrowRange = fishingRodData.throwRange;
        return fishingRod;
    }

    public static FishingRod CreateFishingRod(int id, string name, int price, string description)
    {
        var itemGameObject = new GameObject("FishingRod");
        var item = itemGameObject.AddComponent<FishingRod>();
        item.Id = id;
        item.FishingRodName = name;
        item.Price = price;
        item.Description = description;
        return item;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.game.FoundFishingRods = MainManager.Instance.game.FoundFishingRods.Append(this).ToList();
    }
    public static FishingRod[] SetAvailableRods(Game game)
    {
        FishingRod basicRod = game.gameObject.AddComponent<FishingRod>();
        basicRod.rodTag = RodTag.BasicRod;
        basicRod.SetRodVariables();

        FishingRod advancedRod = game.gameObject.AddComponent<FishingRod>();
        advancedRod.rodTag = RodTag.AdvancedRod;
        advancedRod.SetRodVariables();

        FishingRod rareRod = game.gameObject.AddComponent<FishingRod>();
        rareRod.rodTag = RodTag.RareRod;
        rareRod.SetRodVariables();
        return new FishingRod[] { basicRod, advancedRod, rareRod };
    }
}
