using System.Linq;
using UnityEngine;

public class HatDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/Hats";

    private Hat current;

    private void OnEnable()
    {
        Inventory.EquipmentChanged += SelectHat;
    }
    private void Start()
    {
        if (item == null)
        {
            SelectHat();
        }
    }
    private void OnDestroy()
    {
        Inventory.EquipmentChanged -= SelectHat;

    }
    public void SelectHat()
    {
        Hat[] hatPrefabs = Resources.LoadAll<Hat>(ItemsPath);

        Hat equippedHat = hatPrefabs.FirstOrDefault(r => MainManager.Instance.Inventory.EquippedHat.id == r.id);
        // Get the item component of the instantiated game object  
        if (current != equippedHat)
        {

            current = equippedHat;

            SetNewItemModel(equippedHat);
        }
    }
}
