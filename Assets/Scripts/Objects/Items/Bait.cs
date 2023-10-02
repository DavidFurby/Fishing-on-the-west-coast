using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Bait", menuName = "ScriptableObjects/Bait", order = 1)]
public class Bait : Item
{
    public string baitId;

    public string BaitId => baitId;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(baitId))
        {
            baitId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    public int level;

    public void AddBaitToInstance()
    {
        MainManager.Instance.Inventory.FoundBaits =
            MainManager.Instance.Inventory.FoundBaits.Append(this).ToList();
    }
}
