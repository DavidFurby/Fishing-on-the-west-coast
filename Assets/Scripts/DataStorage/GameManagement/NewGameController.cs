using System.Linq;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
    private const string ItemsPath = "ScriptableObjects/Items/";

    public static void InitializeNewGame(Game game)
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            game.AvailableFishes = Resources.LoadAll<Fish>("ScriptableObjects/Fishes").OrderBy(c => c.fishId).ToArray();
            PopulateAvailableItems(game);
            ResetGameState(game);
        }
    }

    private static void PopulateAvailableItems(Game game)
    {

        game.Inventory.AvailableRods = Resources.LoadAll<Rod>(ItemsPath + "Rods");

        game.Inventory.AvailableRods = game.Inventory.AvailableRods;

        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats");

        game.Inventory.AvailableHats = game.Inventory.AvailableHats;

        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits");

        game.Inventory.AvailableBaits = game.Inventory.AvailableBaits;
    }

    private static void ResetGameState(Game game)
    {
        game.Days = 1;
        game.TotalCatches = 0;
        game.BestDistance = 0;
        game.Scene = "Sandbox";
        game.PlayerLevel.SetPlayerLevel(1, 0);
        game.CaughtFishes.Clear();
        game.Inventory.FoundRods.Clear();
        game.Inventory.FoundRods.Add(game.Inventory.AvailableRods.First((Rod rod) => rod.itemName == "Basic rod"));
        game.Inventory.EquippedRod = game.Inventory.FoundRods[0];
        game.Inventory.FoundBaits.Clear();
        game.Inventory.FoundBaits.Add(game.Inventory.AvailableBaits.First((Bait bait) => bait.itemName == "Basic bait"));
        game.Inventory.EquippedBait = game.Inventory.FoundBaits[0];
        game.Inventory.FoundHats.Clear();
        game.Inventory.FoundHats.Add(game.Inventory.AvailableHats.First((Hat hat) => hat.itemName == "Basic hat"));
        game.Inventory.EquippedHat = game.Inventory.FoundHats[0];
    }
}
