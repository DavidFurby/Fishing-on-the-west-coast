using System;

[Serializable]
public class RodData
{
    public int id;
    public string name;
    public string description;
    public int strength;
    public int throwRange;
    public int price;

    public RodData(Rod rod)
    {
        id = rod.id;
        name = rod.itemName;
        description = rod.description;
        strength = rod.reelInSpeed;
        throwRange = rod.throwRange;
        price = rod.price;
    }
}