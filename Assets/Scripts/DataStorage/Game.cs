using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int day;
    private int catchCount;
    private string scene;
    private Fish[] catches = new Fish[0];

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
    public Fish[] Catches
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
        catches = gameData.foundCatches.Select(fishData => new Fish(fishData)).ToArray();
    }
    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            day = 1;
            catchCount = 0;
            scene = "Home";
            catches = new Fish[0];
        }

    }
}
