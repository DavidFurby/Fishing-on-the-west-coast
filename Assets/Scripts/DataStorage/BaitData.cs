using System;

[Serializable]
public class BaitData
{
    public string baitName;
    public int level;
    public string description;

    public BaitData(Bait bait)
    {
        baitName = bait.BaitName;
        level = bait.Level;
        description = bait.Description;
    }
}