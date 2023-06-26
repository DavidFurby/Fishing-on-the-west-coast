using System;
using UnityEngine;

[Serializable]
public class ShopItem : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private float price;
    [SerializeField] private string description;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public float Price
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
