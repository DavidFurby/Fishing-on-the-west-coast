using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Bait", menuName = "ScriptableObjects/Bait", order = 1)]
public class Bait : Item
{
    public int level;

    public Bait() : base()
    {
        itemTag = ItemTag.Bait;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.Inventory.FoundBaits =
            MainManager.Instance.Inventory.FoundBaits.Append(this).ToList();
    }
}
