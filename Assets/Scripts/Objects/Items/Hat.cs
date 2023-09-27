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
    public void AddHatToInstance()
    {
        MainManager.Instance.Inventory.FoundHats =
            MainManager.Instance.Inventory.FoundHats.Append(this).ToList();
    }

}