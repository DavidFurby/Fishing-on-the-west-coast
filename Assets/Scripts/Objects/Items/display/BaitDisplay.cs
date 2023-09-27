using System.Linq;
using UnityEngine;

public class BaitDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/Baits";
    private Bait current;

    private void OnEnable()
    {
        Inventory.EquipmentChanged += SelectBait;
    }
    private void Start()
    {
        if (item == null)
        {
            SelectBait();
        }
    }
    private void OnDestroy()
    {
        Inventory.EquipmentChanged -= SelectBait;

    }
    public void SelectBait()
    {
        Bait[] baitPrefabs = Resources.LoadAll<Bait>(ItemsPath);
        Bait equippedBait = baitPrefabs.FirstOrDefault(bait => MainManager.Instance.Inventory.EquippedBait.baitId == bait.baitId);
        // Get the item component of the instantiated game object  
        if (current != equippedBait)
        {
            current = equippedBait;
            SetNewItemModel(equippedBait);
        }

    }
}
