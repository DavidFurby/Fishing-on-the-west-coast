using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Game;

[Serializable]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishingRodName;
    [SerializeField] private string description;
    [SerializeField] private int strength;
    [SerializeField] private int throwRange;
    [SerializeField] private int price;
    [SerializeField] private ItemTag itemTag = ItemTag.FishingRod;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string FishingRodName
    {
        get => fishingRodName;
        set => fishingRodName = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }
    public int Strength
    {
        get => strength;
        set => strength = value;
    }

    public int ThrowRange
    {
        get => throwRange;
        set => throwRange = value;
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

    public FishingRod()
    {

    }

    public FishingRod(FishingRodData fishingRodData)
    {
        fishingRodName = fishingRodData.fishingRodName;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
        description = fishingRodData.description;
        price = fishingRodData.price;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
    }
    public static FishingRod SetFishingRod(Game game, FishingRodData fishingRodData)
    {
        FishingRod fishingRod = game.gameObject.AddComponent<FishingRod>();
        fishingRod.id = fishingRodData.id;
        fishingRod.fishingRodName = fishingRodData.fishingRodName;
        fishingRod.Description = fishingRodData.description;
        fishingRod.Price = fishingRodData.price;
        fishingRod.strength = fishingRodData.strength;
        fishingRod.throwRange = fishingRodData.throwRange;
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
    public static FishingRod[] SetAvailableFishingRods(Game game)
    {
        FishingRod basicFishingRod = game.gameObject.AddComponent<FishingRod>();
        basicFishingRod.Id = 1;
        basicFishingRod.FishingRodName = "Basic Fishing Rod";
        basicFishingRod.Description = "A basic fishing rod for catching common fish";
        basicFishingRod.Strength = 10;
        basicFishingRod.ThrowRange = 100;
        basicFishingRod.Price = 10;

        FishingRod advancedFishingRod = game.gameObject.AddComponent<FishingRod>();
        advancedFishingRod.Id = 2;
        advancedFishingRod.FishingRodName = "Advanced Fishing Rod";
        advancedFishingRod.Description = "A more advanced fishing rod for catching rarer fish";
        advancedFishingRod.Strength = 20;
        advancedFishingRod.ThrowRange = 200;
        advancedFishingRod.Price = 20;

        FishingRod premiumFishingRod = game.gameObject.AddComponent<FishingRod>();
        premiumFishingRod.Id = 3;
        premiumFishingRod.FishingRodName = "Premium Fishing Rod";
        premiumFishingRod.Description = "A premium fishing rod for catching the rarest fish";
        premiumFishingRod.Strength = 30;
        premiumFishingRod.throwRange = 300;
        premiumFishingRod.price = 30;

        return new FishingRod[] { basicFishingRod, advancedFishingRod, premiumFishingRod };
    }
}
