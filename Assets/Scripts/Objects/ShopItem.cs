using System;
using UnityEngine;

[Serializable]
public class ShopItem : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private int price;
    [SerializeField] private string description;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }
    public string Description
    {
        get { return description; }
        set { description = value; }
    }
}
