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
        game.CaughtFishes = gameData.foundCatchesId.Select((string catchId) => game.AvailableFishes.First((Fish fish) => fish.fishId == catchId)).ToList();
        game.PlayerLevel.SetPlayerLevel(gameData.level, gameData.experience);
        LoadRod(game, gameData);
        LoadBait(game, gameData);
        LoadHats(game, gameData);

    }

    private static void LoadHats(Game game, GameData gameData)
    {
        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats").OrderBy(r => r.hatId).ToArray();
        game.Inventory.FoundHats = gameData.foundHatsId.Select((string hatId) =>
            game.Inventory.AvailableHats.First((Hat hat) => hat.hatId == hatId)
            ).ToList();
        game.Inventory.EquippedHat = game.Inventory.FoundHats.First((hat) => hat.hatId == gameData.equippedHatId);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits").OrderBy(r => r.baitId).ToArray();

        game.Inventory.FoundBaits = gameData.foundBaitsId.Select((string baitId) => game.Inventory.AvailableBaits.First((Bait bait) => bait.baitId == baitId)).ToList();
        game.Inventory.EquippedBait = game.Inventory.FoundBaits.First((bait) => bait.baitId == gameData.equippedBaitId);
    }

    private static void LoadRod(Game game, GameData gameData)
    {
        game.Inventory.AvailableRods = Resources.LoadAll<Rod>(ItemsPath + "Rods").OrderBy(r => r.rodId).ToArray();

        game.Inventory.FoundRods = gameData.foundRodsId.Select((string rodId) => game.Inventory.AvailableRods.First((Rod rod) => rod.rodId == rodId)).ToList();
        game.Inventory.EquippedRod = game.Inventory.FoundRods.First((rod) => rod.rodId == gameData.equippedRodId);
    }
}
