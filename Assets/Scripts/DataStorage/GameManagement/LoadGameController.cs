using System.Linq;
using UnityEngine;

public class LoadGameController : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items/";

    public static void LoadGame(Game game)
    {
        GameData gameData = SaveSystem.LoadGame();

        game.Days = gameData.daysCount;
        game.TotalCatches = gameData.TotalCatches;
        game.BestDistance = gameData.bestDistance;
        game.Level = gameData.level;
        game.Experience = gameData.experience;
        game.Scene = gameData.scene;
        game.AvailableFishes = Resources.LoadAll<Fish>("ScriptableObjects/Fishes");
        game.CaughtFishes = gameData.foundCatches.Select(fishData => Fish.SetFish(fishData)).ToList();
        LoadFishingRod(game, gameData);
        LoadBait(game, gameData);
        LoadHats(game, gameData);
    }

    private static void LoadHats(Game game, GameData gameData)
    {
        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats");
        game.Inventory.FoundHats = gameData.foundHats.Select(hatData =>
            Hat.SetHat(hatData)
            ).ToList();
        game.Inventory.EquippedHat = game.Inventory.FoundHats.First((hat) => hat.id == gameData.equippedHat.id);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits");
        game.Inventory.FoundBaits = gameData.foundBaits.Select(baitData => Bait.SetBait(baitData)).ToList();
        game.Inventory.EquippedBait = game.Inventory.FoundBaits.First((bait) => bait.id == gameData.equippedBait.id);
    }

    private static void LoadFishingRod(Game game, GameData gameData)
    {
        game.Inventory.AvailableFishingRods = Resources.LoadAll<FishingRod>(ItemsPath + "FishingRods");
        game.Inventory.FoundFishingRods = gameData.foundFishingRods.Select((rodData) => FishingRod.SetFishingRod((rodData))).ToList();
        game.Inventory.EquippedFishingRod = game.Inventory.FoundFishingRods.First((rod) => rod.id == gameData.equippedFishingRod?.id);
    }
}
