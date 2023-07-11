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
        game.Scene = gameData.scene;
        game.AvailableFishes = Resources.LoadAll<Fish>("ScriptableObjects/Fishes");
        game.CaughtFishes = gameData.foundCatches.Select(fishData => Fish.SetFish(fishData)).ToList();
        LoadFishingRod(game, gameData);
        LoadBait(game, gameData);
        LoadHats(game, gameData);
    }

    private static void LoadHats(Game game, GameData gameData)
    {
        game.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats");
        game.FoundHats = gameData.foundHats.Select(hatData =>
            Hat.SetHat(hatData)
            ).ToList();
        game.EquippedHat = game.FoundHats.First((hat) => hat.id == gameData.equippedHat.id);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits");
        game.FoundBaits = gameData.foundBaits.Select(baitData => Bait.SetBait(baitData)).ToList();
        game.EquippedBait = game.FoundBaits.First((bait) => bait.id == gameData.equippedBait.id);
    }

    private static void LoadFishingRod(Game game, GameData gameData)
    {
        game.AvailableFishingRods = Resources.LoadAll<FishingRod>(ItemsPath + "FishingRods");
        game.FoundFishingRods = gameData.foundFishingRods.Select((rodData) => FishingRod.SetFishingRod((rodData))).ToList();
        game.EquippedFishingRod = game.FoundFishingRods.First((rod) => rod.id == gameData.equippedFishingRod?.id);
    }
}
