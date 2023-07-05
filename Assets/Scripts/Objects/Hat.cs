using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using static Game;

[Serializable]
public class Hat : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string hatName;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private ItemTag itemTag = ItemTag.Bait;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string HatName
    {
        get => hatName;
        set => hatName = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }
    public int Price
    {
        get => price;
        set => price = value;
    }
    public ItemTag ItemTag
    {
        get => itemTag;
        set => itemTag = value;
    }
    public Hat()
    {

    }

    public Hat(HatData hatData)
    {
        id = hatData.id;
        hatName = hatData.hatName;
        description = hatData.description;
        price = hatData.price;
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
        hat.id = hatData.id;
        hat.HatName = hatData.hatName;
        hat.Description = hatData.description;
        hat.Price = hatData.price;
        return hat;
    }

    public void AddHatToInstance()
    {
        MainManager.Instance.game.FoundHats = MainManager.Instance.game.FoundHats.Append(this).ToList();
    }
    public static Hat[] SetAvailableHats(Game game)
    {
        Hat basicHat = game.gameObject.AddComponent<Hat>();
        basicHat.Id = 1;
        basicHat.HatName = "Basic Hat";
        basicHat.Description = "A basic hat for everyday wear";
        basicHat.Price = 10;

        Hat fancyHat = game.gameObject.AddComponent<Hat>();
        fancyHat.Id = 2;
        fancyHat.HatName = "Fancy Hat";
        fancyHat.Description = "A fancy hat for special occasions";
        fancyHat.Price = 20;

        Hat premiumHat = game.gameObject.AddComponent<Hat>();
        premiumHat.Id = 3;
        premiumHat.HatName = "Premium Hat";
        premiumHat.Description = "A premium hat for the most discerning customers";
        premiumHat.Price = 30;

        return new Hat[] { basicHat, fancyHat, premiumHat };
    }


}