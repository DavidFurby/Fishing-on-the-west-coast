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
        game.CaughtFishes = gameData.foundCatchesId.Select((int catchId) => game.AvailableFishes.First((Fish fish) => fish.id == catchId)).ToList();
        game.PlayerLevel.SetPlayerLevel(gameData.level, gameData.experience);
        LoadRod(game, gameData);
        LoadBait(game, gameData);
        LoadHats(game, gameData);

    }

    private static void LoadHats(Game game, GameData gameData)
    {
        game.Inventory.AvailableHats = Resources.LoadAll<Hat>(ItemsPath + "Hats").OrderBy(r => r.hatId).ToArray();
        game.Inventory.FoundHats = gameData.foundHatsId.Select((int hatId) =>
            game.Inventory.AvailableHats.First((Hat hat) => hat.hatId == hatId)
            ).ToList();
        game.Inventory.EquippedHat = game.Inventory.FoundHats.First((hat) => hat.hatId == gameData.equippedHatId);
    }

    private static void LoadBait(Game game, GameData gameData)
    {
        game.Inventory.AvailableBaits = Resources.LoadAll<Bait>(ItemsPath + "Baits").OrderBy(r => r.baitId).ToArray();

        game.Inventory.FoundBaits = gameData.foundBaitsId.Select((int baitId) => game.Inventory.AvailableBaits.First((Bait bait) => bait.baitId == baitId)).ToList();
        game.Inventory.EquippedBait = game.Inventory.FoundBaits.First((bait) => bait.baitId == gameData.equippedBaitId);
    }

    private static void LoadRod(Game game, GameData gameData)
    {
        print(gameData.foundRodsId[0]);
        game.Inventory.AvailableRods = Resources.LoadAll<Rod>(ItemsPath + "Rods").OrderBy(r => r.rodId).ToArray();
        print(game.Inventory.AvailableRods[0].rodId);

        game.Inventory.FoundRods = gameData.foundRodsId.Select((int rodId) => game.Inventory.AvailableRods.First((Rod rod) => rod.rodId == rodId)).ToList();
        game.Inventory.EquippedRod = game.Inventory.FoundRods.First((rod) => rod.rodId == gameData.equippedRodId);
    }
}
