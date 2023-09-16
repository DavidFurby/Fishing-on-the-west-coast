using TMPro;
using UnityEngine;
using static Item;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public string ItemName {
        get; set;
    }
    public int Id
    {
        get; set;
    }
    public ItemTag ItemTag
    {
        get; set;
    }
    void Awake()
    {
        NameText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetTextField(string itemName)
    {
        if (NameText != null)
        {
            NameText.text = itemName;
        }
    }
    public static ItemSlot Create(GameObject target, int id, ItemTag itemTag, string itemName)
    {
        ItemSlot slot = target.AddComponent<ItemSlot>();
        slot.ItemName = itemName;
        slot.Id = id;
        slot.ItemTag = itemTag;
        return slot;
    }

}
