using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Rod", menuName = "ScriptableObjects/Rod", order = 1)]
public class Rod : Item
{
    public int reelInSpeed;
    public int throwRange;



    public Rod() : base()
    {
        itemTag = ItemTag.Rod;
    }

    public void AddRodToInstance()
    {
        MainManager.Instance.Inventory.FoundRods = MainManager.Instance.Inventory.FoundRods.Append(this).ToList();
    }
    public static Rod SetRod(RodData rodData)
    {
        Rod rod = CreateInstance<Rod>();
        rod.id = rodData.id;
        rod.name = rodData.name;
        rod.description = rodData.description;
        rod.price = rodData.price;
        rod.reelInSpeed = rodData.strength;
        rod.throwRange = rodData.throwRange;
        return rod;
    }
}