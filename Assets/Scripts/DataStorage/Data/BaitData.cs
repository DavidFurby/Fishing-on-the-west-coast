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
        id = bait.Id;
        baitName = bait.BaitName;
        level = bait.Level;
        description = bait.Description;
        price = bait.Price;
    }
}