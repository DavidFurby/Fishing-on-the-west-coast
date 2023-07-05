using UnityEngine;

public class NewGameController : MonoBehaviour
{
    // Initializes a new game by resetting the game state and populating the available items
    public static void InitializeNewGame(Game game)
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            game.AvailableCatches = Catch.SetAvailableCatches(game);
            PopulateAvailableItems(game);
            ResetGameState(game);
        }
        else
        {
            // Handle error case when SaveSystem.NewGame() returns false
        }
    }

    // Populates the available items for the game
    private static void PopulateAvailableItems(Game game)
    {
        game.AvailableFishingRods = FishingRod.SetAvailableFishingRods(game);
        game.AvailableHats = Hat.SetAvailableHats(game);
        game.AvailableBaits = Bait.SetAvailableBaits(game);
    }


    // Resets the game state to its initial values
    private static void ResetGameState(Game game)
    {
        game.Days = 1;
        game.TotalCatches = 0;
        game.BestDistance = 0;
        game.Scene = "Boat";
        game.Catches.Clear();
        game.FoundFishingRods.Clear();
        game.FoundFishingRods.Add(game.AvailableFishingRods[0]);
        game.EquippedFishingRod = game.FoundFishingRods[0];
        game.FoundBaits.Clear();
        game.FoundBaits.Add(game.AvailableBaits[0]);
        game.EquippedBait = game.FoundBaits[0];
        game.FoundHats.Clear();
        game.FoundHats.Add(game.AvailableHats[0]);
        game.EquippedHat = game.FoundHats[0];
    }
}
