using System.Linq;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items/";

    // Initializes a new game by resetting the game state and populating the available items
    public static void InitializeNewGame(Game game)
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            game.AvailableFishes = Resources.LoadAll<Fish>("ScriptableObjects/Fishes").OrderBy(c => c.id).ToArray();
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
        game.AvailableFishingRods = Resources.LoadAll<FishingRod>(ItemsPath + "FishingRods").OrderBy(r => r.id).ToArray();
        game.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats").OrderBy(h => h.id).ToArray();
        game.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits").OrderBy(b => b.id).ToArray();
    }

    // Resets the game state to its initial values
    private static void ResetGameState(Game game)
    {
        game.Days = 1;
        game.TotalCatches = 0;
        game.BestDistance = 0;
        game.Scene = "Boat";
        game.CaughtFishes.Clear();
        game.FoundFishingRods.Clear();
        game.FoundFishingRods.Add(game.AvailableFishingRods[0]);
        game.EquippedFishingRod = game.FoundFishingRods.First((rod) => rod.id == 1);
        game.FoundBaits.Clear();
        game.FoundBaits.Add(game.AvailableBaits[0]);
        game.EquippedBait = game.FoundBaits.First((bait) => bait.id == 1);
        game.FoundHats.Clear();
        game.FoundHats.Add(game.AvailableHats[0]);
        game.EquippedHat = game.FoundHats.First((hat) => hat.id == 1);
    }
}
