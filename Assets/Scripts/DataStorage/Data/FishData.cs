using System;

[Serializable]
public class FishData
{
    public string name;
    public float size;
    public string info;
    public int level;

    public FishData(Catch fish)
    {
        name = fish.Name;
        size = fish.Size;
        info = fish.Description;
        level = fish.Level;
    }
}