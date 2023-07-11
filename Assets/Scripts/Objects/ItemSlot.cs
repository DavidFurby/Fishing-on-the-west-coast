using TMPro;
using UnityEngine;
using static Item;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    public int Id
    {
        get; set;
    }
    public ItemTag ItemTag
    {
        get; set;
    }

    public void SetTextField(string itemName)
    {
        nameText.text = itemName;
    }
}
