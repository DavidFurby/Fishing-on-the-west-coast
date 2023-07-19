using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Item;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int TotalCatches { get; set; }
    public float BestDistance { get; set; }
    public Fish[] AvailableFishes { get; set; }
    public List<Fish> CaughtFishes { get; set; } = new List<Fish>();
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
    public Item GetEquipment(ItemTag itemTag)
    {
        switch (itemTag)
        {
            case ItemTag.FishingRod:
                return EquippedFishingRod;
            case ItemTag.Bait:
                return EquippedBait;
            case ItemTag.Hat:
                return EquippedHat;
            default: return null;
        }

    }

    //Check if item Exists in inventory
    public bool HasItem(Item item)
    {

        switch (item.itemTag)
        {
            case ItemTag.FishingRod:
                return FoundFishingRods.Any((rod) => rod.id == item.id);
            case ItemTag.Bait:
                return FoundBaits.Any((bait) => bait.id == item.id);
            case ItemTag.Hat:
                return FoundHats.Any((hat) => hat.id == item.id);
            default:
                return false;
        }
    }


    public void AddItem(Item item)
    {
        switch (item.itemTag)
        {
            case ItemTag.FishingRod:
                Debug.Log(AvailableFishingRods.Length);
                FishingRod fishingRod = AvailableFishingRods.FirstOrDefault((fishingRod) => fishingRod.id == item.id);
                fishingRod.AddFishingRodToInstance();
                break;
            case ItemTag.Bait:
                Bait bait = AvailableBaits.FirstOrDefault((bait) => bait.id == item.id);
                bait.AddBaitToInstance();
                break;
            case ItemTag.Hat:
                Hat hat = AvailableHats.FirstOrDefault((hat) => hat.id == item.id);
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
