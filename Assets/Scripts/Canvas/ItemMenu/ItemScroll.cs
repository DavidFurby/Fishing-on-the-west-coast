using System;
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
