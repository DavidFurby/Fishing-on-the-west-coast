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

    private void OnDisable()
    {
        ClearScroll();
    }

    public void ChangeEquippedItem()
    {
        MainManager.Instance.Inventory.SetEquipment(centeredItem.Id, itemTag);
    }

    public void SetItems(List<Item> items)
    {
        if (items == null || items.Count == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(List<Item> items)
    {
        Item equippedItem = items.FirstOrDefault(item => item.id == MainManager.Instance.Inventory.GetEquippedItem(itemTag).id);
        if (equippedItem != null)
        {
            items.Remove(equippedItem);
            items.Insert(0, equippedItem);
        }

        itemArray = items.Select(item =>
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
