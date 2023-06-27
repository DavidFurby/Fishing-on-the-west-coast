using System;
using System.Drawing;
using System.Linq;
using UnityEngine;

[Serializable]
public class FishingRod : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishingRodName;
    [SerializeField] private float strength;
    [SerializeField] private string throwRange;


    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string FishingRodName
    {
        get { return fishingRodName; }
        set { fishingRodName = value; }
    }
    public float Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    public string ThrowRange
    {
        get { return throwRange; }
        set { throwRange = value; }
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
