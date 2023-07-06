using System;

[Serializable]
public class FishingRodData
{
    public int id;
    public string name;
    public string description;
    public int strength;
    public int throwRange;
    public int price;

    public FishingRodData(FishingRod fishingRod)
    {
        id = fishingRod.Id;
        name = fishingRod.Name;
        description = fishingRod.Description;
        strength = fishingRod.Strength;
        throwRange = fishingRod.ThrowRange;
        price = fishingRod.Price;
    }
}