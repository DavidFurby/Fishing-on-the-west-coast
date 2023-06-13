using UnityEngine;

public class Game : MonoBehaviour
{
    private int day;
    private int catchCount;
    private string scene;
    private string[] catches;

    public int Days
    {
        get { return day; }
        set { day = value; }
    }

    public int Fishes
    {
        get { return catchCount; }
        set { catchCount = value; }
    }
    public string[] Catches
    {
        get { return catches; }
        set { catches = value; }

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

        day = gameData.daysCount;
        catchCount = gameData.catchCount;
        scene = gameData.scene;
        catches = gameData.foundCatches;
    }
    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            day = 1;
            catchCount = 0;
            scene = "Home";
            catches = new string[0];
        }

    }
}
