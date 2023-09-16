using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [HideInInspector] public ItemTag itemTag = ItemTag.None;
    public int id = 0;
    public string itemName = "";
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

public Item CloneItem()
{
    Item clone = ScriptableObject.CreateInstance<Item>();
    clone.itemTag = this.itemTag;
    clone.id = this.id;
    clone.itemName = this.itemName;
    clone.price = this.price;
    clone.description = this.description;
    clone.model = this.model;
    return clone;
}

}
