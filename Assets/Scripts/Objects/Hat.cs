using System;
using System.Linq;
using UnityEngine;
using static Game;

[Serializable]
public class Hat : MonoBehaviour
{
    [SerializeField] private HatTag hatTag;

    public enum HatTag
    {
        BasicHat,
        FancyHat,
        PremiumHat
    }

    public int Id { get; set; }


    public string HatName
    {
        get; set;
    }

    public string Description
    {
        get; set;
    }
    public int Price
    {
        get; set;
    }
    public ItemTag ItemTag
    {
        get; set;
    } = ItemTag.Hat;

    private void Start()
    {
        SetHatVariables();
    }

    public Hat(HatTag hatTag)
    {
        this.hatTag = hatTag;
        SetHatVariables();
    }

    private void SetHatVariables()
    {
        switch (hatTag)
        {
            case HatTag.BasicHat:
                Id = 1;
                HatName = "Basic Bait";
                Description = "A basic bait given at childbirth";
                Price = 0;
                break;
            case HatTag.FancyHat:
                Id = 2;
                HatName = "Advanced Bait";
                Description = "A more advanced bait not as easily found";
                Price = 5;
                break;
            case HatTag.PremiumHat:
                Id = 3;
                HatName = "Premium Bait";
                Description = "A premium bait very rarely found";
                Price = 10;
                break;
        }
    }
    public Hat()
    {

    }

    public Hat(HatData hatData)
    {
        Id = hatData.id;
        HatName = hatData.hatName;
        Description = hatData.description;
        Price = hatData.price;
    }

    public Hat(int id, string name, int price, string description)
    {
        Id = id;
        HatName = name;
        Price = price;
        Description = description;
    }

    public static Hat SetHat(Game game, HatData hatData)
    {
        Hat hat = game.gameObject.AddComponent<Hat>();
        hat.Id = hatData.id;
        hat.HatName = hatData.hatName;
        hat.Description = hatData.description;
        hat.Price = hatData.price;

        return hat;
    }

    public void AddHatToInstance()
    {
        MainManager.Instance.game.FoundHats =
            MainManager.Instance.game.FoundHats.Append(this).ToList();
    }

    public static Hat[] SetAvailableHats(Game game)
    {
        Hat basicHat = game.gameObject.AddComponent<Hat>();
        basicHat.hatTag = HatTag.BasicHat;
        basicHat.SetHatVariables();

        Hat fancyHat = game.gameObject.AddComponent<Hat>();
        fancyHat.hatTag = HatTag.FancyHat;
        fancyHat.SetHatVariables();

        Hat rareHat = game.gameObject.AddComponent<Hat>();
        rareHat.hatTag = HatTag.PremiumHat;
        rareHat.SetHatVariables();
        return new Hat[] { basicHat, fancyHat, rareHat };
    }
}