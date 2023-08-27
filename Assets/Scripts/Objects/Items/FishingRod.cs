using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "FishingRod", menuName = "ScriptableObjects/FishingRod", order = 1)]
public class FishingRod : Item
{
    public int reelInSpeed;
    public int throwRange;



    public FishingRod() : base()
    {
        itemTag = ItemTag.FishingRod;
    }


    public FishingRod(FishingRodData fishingRodData)
    {
        id = fishingRodData.id;
        name = fishingRodData.name;
        reelInSpeed = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
        description = fishingRodData.description;
        price = fishingRodData.price;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.Inventory.FoundFishingRods = MainManager.Instance.Inventory.FoundFishingRods.Append(this).ToList();
    }
    public static FishingRod SetFishingRod(FishingRodData fishingRodData)
    {
        FishingRod fishingRod = CreateInstance<FishingRod>();
        fishingRod.id = fishingRodData.id;
        fishingRod.name = fishingRodData.name;
        fishingRod.description = fishingRodData.description;
        fishingRod.price = fishingRodData.price;
        fishingRod.reelInSpeed = fishingRodData.strength;
        fishingRod.throwRange = fishingRodData.throwRange;
        return fishingRod;
    }
}
