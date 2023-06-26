using System.Linq;
using Unity.VisualScripting;

[System.Serializable]
public class GameData
{

    public int daysCount = 0;
    public int catchCount = 0;
    public FishData[] foundCatches = new FishData[0];
    public FishingRodData[] foundFishingRods = new FishingRodData[0];
    public string scene;


    public GameData(Game game)
    {
        daysCount = game.Days;
        catchCount = game.Fishes;
        scene = game.Scene.ToString();
        foundCatches = game.Catches.Select(fish => new FishData(fish)).ToArray();
        foundFishingRods = game.FishingRods.Select(fishingRod => new FishingRodData(fishingRod)).ToArray();
    }
}
