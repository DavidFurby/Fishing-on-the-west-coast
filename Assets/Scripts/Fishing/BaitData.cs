using System;

[Serializable]
public class BaitData
{
    public string baitName;
    public int level;

    public BaitData(Bait baitData)
    {
        baitName = baitData.baitName;
        level = baitData.level;
    }
}