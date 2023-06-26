using System;
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
        size = UnityEngine.Random.Range(size, size * 2);
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
