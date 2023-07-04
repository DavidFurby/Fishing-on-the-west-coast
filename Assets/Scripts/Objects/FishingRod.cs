using System;
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
        fishingRodName = "Basic rod";
        strength = 20;
        throwRange = 100;
        description = "A rod for beginners. Made for seas of the kinder sort";
        price = 0;
    }

    public FishingRod(FishingRodData fishingRodData)
    {
        fishingRodName = fishingRodData.fishingRodName;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
        description = fishingRodData.description;
        price = fishingRodData.price;
    }
    public static FishingRod SetFishingRod(Game game, int id, string name, string description, int price)
    {
        FishingRod fishingRod = game.gameObject.AddComponent<FishingRod>();
        fishingRod.id = id;
        fishingRod.fishingRodName = name;
        fishingRod.Description = description;
        fishingRod.Price = price;
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
        MainManager.Instance.game.FoundFishingRods = MainManager.Instance.game.FoundFishingRods.Append(this).ToArray();
    }
}
