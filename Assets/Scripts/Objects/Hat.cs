using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Hat : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string hatName;
    [SerializeField] private string description;
    [SerializeField] private int price;
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

    public void AddHatToInstance()
    {
        MainManager.Instance.game.FoundHats = MainManager.Instance.game.FoundHats.Append(this).ToArray();
    }
}