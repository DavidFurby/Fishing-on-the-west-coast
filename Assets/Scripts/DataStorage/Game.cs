using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int TotalCatches { get; set; }

    public float BestDistance { get; set; }
    public Catch[] Catches { get; set; } = new Catch[0];
    public FishingRod[] FoundFishingRods { get; set; } = new FishingRod[0];
    public FishingRod EquippedFishingRod { get; set; }

    public Bait[] FoundBaits { get; set; } = new Bait[0];

    public Bait EquippedBait { get; set; }

    public Hat[] FoundHats { get; set; } = new Hat[0];

    public Hat EquippedHat { get; set; }
    public string Scene { get; set; }

    public enum ItemTag
    {
        FishingRod,
        Bait,
        Hat
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
                var fishingRod = FishingRod.CreateFishingRod(item.Id, item.Name, item.Price, item.Description);
                fishingRod.AddFishingRodToInstance();
                break;
            case ItemTag.Bait:
                var bait = Bait.CreateBait(item.Id, item.Name, item.Price, item.Description);
                bait.AddBaitToInstance();
                break;
            case ItemTag.Hat:
                var hat = new Hat(item.Id, item.Name, item.Price, item.Description);
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
        GameData gameData = SaveSystem.LoadGame();

        Days = gameData.daysCount;
        TotalCatches = gameData.TotalCatches;
        BestDistance = gameData.bestDistance;
        Scene = gameData.scene;
        Catches = gameData.foundCatches.Select(fishData => gameObject.AddComponent<Catch>()).ToArray();
        LoadFishingRod(gameData);
        LoadBait(gameData);
        LoadHats(gameData);
    }

    private void LoadHats(GameData gameData)
    {
        FoundHats = gameData.foundHats.Select(hatData =>
         Hat.SetHat(this, hatData.id, hatData.hatName, hatData.description, hatData.price)
           ).ToArray();
        EquippedHat = Hat.SetHat(this, gameData.equippedHat.id, gameData.equippedHat.hatName, gameData.equippedHat.description, gameData.equippedHat.price);
    }

    private void LoadBait(GameData gameData)
    {
        FoundBaits = gameData.foundBaits.Select(baitData =>
        Bait.SetBait(this, baitData.id, baitData.baitName, baitData.description, baitData.price)
        ).ToArray();
        EquippedBait = Bait.SetBait(this, gameData.equippedBait.id, gameData.equippedBait.baitName, gameData.equippedBait.description, gameData.equippedBait.price);

    }

    private void LoadFishingRod(GameData gameData)
    {
        FoundFishingRods = gameData.foundFishingRods.Select(fishingRodData =>
        FishingRod.SetFishingRod(this, fishingRodData.id, fishingRodData.fishingRodName, fishingRodData.description, fishingRodData.price)).ToArray();
        EquippedFishingRod = FishingRod.SetFishingRod(this, gameData.equippedFishingRod.id, gameData.equippedFishingRod.fishingRodName, gameData.equippedFishingRod.description, gameData.equippedFishingRod.price);

    }

    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            Days = 1;
            TotalCatches = 0;
            BestDistance = 0;
            Scene = "Boat";
            Catches = new Catch[0];
            FoundFishingRods = new FishingRod[] { gameObject.AddComponent<FishingRod>() };
            EquippedFishingRod = FoundFishingRods[0];
            FoundBaits = new Bait[] { gameObject.AddComponent<Bait>() };
            EquippedBait = FoundBaits[0];
            FoundHats = new Hat[] { gameObject.AddComponent<Hat>() };
            EquippedHat = FoundHats[0];
        }
    }
}