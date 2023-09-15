using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemWheel : InfiniteScrollVertical
{
    public ItemTag itemTag;
    private Image image;


    private void Start()
    {
        image = GetComponent<Image>();
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
        _itemArray = new ItemSlot[items.Length];
        print(_itemArray.Length);
        for (int i = 0; i < items.Length; i++)
        {
            // Get the item slot prefab
            ItemSlot newItemSlot = new()
            {
                // Set the properties of the ItemSlot
                Id = items[i].id,
                ItemTag = items[i].itemTag,
                ItemName = items[i].itemName
            };
            // Add the new item slot object to the list
            _itemArray[i] = newItemSlot;
        }
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
