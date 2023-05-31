using UnityEngine;
using UnityEngine.Playables;

public class Game : MonoBehaviour
{
    private int days;
    private int fishes;
    private string scene;

    public int Days
    {
        get { return days; }
        set { days = value; }
    }

    public int Fishes
    {
        get { return fishes; }
        set { fishes = value; }
    }

    public string Scene
    {
        get { return scene; }
        set { scene = value; }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }
    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGame();

        days = gameData.daysCount;
        fishes = gameData.fishes;
        scene = gameData.scene;
    }
    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            days = 0;
            fishes = 0;
            scene = "Home";
        }

    }
}
