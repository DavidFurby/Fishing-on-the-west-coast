using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemWheel : InfiniteScrollVertical
{
    public ItemTag itemTag;
    private ItemSlot[] listOfItemSlots;
    private int middleIndex;
    private Image image;

    private void Awake() {
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
        FindEquippedItemIndex(items);
        CreateNewEquipmentSlots(items);
    }

    private void FindEquippedItemIndex(Item[] items)
    {
        if (itemTag == ItemTag.None)
        {

        }
        var equippedItem = MainManager.Instance.Inventory.GetEquipment(itemTag);
        if (equippedItem != null)
        {
            middleIndex = Array.FindIndex(items, item => item.id == equippedItem.id);
        }
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
