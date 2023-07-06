using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class FishingRod : Item
{
    public const Game.ItemTag rod = Game.ItemTag.FishingRod;
    [SerializeField] private RodTag rodTag;

    public enum RodTag
    {
        BasicRod,
        AdvancedRod,
        RareRod
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
                Name = "Basic Rod";
                Description = "A basic rod given at childbirth";
                Price = 0;
                Strength = 20;
                ThrowRange = 100;
                break;
            case RodTag.AdvancedRod:
                Id = 2;
                Name = "Advanced Rod";
                Description = "A more advanced rod not as easily found";
                Price = 5;
                Strength = 50;
                ThrowRange = 200;
                break;
            case RodTag.RareRod:
                Id = 3;
                Name = "Premium Rod";
                Description = "A premium rod very rarely found";
                Price = 10;
                Strength = 100;
                ThrowRange = 300;
                break;
        }
    }

    public FishingRod(FishingRodData fishingRodData)
    {
        Name = fishingRodData.name;
        Strength = fishingRodData.strength;
        ThrowRange = fishingRodData.throwRange;
        Description = fishingRodData.description;
        Price = fishingRodData.price;
    }
    public static FishingRod SetFishingRod(Game game, FishingRodData fishingRodData)
    {
        FishingRod fishingRod = game.gameObject.AddComponent<FishingRod>();
        fishingRod.Id = fishingRodData.id;
        fishingRod.Name = fishingRodData.name;
        fishingRod.Description = fishingRodData.description;
        fishingRod.Price = fishingRodData.price;
        fishingRod.Strength = fishingRodData.strength;
        fishingRod.ThrowRange = fishingRodData.throwRange;
        return fishingRod;
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
