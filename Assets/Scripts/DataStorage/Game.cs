using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int day;
    private int catchCount;
    private string scene;
    private Fish[] catches = new Fish[0];
    private FishingRod[] fishingRods = new FishingRod[0];
    private FishingRod equippedFishingRod;

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
    public FishingRod[] FishingRods
    {
        get { return fishingRods; }
        set { fishingRods = value; }

    }

    public FishingRod EquippedFishingRod
    {
        get { return equippedFishingRod; }
        set { equippedFishingRod = value; }
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
        catches = gameData.foundCatches.Select(fishData => gameObject.AddComponent<Fish>()).ToArray();
        fishingRods = gameData.foundFishingRods.Select(fishingRodData => gameObject.AddComponent<FishingRod>()).ToArray();
        equippedFishingRod = gameObject.AddComponent<FishingRod>();
        equippedFishingRod.FishingRodName = gameData.equippedFishingRod.fishingRodName;
        equippedFishingRod.Strength = gameData.equippedFishingRod.strength;
        equippedFishingRod.ThrowRange = gameData.equippedFishingRod.throwRange;
        Debug.Log(equippedFishingRod.FishingRodName);

    }
    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            day = 1;
            catchCount = 0;
            scene = "Boat";
            catches = new Fish[0];
            FishingRods = new FishingRod[] { gameObject.AddComponent<FishingRod>() };
            EquippedFishingRod = FishingRods[0];
            Debug.Log(EquippedFishingRod.FishingRodName);
        }

    }
}
