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
        print(NameText);
    }
    public void SetTextField(string itemName)
    {
        print("Set");
        print(NameText);
        if (NameText != null)
        {
            NameText.text = itemName;
            print(NameText.text);
        }
    }

}
