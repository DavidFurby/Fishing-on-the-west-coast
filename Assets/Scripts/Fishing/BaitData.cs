using System;

[Serializable]
public class BaitData
{
    public string baitName;
    public int level;

    public BaitData(Bait baitData)
    {
        baitName = baitData.BaitName;
        level = baitData.Level;
    }
}