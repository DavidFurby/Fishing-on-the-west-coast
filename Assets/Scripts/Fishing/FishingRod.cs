using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishingRodName;
    [SerializeField] private int strength;
    [SerializeField] private int throwRange;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string FishingRodName
    {
        get => fishingRodName;
        set => fishingRodName = value;
    }

    public int Strength
    {
        get => strength;
        set => strength = value;
    }

    public int ThrowRange
    {
        get => throwRange;
        set => throwRange = value;
    }

    public FishingRod()
    {
        fishingRodName = "Basic rod";
        strength = 20;
        throwRange = 100;
    }

    public FishingRod(FishingRodData fishingRodData)
    {
        fishingRodName = fishingRodData.fishingRodName;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.game.FishingRods = MainManager.Instance.game.FishingRods.Append(this).ToArray();
    }
}