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
        game.Inventory.AvailableFishingRods = Resources.LoadAll<FishingRod>(ItemsPath + "FishingRods").OrderBy(r => r.id).ToArray();
        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats").OrderBy(h => h.id).ToArray();
        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits").OrderBy(b => b.id).ToArray();
    }

    // Resets the game state to its initial values
    private static void ResetGameState(Game game)
    {
        game.Days = 1;
        game.TotalCatches = 0;
        game.BestDistance = 0;
        game.Scene = "Boat";
        game.Level = 0;
        game.Experience = 0;
        game.CaughtFishes.Clear();
        game.Inventory.FoundFishingRods.Clear();
        game.Inventory.FoundFishingRods.Add(game.Inventory.AvailableFishingRods[0]);
        game.Inventory.EquippedFishingRod = game.Inventory.FoundFishingRods.First((rod) => rod.id == 1);
        game.Inventory.FoundBaits.Clear();
        game.Inventory.FoundBaits.Add(game.Inventory.AvailableBaits[0]);
        game.Inventory.EquippedBait = game.Inventory.FoundBaits.First((bait) => bait.id == 1);
        game.Inventory.FoundHats.Clear();
        game.Inventory.FoundHats.Add(game.Inventory.AvailableHats[0]);
        game.Inventory.EquippedHat = game.Inventory.FoundHats.First((hat) => hat.id == 1);
    }
}
