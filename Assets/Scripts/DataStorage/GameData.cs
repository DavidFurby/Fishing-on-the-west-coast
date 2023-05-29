[System.Serializable]
public class GameData
{

    public int daysCount = 0;
    public int fishes = 0;
    public string scene;


    public GameData(Game game)
    {
        daysCount = game.days;
        fishes = game.fishes;
        scene = game.scene.ToString();
    }
}
