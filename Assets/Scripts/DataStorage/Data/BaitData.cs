using System;

[Serializable]
public class BaitData
{
    public int id;
    public string baitName;
    public int level;
    public string description;
    public int price;

    public BaitData(Bait bait)
    {
        id = bait.id;
        baitName = bait.itemName;
        level = bait.level;
        description = bait.description;
        price = bait.price;
    }
}