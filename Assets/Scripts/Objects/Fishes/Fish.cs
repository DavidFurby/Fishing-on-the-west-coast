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
    public int exp;
    [HideInInspector] public float size = 0;

    public enum CatchTag
    {
        BasicCatch,
        AdvanceCatch,
        RareCatch,
    }

    public void AddFishToInstance()
    {
        MainManager.Instance.CaughtFishes = MainManager.Instance.CaughtFishes.Append(this).ToList();
    }

    public void ReplaceFishInInstance(int index)
    {
        MainManager.Instance.CaughtFishes[index] = this;
    }
}
