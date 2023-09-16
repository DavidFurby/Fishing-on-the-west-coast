using System;
using System.Collections.Generic;
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

    void OnDisable()
    {
        ClearScroll();
    }

    public void ChangeEquippedItem()
    {
        MainManager.Instance.Inventory.SetEquipment(centeredItem.Id, itemTag);
        print(MainManager.Instance.Inventory.EquippedBait.itemName);

    }

    public void SetItems(List<Item> items)
    {
        if (items == null || items.Count == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(List<Item> items)
    {
        Item equippedItem = items.First((Item item) => item.id == MainManager.Instance.Inventory.GetEquippedItem(itemTag).id);
        items.Remove(equippedItem);
        items.Insert(0, equippedItem);

        _itemArray = items.Select(item =>
        {
            GameObject slotObject = new("ItemSlot");
            return ItemSlot.Create(slotObject, item.id, item.itemTag, item.itemName);
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
