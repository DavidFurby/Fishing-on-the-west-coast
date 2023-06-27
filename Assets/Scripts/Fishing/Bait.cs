using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Bait : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string baitName;
    [SerializeField] private int level;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string BaitName
    {
        get => baitName;
        set => baitName = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public Bait()
    {
        baitName = "basic bait";
        level = 0;
    }

    public Bait(BaitData baitData)
    {
        baitData = baitData.baitName;
        level = baitData.level;
    }

    public void AddFishingRodToInstance()
    {
        MainManager.Instance.game.FishingRods = MainManager.Instance.game.baits.Append(this).ToArray();
    }
}