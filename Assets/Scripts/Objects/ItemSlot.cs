using TMPro;
using UnityEngine;
using static Item;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI NameText;
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
    public void SetSlot(int id, ItemTag itemTag, string itemName)
    {
        Id = id;
        ItemTag = itemTag;
        if (NameText != null)
        {
            NameText.text = itemName;
        }
    }

}
