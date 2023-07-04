using System;
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
        id = 0;
        hatName = "Basic hat";
        description = "Whats a bald old man without a hat";
        price = 0;
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
    public static Hat SetHat(Game game, int id, string name, string description, int price)
    {
        Hat hat = game.gameObject.AddComponent<Hat>();
        hat.id = id;
        hat.HatName = name;
        hat.Description = description;
        hat.Price = price;
        return hat;
    }

    public void AddHatToInstance()
    {
        MainManager.Instance.game.FoundHats = MainManager.Instance.game.FoundHats.Append(this).ToArray();
    }
}