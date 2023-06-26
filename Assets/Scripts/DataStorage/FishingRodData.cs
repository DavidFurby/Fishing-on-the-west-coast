using System;

[Serializable]
public class FishingRodData
{
    public string fishingRodName;
    public float strength;
    public string throwRange;

    public FishingRodData(FishingRod fishingRod)
    {
        fishingRodName = fishingRod.FishingRodName;
        strength = fishingRod.Strength;
        throwRange = fishingRod.ThrowRange;
    }
}