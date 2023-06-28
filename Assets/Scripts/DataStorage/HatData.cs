using System;

[Serializable]
public class HatData
{
    public string hatName;
    public string description;

    public HatData(Hat hat)
    {
        hatName = hat.HatName;
        description = hat.Description;
    }
}