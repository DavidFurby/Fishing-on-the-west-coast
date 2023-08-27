using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Hat", menuName = "ScriptableObjects/Hat", order = 1)]
    public class Hat : Item
{

    public Hat() : base()
    {
        itemTag = ItemTag.Hat;
    }
    public Hat(HatData hatData)
    {
        id = hatData.id;
        name = hatData.name;
        description = hatData.description;
        price = hatData.price;
    }
    public void AddHatToInstance()
    {
        MainManager.Instance.Inventory.FoundHats =
            MainManager.Instance.Inventory.FoundHats.Append(this).ToList();
    }
    public static Hat SetHat(HatData hatData)
    {
        Hat hat = CreateInstance<Hat>();
        hat.id = hatData.id;
        hat.name = hatData.name;
        hat.description = hatData.description;
        hat.price = hatData.price;
        return hat;
    }

}