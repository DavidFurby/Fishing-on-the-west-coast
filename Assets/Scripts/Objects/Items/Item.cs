using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [HideInInspector] public ItemTag itemTag = ItemTag.None;
    public string id;
    public string Id => id;

    public string itemName = "";
    public int price = 0;
    public string description = "";
    public GameObject model;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToSafeString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    public enum ItemTag
    {
        None,
        Rod,
        Bait,
        Hat,
    }

    public Item CloneItem()
    {
        Item clone = CreateInstance<Item>();
        clone.itemTag = itemTag;
        clone.id = id;
        clone.itemName = itemName;
        clone.price = price;
        clone.description = description;
        clone.model = model;
        return clone;
    }

}
