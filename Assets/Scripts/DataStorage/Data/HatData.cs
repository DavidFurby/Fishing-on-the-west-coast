using System;

[Serializable]
public class HatData
{
    public string name;
    public string description;
    public int id;
    public int price;

    public HatData(Hat hat)
    {
        id = hat.id;
        name = hat.itemName;
        description = hat.description;
        price = hat.price;
    }
}