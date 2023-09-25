using System.Linq;
using UnityEngine;

public class RodDisplay : ItemDisplay
{
    private const string ItemsPath = "ScriptableObjects/Items/Rods";
    private Rod current;
    private void OnEnable()
    {
        Inventory.EquipmentChanged += SelectRod;
    }
    private void Start()
    {
        if (item == null)
        {
            SelectRod();
        }
    }
    private void OnDestroy()
    {
        Inventory.EquipmentChanged -= SelectRod;
    }
    public void SelectRod()
    {
        Rod[] rodPrefabs = Resources.LoadAll<Rod>(ItemsPath);
        Rod equippedRod = rodPrefabs.FirstOrDefault(r => MainManager.Instance.Inventory.EquippedRod.id == r.id);
        // Get the item component of the instantiated game object  
        if (current != equippedRod)
        {
            current = equippedRod;
            SetNewItemModel(equippedRod);
        }


    }
}
