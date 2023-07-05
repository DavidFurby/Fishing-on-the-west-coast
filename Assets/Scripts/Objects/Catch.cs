using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Catch : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string fishName;
    [SerializeField] private float size;
    [SerializeField] private string info;
    [SerializeField] private int level;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string CatchName
    {
        get => fishName;
        set => fishName = value;
    }

    public float Size
    {
        get => size;
        set => size = value;
    }

    public string Description
    {
        get => info;
        set => info = value;
    }
    public int Level
    {
        get => level;
        set => level = value;
    }

    public Catch(FishData fishData)
    {
        fishName = fishData.fishName;
        size = fishData.size;
        info = fishData.info;
        level = fishData.level;
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
        MainManager.Instance.game.Catches = MainManager.Instance.game.Catches.Append(this).ToList();
    }

    public void ReplaceFishInInstance(int index)
    {
        MainManager.Instance.game.Catches[index] = this;
    }

    public static Catch[] SetAvailableCatches(Game game)
    {

        Catch basicCatch = game.gameObject.AddComponent<Catch>();
        basicCatch.Id = 1;
        basicCatch.CatchName = "Basic Catch";
        basicCatch.Level = 1;
        basicCatch.Description = "A basic catch easily caught";

        Catch advancedCatch = game.gameObject.AddComponent<Catch>();
        advancedCatch.Id = 2;
        advancedCatch.CatchName = "Advanced Catch";
        advancedCatch.Level = 2;
        advancedCatch.Description = "A more advanced catch not as easily caught";

        Catch premiumCatch = game.gameObject.AddComponent<Catch>();
        premiumCatch.Id = 3;
        premiumCatch.CatchName = "Premium Catch";
        premiumCatch.Level = 3;
        premiumCatch.Description = "A premium catch very rarely caught";

        return new Catch[] { basicCatch, advancedCatch, premiumCatch };
    }
}