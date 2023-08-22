using System;
using System.Collections.Generic;
using System.Linq;
using static Item;

public class Inventory
{
    public List<FishingRod> FoundFishingRods { get; set; } = new List<FishingRod>();
    public FishingRod EquippedFishingRod { get; set; }
    public FishingRod[] AvailableFishingRods { get; set; }
    public List<Bait> FoundBaits { get; set; } = new List<Bait>();
    public Bait EquippedBait { get; set; }
    public Bait[] AvailableBaits { get; set; }
    public List<Hat> FoundHats { get; set; } = new List<Hat>();
    public Hat EquippedHat { get; set; }
    public Hat[] AvailableHats { get; set; }

    public static event Action EquipmentChanged;


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
        EquipmentChanged?.Invoke();
    }
    public Item GetEquipment(ItemTag itemTag)
    {
        return itemTag switch
        {
            ItemTag.FishingRod => EquippedFishingRod,
            ItemTag.Bait => EquippedBait,
            ItemTag.Hat => EquippedHat,
            _ => null,
        };
    }
    //Check if item Exists in inventory
    public bool HasItem(Item item)
    {
        return item.itemTag switch
        {
            ItemTag.FishingRod => FoundFishingRods.Any((rod) => rod.id == item.id),
            ItemTag.Bait => FoundBaits.Any((bait) => bait.id == item.id),
            ItemTag.Hat => FoundHats.Any((hat) => hat.id == item.id),
            _ => false,
        };
    }
    public void AddItem(Item item)
    {
        switch (item.itemTag)
        {
            case ItemTag.FishingRod:
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
}
