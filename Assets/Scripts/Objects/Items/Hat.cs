using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Hat : Item
{
    [SerializeField] private HatTag hatTag;

    public enum HatTag
    {
        BasicHat,
        FancyHat,
        PremiumHat
    }
    private void Awake()
    {
        SetHatVariables();
    }

    public Hat(HatTag hatTag)
    {
        this.hatTag = hatTag;
        SetHatVariables();
    }

    public Hat(HatData hatData)
    {
        Id = hatData.id;
        Name = hatData.name;
        Description = hatData.description;
        Price = hatData.price;
    }
    private void SetHatVariables()
    {
        switch (hatTag)
        {
            case HatTag.BasicHat:
                Id = 1;
                Name = "Basic Hat";
                Description = "A basic hat given at childbirth";
                Price = 0;
                break;
            case HatTag.FancyHat:
                Id = 2;
                Name = "Fancy Hat";
                Description = "A more advanced hat not as easily found";
                Price = 5;
                break;
            case HatTag.PremiumHat:
                Id = 3;
                Name = "Premium Hat";
                Description = "A premium hat very rarely found";
                Price = 10;
                break;
        }
        itemTag = ItemTag.Hat;
    }

    public static Hat SetHat(Game game, HatData hatData)
    {
        Hat hat = game.gameObject.AddComponent<Hat>();
        hat.Id = hatData.id;
        hat.Name = hatData.name;
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