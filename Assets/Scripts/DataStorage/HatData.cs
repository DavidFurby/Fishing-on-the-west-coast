using System;

[Serializable]
public class HatData
{
    public string hatName;
    public string description;
    public int id;
    public int price;

    public HatData(Hat hat)
    {
        id = hat.Id;
        hatName = hat.HatName;
        description = hat.Description;
        price = hat.Price;
    }
}