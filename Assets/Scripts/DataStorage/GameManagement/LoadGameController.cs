using System.Linq;
using UnityEngine;

public class LoadGameController : MonoBehaviour
{
    public static void LoadGame(Game game)
    {
        GameData gameData = SaveSystem.LoadGame();

        game.Days = gameData.daysCount;
        game.TotalCatches = gameData.TotalCatches;
        game.BestDistance = gameData.bestDistance;
        game.Scene = gameData.scene;
        game.AvailableCatches = Catch.SetAvailableCatches(game);
        game.Catches = gameData.foundCatches.Select(fishData => game.gameObject.AddComponent<Catch>()).ToList();
        LoadFishingRod(game, gameData);
        LoadBait(game, gameData);
        LoadHats(game, gameData);
    }

    private static void LoadHats(Game game, GameData gameData)
    {
        game.AvailableHats = Resources.LoadAll<Hat>("Hats");
        game.FoundHats = gameData.foundHats.Select(hatData =>
            Hat.SetHat(hatData)
            ).ToList();
        game.EquippedHat = game.FoundHats.First((hat) => hat.id == gameData.equippedHat.id);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.AvailableBaits = Resources.LoadAll<Bait>("Baits");
        game.FoundBaits = gameData.foundBaits.Select(baitData => Bait.SetBait(baitData)).ToList();
        game.EquippedBait = game.FoundBaits.First((bait) => bait.id == gameData.equippedBait.id);
    }

    private static void LoadFishingRod(Game game, GameData gameData)
    {
        game.AvailableFishingRods = Resources.LoadAll<FishingRod>("FishingRods");
        game.FoundFishingRods = gameData.foundFishingRods.Select((rodData) => FishingRod.SetFishingRod((rodData))).ToList();
        game.EquippedFishingRod = game.FoundFishingRods.First((rod) => rod.id == gameData.equippedFishingRod?.id);
    }
}
