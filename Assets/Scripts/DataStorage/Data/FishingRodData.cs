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
        id = fishingRod.id;
        name = fishingRod.name;
        description = fishingRod.description;
        strength = fishingRod.reelInSpeed;
        throwRange = fishingRod.throwRange;
        price = fishingRod.price;
    }
}