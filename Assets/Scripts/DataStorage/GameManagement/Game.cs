using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Item;

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

    public void SetEquipment(int id, ItemTag tag)
    {
        switch (tag)
        {
            case ItemTag.FishingRod:
                EquippedFishingRod = FoundFishingRods.First((rod) => rod.id == id);
                break;
            case ItemTag.Bait:
                EquippedBait = FoundBaits.First((bait) => bait.id == id);
                break;
            case ItemTag.Hat:
                EquippedHat = FoundHats.First((hat) => hat.id == id);
                break;
        }
    }

    //Check if item Exists in inventory
    public bool HasItem(int id, ItemTag itemTag)
    {

        Debug.Log(id + itemTag);
        return itemTag switch
        {
            ItemTag.FishingRod => FoundFishingRods.Any((rod) => rod.id == id),
            ItemTag.Bait => FoundBaits.Any((bait) => bait.id == id),
            ItemTag.Hat => FoundHats.Any((hat) => hat.id == id),
            ItemTag.None => false,
            _ => false,
        };
    }

    public void AddItem(Item item)
    {
        switch (item.itemTag)
        {
            case ItemTag.FishingRod:
                FishingRod fishingRod = AvailableFishingRods.First((fishingRod) => fishingRod.id == item.id);
                break;
            case ItemTag.Bait:
                Bait bait = AvailableBaits.First((bait) => bait.id == item.id);
                bait.AddBaitToInstance();
                break;
            case ItemTag.Hat:
                Hat hat = AvailableHats.First((hat) => hat.id == item.id);
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
