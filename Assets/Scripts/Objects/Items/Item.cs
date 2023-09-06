using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    [HideInInspector] public ItemTag itemTag = ItemTag.None;
    [SerializeField] public int id = 0;
    [SerializeField] public new string name = "";
    [SerializeField] public int price = 0;
    [SerializeField] public string description = "";
    [SerializeField] public GameObject model;

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
        name = other.name;
        price = other.price;
        description = other.description;
    }
}
