using System.Linq;
using UnityEngine;

public class RodDisplay : ItemDisplay
{
    private const string ItemsPath = "Objects/ItemResources/FishingRods";

    private void Start()
    {
        if (item == null)
        {
            SelectRod();
        }
    }
    public void SelectRod()
    {
        Debug.Log("Selecting Rod");
        var rodPrefabs = Resources.LoadAll<GameObject>(ItemsPath);
        var equippedRod = rodPrefabs.FirstOrDefault(r => r.GetComponent<RodDisplay>() != null && MainManager.Instance.game.Inventory.EquippedFishingRod.id == r.GetComponent<RodDisplay>().item.id);

        DisplayModel(equippedRod);
        // Get the item component of the instantiated game object    
        item = equippedRod.GetComponent<RodDisplay>().item;
    }
}