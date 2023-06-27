using System;

[Serializable]
public class FishData
{
    public string fishName;
    public float size;
    public string info;
    public int level;

    public FishData(Fish fish)
    {
        fishName = fish.FishName;
        size = fish.Size;
        info = fish.Info;
        level = fish.Level;
    }
}