using System;

[Serializable]
public class RodData
{
    public int id;
    public string rodName;
    public string description;
    public int reelInSpeed;
    public int throwRange;
    public int price;

    public RodData(Rod rod)
    {
        id = rod.id;
        rodName = rod.itemName;
        description = rod.description;
        reelInSpeed = rod.reelInSpeed;
        throwRange = rod.throwRange;
        price = rod.price;
    }
}