using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int Days { get; set; }
    public int Fishes { get; set; }
    public Fish[] Catches { get; set; } = new Fish[0];
    public FishingRod[] FishingRods { get; set; } = new FishingRod[0];
    public FishingRod EquippedFishingRod { get; set; }
    public string Scene { get; set; }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData gameData = SaveSystem.LoadGame();

        Days = gameData.daysCount;
        Fishes = gameData.catchCount;
        Scene = gameData.scene;
        Catches = gameData.foundCatches.Select(fishData => gameObject.AddComponent<Fish>()).ToArray();
        FishingRods = gameData.foundFishingRods.Select(fishingRodData => gameObject.AddComponent<FishingRod>()).ToArray();
        EquippedFishingRod = gameObject.AddComponent<FishingRod>();
        EquippedFishingRod.FishingRodName = gameData.equippedFishingRod.fishingRodName;
        EquippedFishingRod.Strength = gameData.equippedFishingRod.strength;
        EquippedFishingRod.ThrowRange = gameData.equippedFishingRod.throwRange;
    }

    public void NewGame()
    {
        bool removed = SaveSystem.NewGame();
        if (removed)
        {
            Days = 1;
            Fishes = 0;
            Scene = "Boat";
            Catches = new Fish[0];
            FishingRods = new FishingRod[] { gameObject.AddComponent<FishingRod>() };
            EquippedFishingRod = FishingRods[0];
        }
    }
}