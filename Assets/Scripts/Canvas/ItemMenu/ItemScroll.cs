using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Item;
public class ItemScroll : InfiniteScrollVertical
{
    public ItemTag itemTag;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        ClearScroll();
    }

    public void ChangeEquippedItem()
    {
        int centerIndex = Mathf.RoundToInt(scrollRect.verticalNormalizedPosition * (_itemArray.Length - 1));
        MainManager.Instance.Inventory.SetEquipment(_itemArray[centerIndex].Id, itemTag);
    }

    public void SetItems(Item[] items)
    {
        if (items == null || items.Length == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(Item[] items)
    {
        _itemArray = items.Select(item => new ItemSlot
        {
            Id = item.id,
            ItemTag = item.itemTag,
            ItemName = item.itemName
        }).ToArray();
        InitialSetup();
    }

    public void SetWheelFocus(bool focus)
    {
        if (image != null)
        {
            image.color = focus ? Color.red : Color.white;
            scrollEnabled = focus;
        }
    }
}
