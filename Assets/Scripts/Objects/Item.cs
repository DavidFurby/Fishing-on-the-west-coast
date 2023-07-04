using System;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private int price;
    [SerializeField] private string description;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
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
    public Item(int id, string name, int price, string description)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
    }
}
