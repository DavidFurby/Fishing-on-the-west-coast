using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemScroll : InfiniteScrollVertical<Item>
{
    public ItemTag itemTag;
    private Image image;
    private readonly string itemSlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private ItemSlot itemSlotPrefab;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        itemSlotPrefab = Resources.Load<ItemSlot>(itemSlotPath);
        itemHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
        image = GetComponent<Image>();
    }

    private void OnDisable()
    {
        ClearScroll();
    }

    public void ChangeEquippedItem()
    {
        MainManager.Instance.Inventory.SetEquipment(centeredItem.id, itemTag);
    }

    public void SetItems(List<Item> items)
    {
        if (items == null || items.Count == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(List<Item> items)
    {
        itemArray = items.ToArray();
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
    protected override void UpdateItemSlot(RectTransform parent, Item item, bool asLastSibling = false)
    {
        ItemSlot slot = Instantiate(itemSlotPrefab, parent);
        slot.SetSlot(item.id, item.itemTag, item.itemName);
        if (asLastSibling)
            slot.transform.SetAsLastSibling();
        else
            slot.transform.SetAsFirstSibling();
    }

}
