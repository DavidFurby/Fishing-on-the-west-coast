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
        baitName = "Basic bait";
        level = 0;
    }

    public Bait(BaitData baitData)
    {
        baitName = baitData.baitName;
        level = baitData.level;
    }

    public void AddBaitToInstance()
    {
        MainManager.Instance.game.Baits = MainManager.Instance.game.Baits.Append(this).ToArray();
    }
}