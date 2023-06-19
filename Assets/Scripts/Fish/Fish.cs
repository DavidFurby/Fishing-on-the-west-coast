using System;
using System.Drawing;
using System.Linq;
using UnityEngine;

[Serializable]
public class Fish : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishName;
    [SerializeField] private float size;
    [SerializeField] private string info;


    public int Id
    {
        get { return id; }
        set { id = value; }
    }
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
    private void Start()
    {
        SetSizeBasedOnAverage();
    }

    private void SetSizeBasedOnAverage()
    {
        size = Mathf.Round(UnityEngine.Random.Range(size / 2, size * 2)) / 100f;
    }

    public void DestroyFish()
    {
        Destroy(gameObject);
    }

    public void AddFishToInstance()
    {
        MainManager.Instance.game.Catches = MainManager.Instance.game.Catches.Append(this).ToArray();
    }
    public void ReplaceFishInInstance(int index)
    {
        MainManager.Instance.game.Catches[index] = this;
    }
}
