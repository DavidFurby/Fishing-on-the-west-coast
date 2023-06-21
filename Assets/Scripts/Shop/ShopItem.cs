using System;
using UnityEngine;

[Serializable]
public class ShopItem : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private float price;
    [SerializeField] private string description;


    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string FishName
    {
        get { return itemName; }
        set { itemName = value; }
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

    public ShopItem(FishData fishData)
    {
        itemName = fishData.fishName;
        price = fishData.size;
        description = fishData.info;
    }
    public void DestroyFish()
    {
        Destroy(gameObject);
    }
}
