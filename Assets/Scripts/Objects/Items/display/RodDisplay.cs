using System.Linq;
using UnityEngine;

public class RodDisplay : ItemDisplay
{
    private const string ItemsPath = "Objects/ItemResources/FishingRods";
    private GameObject[] rods;
    private void Start()
    {
        SelectRod();
    }

    public void SelectRod()
    {
        rods = Resources.LoadAll<GameObject>(ItemsPath);
        item = MainManager.Instance.game.Inventory.EquippedFishingRod;

        for (int i = 0; i < rods.Length; i++)
        {
            if (rods[i].GetComponent<RodDisplay>() != null && item.id == rods[i].GetComponent<RodDisplay>().item.id)
            {
                DisplayModel(rods[i]); // Call the DisplayModel method from the base class

                break;
            }
        }
    }
}
