using System;

[Serializable]
public class FishingRodData
{
    public string fishingRodName;
    public int strength;
    public int throwRange;

    public FishingRodData(FishingRod fishingRod)
    {
        fishingRodName = fishingRod.FishingRodName;
        strength = fishingRod.Strength;
        throwRange = fishingRod.ThrowRange;
    }
}