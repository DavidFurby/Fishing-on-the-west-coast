using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Item;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int TotalCatches { get; set; }
    public float BestDistance { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public Fish[] AvailableFishes { get; set; }
    public List<Fish> CaughtFishes { get; set; } = new List<Fish>();
    public string Scene { get; set; }


        public Inventory Inventory { get; set; } = new Inventory();

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        LoadGameController.LoadGame(this);
    }

    public void NewGame()
    {
        NewGameController.InitializeNewGame(this);
    }
}

