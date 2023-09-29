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
            game.AvailableFishes = Resources.LoadAll<Fish>("ScriptableObjects/Fishes").OrderBy(c => c.id).ToArray();
            PopulateAvailableItems(game);
            ResetGameState(game);
        }
    }

    private static void PopulateAvailableItems(Game game)
    {
        int currentId = 1;

        game.Inventory.AvailableRods = Resources.LoadAll<Rod>(ItemsPath + "Rods");
        foreach (var rod in game.Inventory.AvailableRods)
        {
            rod.id = currentId;
            currentId++;
        }
        game.Inventory.AvailableRods = game.Inventory.AvailableRods.OrderBy(r => r.rodId).ToArray();

        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats");
        foreach (var hat in game.Inventory.AvailableHats)
        {
            hat.id = currentId;
            currentId++;
        }
        game.Inventory.AvailableHats = game.Inventory.AvailableHats.OrderBy(r => r.hatId).ToArray();

        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits");
        foreach (var bait in game.Inventory.AvailableBaits)
        {
            bait.id = currentId;
            currentId++;
        }
        game.Inventory.AvailableBaits = game.Inventory.AvailableBaits.OrderBy(r => r.baitId).ToArray();
    }

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
