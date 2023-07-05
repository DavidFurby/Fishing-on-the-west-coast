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
        game.AvailableHats = Hat.SetAvailableHats(game);
        game.FoundHats = gameData.foundHats.Select(hatData =>
            Hat.SetHat(game, hatData)
            ).ToList();
        game.EquippedHat = Hat.SetHat(game, gameData.equippedHat);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.AvailableBaits = Bait.SetAvailableBaits(game);
        game.FoundBaits = gameData.foundBaits.Select(baitData =>
            Bait.SetBait(game,
                baitData
               )
            ).ToList();
        game.EquippedBait = Bait.SetBait(game, gameData.equippedBait);
    }

    private static void LoadFishingRod(Game game, GameData gameData)
    {
        game.AvailableFishingRods = FishingRod.SetAvailableRods(game);
        game.FoundFishingRods = gameData.foundFishingRods.Select(rodData =>
            FishingRod.SetFishingRod(game, rodData)).ToList();
        game.EquippedFishingRod = FishingRod.SetFishingRod(game, gameData.equippedFishingRod);
    }
}
