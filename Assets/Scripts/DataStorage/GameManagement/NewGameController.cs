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
        game.Inventory.AvailableRods = Resources.LoadAll<Rod>(ItemsPath + "Rods").OrderBy(r => r.id).ToArray();
        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats").OrderBy(h => h.id).ToArray();
        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits").OrderBy(b => b.id).ToArray();
        Debug.Log(game.Inventory.AvailableRods[0].name);
    }

    // Resets the game state to its initial values
    private static void ResetGameState(Game game)
    {
        game.Days = 1;
        game.TotalCatches = 0;
        game.BestDistance = 0;
        game.Scene = "Boat";
        game.PlayerLevel.SetPlayerLevel(1, 0);
        game.CaughtFishes.Clear();
        game.Inventory.FoundRods.Clear();
        game.Inventory.FoundRods.Add(game.Inventory.AvailableRods[0]);
        game.Inventory.EquippedRod = game.Inventory.FoundRods[0];
        game.Inventory.FoundBaits.Clear();
        game.Inventory.FoundBaits.Add(game.Inventory.AvailableBaits[0]);
        game.Inventory.EquippedBait = game.Inventory.FoundBaits[0];
        game.Inventory.FoundHats.Clear();
        game.Inventory.FoundHats.Add(game.Inventory.AvailableHats[0]);
        game.Inventory.EquippedHat = game.Inventory.FoundHats[0];
    }
}
