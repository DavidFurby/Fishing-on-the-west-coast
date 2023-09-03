using System.Linq;

[System.Serializable]
public class GameData
{

    public int daysCount = 0;
    public int TotalCatches = 0;
    public float bestDistance;
    public int level;
    public int experience;
    public FishData[] foundCatches = new FishData[0];
    public FishingRodData[] foundFishingRods = new FishingRodData[0];
    public FishingRodData equippedFishingRod;
    public string scene;
    public BaitData[] foundBaits = new BaitData[0];
    public BaitData equippedBait;
    public HatData[] foundHats;
    public HatData equippedHat;

    public GameData(Game game)
    {
        daysCount = game.Days;
        TotalCatches = game.TotalCatches;
        bestDistance = game.BestDistance;
        level = game.PlayerLevel.Level;
        experience = game.PlayerLevel.Exp;
        scene = game.Scene.ToString();
        foundCatches = game.CaughtFishes.Select(fish => new FishData(fish)).ToArray();
        foundFishingRods = game.Inventory.FoundFishingRods.Select(fishingRod => new FishingRodData(fishingRod)).ToArray();
        equippedFishingRod = new FishingRodData(game.Inventory.EquippedFishingRod);
        foundBaits = game.Inventory.FoundBaits.Select(bait => new BaitData(bait)).ToArray();
        equippedBait = new BaitData(game.Inventory.EquippedBait);
        foundHats = game.Inventory.FoundHats.Select(hat => new HatData(hat)).ToArray();
        equippedHat = new HatData(game.Inventory.EquippedHat);
    }
}
