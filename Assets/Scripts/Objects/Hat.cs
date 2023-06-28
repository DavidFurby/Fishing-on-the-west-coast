using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Hat : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string hatName;
    [SerializeField] private string description;

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

    public Hat()
    {
        hatName = "Basic hat";
        description = "Whats a bald old man without a hat";
    }

    public Hat(HatData hatData)
    {
        hatName = hatData.hatName;
        description = hatData.description;
    }

    public void AddHatToInstance()
    {
        MainManager.Instance.game.FoundHats = MainManager.Instance.game.FoundHats.Append(this).ToArray();
    }
}