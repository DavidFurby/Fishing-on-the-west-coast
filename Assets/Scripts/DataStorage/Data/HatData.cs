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
        id = hat.Id;
        name = hat.Name;
        description = hat.Description;
        price = hat.Price;
    }
}