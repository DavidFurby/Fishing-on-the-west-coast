using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int TotalCatches { get; set; }
    public float BestDistance { get; set; }
    public Catch[] AvailableCatches { get; set; }
    public List<Catch> Catches { get; set; } = new List<Catch>();
    public List<FishingRod> FoundFishingRods { get; set; } = new List<FishingRod>();
    public FishingRod EquippedFishingRod { get; set; }
    public FishingRod[] AvailableFishingRods { get; set; }
    public List<Bait> FoundBaits { get; set; } = new List<Bait>();
    public Bait EquippedBait { get; set; }
    public Bait[] AvailableBaits { get; set; }
    public List<Hat> FoundHats { get; set; } = new List<Hat>();
    public Hat EquippedHat { get; set; }
    public Hat[] AvailableHats { get; set; }
    public string Scene { get; set; }

    public enum ItemTag
    {
        FishingRod,
        Bait,
        Hat,
        None
    }

    public Item GetEquipment(ItemTag itemTag)
    {
        Item item = null;
        switch (itemTag)
        {
            case ItemTag.FishingRod:
                item = gameObject.AddComponent<Item>();
                break;
            case ItemTag.Bait:
                item = gameObject.AddComponent<Item>();
                break;
            case ItemTag.Hat:
                item = gameObject.AddComponent<Item>();
                break;
        }
        return item;
    }

    public void SetEquipment(int id, ItemTag tag)
    {
        switch (tag)
        {
            case ItemTag.FishingRod:
                EquippedFishingRod = FoundFishingRods.First((rod) => rod.Id == id);
                break;
            case ItemTag.Bait:
                EquippedBait = FoundBaits.First((bait) => bait.Id == id);
                break;
            case ItemTag.Hat:
                EquippedHat = FoundHats.First((hat) => hat.Id == id);
                break;
        }
    }

    //Check if item Exists in inventory
    public bool HasItem(int id)
    {
        return FoundFishingRods.Any((rod) => rod.Id == id) ||
               FoundBaits.Any((bait) => bait.Id == id) ||
               FoundHats.Any((hat) => hat.Id == id);
    }

    public void AddItem(Item item)
    {
        switch (item.ItemTag)
        {
            case ItemTag.FishingRod:
                FishingRod fishingRod = AvailableFishingRods.First((fishingRod) => fishingRod.Id == item.Id);
                break;
            case ItemTag.Bait:
                Bait bait = AvailableBaits.First((bait) => bait.Id == item.Id);
                bait.AddBaitToInstance();
                break;
            case ItemTag.Hat:
                Hat hat = AvailableHats.First((hat) => hat.Id == item.Id);
                hat.AddHatToInstance();
                break;
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        LoadGameController.LoadGame(this);
    }

    public void NewGame()
    {
        NewGameController.InitializeNewGame(this);
    }
}
