using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemWheel : InfiniteScrollVertical
{
    public ItemTag itemTag;
    private ItemSlot[] listOfItemSlots;
    private Image image;

    private void Start() {
        image = GetComponent<Image>();
    }

    public void ChangeEquippedItem()
    {
        int centerIndex = Mathf.RoundToInt(scrollRect.verticalNormalizedPosition * (listOfItemSlots.Length - 1));
        MainManager.Instance.Inventory.SetEquipment(listOfItemSlots[centerIndex].Id, itemTag);
    }

    public void SetItems(Item[] items)
    {
        if (items == null || items.Length == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(Item[] items)
    {
        listOfItemSlots = new ItemSlot[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            ItemSlot newItemSlot = new()
            {
                Id = items[i].id,
                ItemTag = items[i].itemTag,
                ItemName = items[i].itemName
            };
            listOfItemSlots[i] = newItemSlot;
        }
        AddItems(listOfItemSlots.Select(itemSlot => itemSlot).ToArray());
    }

    public void SetWheelFocus(bool focus)
    {
        if (image != null)
        {
            image.color = focus ? Color.red : Color.white;

        }
    }
}
