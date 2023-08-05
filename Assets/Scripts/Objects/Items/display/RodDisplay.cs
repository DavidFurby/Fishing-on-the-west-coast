using System.Linq;
using UnityEngine;

public class RodDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/FishingRods";

    private void Start()
    {
        if (item == null)
        {
            SelectRod();
        }
    }
    public void SelectRod()
    {
        FishingRod[] rodPrefabs = Resources.LoadAll<FishingRod>(ItemsPath);
        FishingRod equippedRod = rodPrefabs.FirstOrDefault(r => MainManager.Instance.game.Inventory.EquippedFishingRod.id == r.id);
        // Get the item component of the instantiated game object  
        SetNewItemModel(equippedRod);
        Debug.Log(item);

    }
}
