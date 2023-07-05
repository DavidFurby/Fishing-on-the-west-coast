using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Catch : MonoBehaviour
{
    [SerializeField] CatchTag baitTag;

    public enum CatchTag
    {
        BasicCatch,
        AdvanceCatch,
        RareCatch,
    }

    public int Id { get; set; }
    public string CatchName { get; set; }
    public float Size { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public Catch(CatchTag catchTag)
    {
        this.baitTag = catchTag;
        SetCatchVariables();
    }

    public Catch(FishData fishData)
    {
        CatchName = fishData.fishName;
        Size = fishData.size;
        Description = fishData.info;
        Level = fishData.level;
    }

    private void Start()
    {
        SetCatchVariables();
    }

    private void SetCatchVariables()
    {
        switch (baitTag)
        {
            case CatchTag.BasicCatch:
                Id = 1;
                CatchName = "Basic Catch";
                Level = 1;
                Description = "A basic catch easily caught";
                Size = 2;
                break;
            case CatchTag.AdvanceCatch:
                Id = 2;
                CatchName = "Advanced Catch";
                Level = 2;
                Description = "A more advanced catch not as easily caught";
                Size = 5;
                break;
            case CatchTag.RareCatch:
                Id = 3;
                CatchName = "Premium Catch";
                Level = 3;
                Description = "A premium catch very rarely caught";
                Size = 10;
                break;
        }
        Size = UnityEngine.Random.Range(Size, Size * 2);
    }

    public override string ToString()
    {
        return $"Id: {Id}, CatchName: {CatchName}, Size: {Size}, Description: {Description}, Level: {Level}";
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
        basicCatch.baitTag = CatchTag.BasicCatch;
        basicCatch.SetCatchVariables();

        Catch advancedCatch = game.gameObject.AddComponent<Catch>();
        advancedCatch.baitTag = CatchTag.AdvanceCatch;
        advancedCatch.SetCatchVariables();

        Catch premiumCatch = game.gameObject.AddComponent<Catch>();
        premiumCatch.baitTag = CatchTag.RareCatch;
        premiumCatch.SetCatchVariables();

        return new Catch[] { basicCatch, advancedCatch, premiumCatch };
    }
}
