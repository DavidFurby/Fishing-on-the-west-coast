using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "FishingRod", menuName = "ScriptableObjects/FishingRod", order = 1)]
public class FishingRod : Item
{
    [SerializeField] public int strength;
    [SerializeField] public int throwRange;

    public FishingRod() : base()
    {
        itemTag = ItemTag.FishingRod;
    }


    public FishingRod(FishingRodData fishingRodData)
    {
        id = fishingRodData.id;
        name = fishingRodData.name;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
        description = fishingRodData.description;
        price = fishingRodData.price;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.game.FoundFishingRods = MainManager.Instance.game.FoundFishingRods.Append(this).ToList();
    }
    public static FishingRod SetFishingRod(FishingRodData fishingRodData)
    {
        FishingRod fishingRod = CreateInstance<FishingRod>();
        fishingRod.id = fishingRodData.id;
        fishingRod.name = fishingRodData.name;
        fishingRod.description = fishingRodData.description;
        fishingRod.price = fishingRodData.price;
        fishingRod.strength = fishingRodData.strength;
        fishingRod.throwRange = fishingRodData.throwRange;
        return fishingRod;
    }
}
