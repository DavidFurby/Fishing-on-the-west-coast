using System;
using UnityEngine;
using static Hat;

[Serializable]
public class Item : MonoBehaviour
{
    public ItemTag itemTag = ItemTag.None;

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
    public void Awake()
    {
        SetDefaultVariables();
    }

    private void SetDefaultVariables()
    {
        Id = 0;
        Name = "Empty";
        Description = "";
        Price = 0;

    }
}
