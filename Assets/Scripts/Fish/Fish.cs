using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Fish : MonoBehaviour
{
    [SerializeField] private string fishName;
    [SerializeField] private float size;
    [SerializeField] private string info;

    public string FishName
    {
        get { return fishName; }
        set { fishName = value; }
    }
    public float Size
    {
        get { return size; }
        set { size = value; }
    }
    public string Info
    {
        get { return info; }
        set { info = value; }
    }

    public Fish(FishData fishData)
    {
        fishName = fishData.fishName;
        size = fishData.size;
        info = fishData.info;
    }

    public void AddFishToInstance()
    {
        MainManager.Instance.game.Catches = MainManager.Instance.game.Catches.Append(this).ToArray();
    }
}
