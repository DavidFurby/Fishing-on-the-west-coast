using System.Linq;

[System.Serializable]
public class GameData
{

    public int daysCount = 0;
    public int catchCount = 0;
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
        catchCount = game.Fishes;
        scene = game.Scene.ToString();
        foundCatches = game.Catches.Select(fish => new FishData(fish)).ToArray();
        foundFishingRods = game.FoundFishingRods.Select(fishingRod => new FishingRodData(fishingRod)).ToArray();
        equippedFishingRod = new FishingRodData(game.EquippedFishingRod);
        foundBaits = game.FoundBaits.Select(bait => new BaitData(bait)).ToArray();
        equippedBait = new BaitData(game.EquippedBait);
        foundHats = game.FoundHats.Select(hat => new HatData(hat)).ToArray();
        equippedHat = new HatData(game.EquippedHat);
    }
}
