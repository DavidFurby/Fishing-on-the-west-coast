using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish", order = 1)]
[Serializable]
public class Fish : ScriptableObject
{
    public string fishId;
    public string FishId => fishId;

    public new string name;
    public float averageSize;
    public string description;
    public int level;
    public int exp;
    [HideInInspector] public float size = 0;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(fishId))
        {
            fishId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
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
