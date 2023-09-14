using System;

[Serializable]
public class BaitData
{
    public int id;
    public string name;
    public int level;
    public string description;
    public int price;

    public BaitData(Bait bait)
    {
        id = bait.id;
        name = bait.itemName;
        level = bait.level;
        description = bait.description;
        price = bait.price;
    }
}