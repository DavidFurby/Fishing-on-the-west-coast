using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish", order = 1)]
[Serializable]
public class Fish : ScriptableObject
{
    public int id;
    public new string name;
    public float averageSize;
    public string description;
    public int level;
    [HideInInspector] public float size = 0;

    public enum CatchTag
    {
        BasicCatch,
        AdvanceCatch,
        RareCatch,
    }

    public void AddFishToInstance()
    {
        MainManager.Instance.game.CaughtFishes = MainManager.Instance.game.CaughtFishes.Append(this).ToList();
    }

    public void ReplaceFishInInstance(int index)
    {
        MainManager.Instance.game.CaughtFishes[index] = this;
    }

    public static Fish SetFish(FishData fishData)
    {
        Fish fish = CreateInstance<Fish>();
        fish.id = fishData.id;
        fish.name = fishData.name;
        fish.description = fishData.description;
        fish.size = fishData.size;
        fish.level = fishData.level;
        return fish;
    }
}
