using System;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Rod", menuName = "ScriptableObjects/Rod", order = 1)]
public class Rod : Item
{
    public int rodId;
    public int reelInSpeed;
    public int throwRange;

    public void AddRodToInstance()
    {
        MainManager.Instance.Inventory.FoundRods = MainManager.Instance.Inventory.FoundRods.Append(this).ToList();
    }
}
