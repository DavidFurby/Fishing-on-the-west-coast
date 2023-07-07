using System;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] public ItemTag itemTag = ItemTag.None;
    public int Id
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }
    public int Price
    {
        get;
        set;
    }
    public string Description
    {
        get;
        set;
    }
    public enum ItemTag
    {
        None,
        FishingRod,
        Bait,
        Hat,
    }
}
