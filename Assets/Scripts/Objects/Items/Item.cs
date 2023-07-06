using System;
using UnityEngine;
using static Game;

[Serializable]
public class Item : MonoBehaviour
{

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
    public ItemTag ItemTag
    {
        get;
        set;
    }
    public Item()
    {

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
        item.Id = id;
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
