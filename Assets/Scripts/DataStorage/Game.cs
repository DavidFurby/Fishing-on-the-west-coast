using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int Fishes { get; set; }
    public Fish[] Catches { get; set; } = new Fish[0];
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
    public Equipment GetEquipment(ItemTag itemTag)
    {
        Equipment equipment = null;
        switch (itemTag)
        {
            case ItemTag.FishingRod:
                equipment = new Equipment(EquippedFishingRod.Id, EquippedFishingRod.FishingRodName, EquippedFishingRod.Description);
                break;
            case ItemTag.Bait:
                equipment = new Equipment(EquippedBait.Id, EquippedBait.BaitName, EquippedBait.Description);
                break;
            case ItemTag.Hat:
                equipment = new Equipment(EquippedHat.Id, EquippedHat.HatName, EquippedHat.Description);
                break;
        }
        return equipment;
    }
    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGame();

        Days = gameData.daysCount;
        Fishes = gameData.catchCount;
        Scene = gameData.scene;
        Catches = gameData.foundCatches.Select(fishData => gameObject.AddComponent<Fish>()).ToArray();

        FoundFishingRods = gameData.foundFishingRods.Select(fishingRodData => gameObject.AddComponent<FishingRod>()).ToArray();
        EquippedFishingRod = gameObject.AddComponent<FishingRod>();
        EquippedFishingRod.FishingRodName = gameData.equippedFishingRod.fishingRodName;
        EquippedFishingRod.Strength = gameData.equippedFishingRod.strength;
        EquippedFishingRod.ThrowRange = gameData.equippedFishingRod.throwRange;

        FoundBaits = gameData.foundBaits.Select(baitData => gameObject.AddComponent<Bait>()).ToArray();
        EquippedBait = gameObject.AddComponent<Bait>();
        EquippedBait.BaitName = gameData.equippedFishingRod.fishingRodName;
        EquippedBait.Level = gameData.equippedBait.level;
        EquippedBait.Description = gameData.equippedBait.description;

        FoundHats = gameData.foundHats.Select(hatData => gameObject.AddComponent<Hat>()).ToArray();
        EquippedHat = gameObject.AddComponent<Hat>();
        EquippedHat.HatName = gameData.equippedHat.hatName;
        EquippedHat.Description = gameData.equippedHat.description;
    }

    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            Days = 1;
            Fishes = 0;
            Scene = "Boat";
            Catches = new Fish[0];
            FoundFishingRods = new FishingRod[] { gameObject.AddComponent<FishingRod>() };
            EquippedFishingRod = FoundFishingRods[0];
            FoundBaits = new Bait[] { gameObject.AddComponent<Bait>() };
            EquippedBait = FoundBaits[0];
            FoundHats = new Hat[] { gameObject.AddComponent<Hat>() };
            EquippedHat = FoundHats[0];
        }
    }
    public class Equipment
    {
        public int id;
        public string name;
        public string description;

        public Equipment(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
    }
}