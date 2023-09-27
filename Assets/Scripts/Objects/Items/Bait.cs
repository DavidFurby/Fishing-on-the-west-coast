using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Bait", menuName = "ScriptableObjects/Bait", order = 1)]
public class Bait : Item
{
    public int baitId;
    public int level;

    public void AddBaitToInstance()
    {
        MainManager.Instance.Inventory.FoundBaits =
            MainManager.Instance.Inventory.FoundBaits.Append(this).ToList();
    }
}
