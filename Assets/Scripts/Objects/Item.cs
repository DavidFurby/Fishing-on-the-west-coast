using System;
using UnityEngine;
using static Game;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private new string name;
    [SerializeField] private int price;
    [SerializeField] private string description;
    [SerializeField] private ItemTag itemTag;

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
    public ItemTag ItemTag
    {
        get { return itemTag; }
        set { itemTag = value; }
    }

    public Item(int id, string name, int price, string description, ItemTag itemTag)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        ItemTag = itemTag;
    }
    public static Item SetItem(GameObject gameObject, int id, string name, string description, int price, ItemTag itemTag)
    {
        Item item = gameObject.AddComponent<Item>();
        item.id = id;
        item.Name = name;
        item.Description = description;
        item.Price = price;
        item.ItemTag = itemTag;
        return item;
    }


    public static Item CreateItem(int id, string name, int price, string description, ItemTag itemTag)
    {
        var itemGameObject = new GameObject("Item");
        var item = itemGameObject.AddComponent<Item>();
        item.Id = id;
        item.Name = name;
        item.Price = price;
        item.Description = description;
        item.ItemTag = itemTag;
        return item;
    }
}
