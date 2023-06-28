using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishingRodName;
    [SerializeField] private string description;
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
    public string Description
    {
        get => description;
        set => description = value;
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
        description = "A rod for beginners. Made for seas of the kinder sort";
    }

    public FishingRod(FishingRodData fishingRodData)
    {
        fishingRodName = fishingRodData.fishingRodName;
        strength = fishingRodData.strength;
        throwRange = fishingRodData.throwRange;
        description = fishingRodData.description;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.game.FoundFishingRods = MainManager.Instance.game.FoundFishingRods.Append(this).ToArray();
    }
}