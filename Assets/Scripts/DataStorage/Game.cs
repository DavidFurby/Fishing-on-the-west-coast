using UnityEngine;

public class Game : MonoBehaviour
{
    public int days;
    public int fishes;
    public string scene;

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
        SaveSystem.NewGame();

    }
}
