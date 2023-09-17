using TMPro;
using UnityEngine;
using static Item;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public string ItemName
    {
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
    public static ItemSlot Create(int id, ItemTag itemTag, string itemName)
    {
        ItemSlot slot = new()
        {
            ItemName = itemName,
            Id = id,
            ItemTag = itemTag
        };
        return slot;
    }

}
