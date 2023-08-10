using System.Linq;
using UnityEngine;

public class HatDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/Hats";

    private void Start()
    {
        if (item == null)
        {
            SelectHat();
        }
    }
    public void SelectHat()
    {
        Hat[] hatPrefabs = Resources.LoadAll<Hat>(ItemsPath);
        Hat equippedHat = hatPrefabs.FirstOrDefault(r => MainManager.Instance.game.Inventory.EquippedHat.id == r.id);
        // Get the item component of the instantiated game object  
        SetNewItemModel(equippedHat);
    }
}
