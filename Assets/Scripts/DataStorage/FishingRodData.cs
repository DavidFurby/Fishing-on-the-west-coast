using System;

[Serializable]
public class FishingRodData
{
    public string fishingRodName;
    public string description;
    public int strength;
    public int throwRange;
    public int price;

    public FishingRodData(FishingRod fishingRod)
    {
        fishingRodName = fishingRod.FishingRodName;
        description = fishingRod.Description;
        strength = fishingRod.Strength;
        throwRange = fishingRod.ThrowRange;
        price = fishingRod.Price;
    }
}