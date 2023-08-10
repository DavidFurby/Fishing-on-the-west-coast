using System.Linq;
using UnityEngine;

public class BaitDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/Baits";

    private void Start()
    {
        if (item == null)
        {
            SelectBait();
        }
    }
    public void SelectBait()
    {
        Bait[] baitPrefabs = Resources.LoadAll<Bait>(ItemsPath);
        Bait equippedBait = baitPrefabs.FirstOrDefault(r => MainManager.Instance.game.Inventory.EquippedBait.id == r.id);
        // Get the item component of the instantiated game object  
        SetNewItemModel(equippedBait);
    }
}
