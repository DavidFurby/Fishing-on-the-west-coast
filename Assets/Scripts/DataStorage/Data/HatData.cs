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
        id = hat.id;
        hatName = hat.itemName;
        description = hat.description;
        price = hat.price;
    }
}