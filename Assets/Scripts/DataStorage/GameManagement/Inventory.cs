using System;
using System.Collections.Generic;
using System.Linq;
using static Item;

public class Inventory
{
    public List<Rod> FoundRods { get; set; } = new List<Rod>();
    public Rod EquippedRod { get; set; }
    public Rod[] AvailableRods { get; set; }
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
            case ItemTag.Rod:
                EquippedRod = FoundRods.First((rod) => rod.id == id);
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
   public Item GetEquippedItem(ItemTag tag)
{
        return tag switch
        {
            ItemTag.Rod => EquippedRod,
            ItemTag.Bait => EquippedBait,
            ItemTag.Hat => EquippedHat,
            _ => null,
        };
    }

   public Item[] GetEquipment(ItemTag tag)
{
        return tag switch
        {
            ItemTag.Rod => FoundRods.ToArray(),
            ItemTag.Bait => FoundBaits.ToArray(),
            ItemTag.Hat => FoundHats.ToArray(),
            _ => null,
        };
    }
    //Check if item Exists in inventory
    public bool HasItem(Item item)
    {
        
        return item.itemTag switch
        {
            ItemTag.Rod => FoundRods.Any((rod) => rod.id == item.id),
            ItemTag.Bait => FoundBaits.Any((bait) => bait.id == item.id),
            ItemTag.Hat => FoundHats.Any((hat) => hat.id == item.id),
            _ => false,
        };
    }
    public void AddItem(Item item)
    {
        switch (item.itemTag)
        {
            case ItemTag.Rod:
                Rod rod = AvailableRods.FirstOrDefault((rod) => rod.id == item.id);
                rod.AddRodToInstance();
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
