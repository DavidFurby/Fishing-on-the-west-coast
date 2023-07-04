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
        {
            Hat hat = gameObject.AddComponent<Hat>();
            hat.HatName = hatData.hatName;
            hat.Description = hatData.description;
            hat.Price = hatData.price;
            return hat;
        }).ToArray();
        EquippedHat = gameObject.AddComponent<Hat>();
        EquippedHat.HatName = gameData.equippedHat.hatName;
        EquippedHat.Description = gameData.equippedHat.description;
        EquippedBait.Price = gameData.equippedBait.price;
    }

    private void LoadBait(GameData gameData)
    {
        FoundBaits = gameData.foundBaits.Select(baitData =>
        {
            Bait bait = gameObject.AddComponent<Bait>();
            bait.BaitName = baitData.baitName;
            bait.Level = baitData.level;
            bait.Description = baitData.description;
            bait.Price = baitData.price;
            return bait;
        }).ToArray();
        EquippedBait = gameObject.AddComponent<Bait>();
        EquippedBait.BaitName = gameData.equippedBait.baitName;
        EquippedBait.Level = gameData.equippedBait.level;
        EquippedBait.Description = gameData.equippedBait.description;
        EquippedBait.Price = gameData.equippedBait.price;
    }

    private void LoadFishingRod(GameData gameData)
    {
        FoundFishingRods = gameData.foundFishingRods.Select(fishingRodData =>
        {
            FishingRod fishingRod = gameObject.AddComponent<FishingRod>();
            fishingRod.FishingRodName = fishingRodData.fishingRodName;
            fishingRod.Strength = fishingRodData.strength;
            fishingRod.ThrowRange = fishingRodData.throwRange;
            fishingRod.Price = fishingRodData.price;
            return fishingRod;
        }).ToArray();
        EquippedFishingRod = gameObject.AddComponent<FishingRod>();
        EquippedFishingRod.FishingRodName = gameData.equippedFishingRod.fishingRodName;
        EquippedFishingRod.Strength = gameData.equippedFishingRod.strength;
        EquippedFishingRod.ThrowRange = gameData.equippedFishingRod.throwRange;
        EquippedFishingRod.Price = gameData.equippedFishingRod.price;
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