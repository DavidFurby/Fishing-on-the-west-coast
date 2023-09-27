using System.Linq;

[System.Serializable]
public class GameData
{

    public int daysCount = 0;
    public int TotalCatches = 0;
    public float bestDistance;
    public int level;
    public int experience;
    public int[] foundCatchesId;
    public int[] foundRodsId;
    public int equippedRodId;
    public string scene;
    public int[] foundBaitsId;
    public int equippedBaitId;
    public int[] foundHatsId;
    public int equippedHatId;

    public GameData(Game game)
    {
        daysCount = game.Days;
        TotalCatches = game.TotalCatches;
        bestDistance = game.BestDistance;
        level = game.PlayerLevel.Level;
        experience = game.PlayerLevel.Exp;
        scene = game.Scene.ToString();
        foundCatchesId = game.CaughtFishes.Select(fish => fish.id).ToArray();
        foundRodsId = game.Inventory.FoundRods.Select(rod => rod.rodId).ToArray();
        equippedRodId = game.Inventory.EquippedRod.rodId;
        foundBaitsId = game.Inventory.FoundBaits.Select(bait => bait.baitId).ToArray();
        equippedBaitId = game.Inventory.EquippedBait.baitId;
        foundHatsId = game.Inventory.FoundHats.Select(hat => hat.hatId).ToArray();
        equippedHatId = game.Inventory.EquippedHat.hatId;
    }
}
