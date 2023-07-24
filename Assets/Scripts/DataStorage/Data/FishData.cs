using System;

[Serializable]
public class FishData
{
    public int id;
    public string name;
    public float size;
    public string description;
    public int level;

    public int experience;

    public FishData(Fish fish)
    {
        id = fish.id;
        name = fish.name;
        size = fish.size;
        description = fish.description;
        level = fish.level;
        experience = fish.experience;
    }
}