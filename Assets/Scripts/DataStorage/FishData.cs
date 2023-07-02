using System;

[Serializable]
public class FishData
{
    public string fishName;
    public float size;
    public string info;
    public int level;

    public FishData(Catch fish)
    {
        fishName = fish.CatchName;
        size = fish.Size;
        info = fish.Description;
        level = fish.Level;
    }
}