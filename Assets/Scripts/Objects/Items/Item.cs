using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [HideInInspector] public ItemTag itemTag = ItemTag.None;
    public int id = 0;
    public  string itemName = "";
    public int price = 0;
    public string description = "";
    public GameObject model;

    public enum ItemTag
    {
        None,
        Rod,
        Bait,
        Hat,
    }

    public void CopyValuesFrom(Item other)
    {
        itemTag = other.itemTag;
        id = other.id;
        itemName = other.itemName;
        price = other.price;
        description = other.description;
    }
}
