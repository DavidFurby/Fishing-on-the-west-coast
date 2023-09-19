using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemScroll : InfiniteScrollVertical<Item>
{
    private const string ItemSlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    public ItemTag itemTag;
    private Image image;
    private ItemSlot itemSlotPrefab;
    [SerializeField] private GameObject centerArea;
    public static event Action OnSetCenterItem;

    private void Start()
    {
        LoadItemSlotPrefab();
        image = GetComponent<Image>();
    }

    private void LoadItemSlotPrefab()
    {
        itemSlotPrefab = Resources.Load<ItemSlot>(ItemSlotPath);
        itemHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
    }

    private void OnDisable()
    {
        ClearScroll();
    }

    public void ChangeEquippedItem()
    {
        MainManager.Instance.Inventory.SetEquipment(centeredItem.GetComponent<ItemSlot>().Id, itemTag);
    }

    public void SetItems(List<Item> items)
    {
        if (items == null || items.Count == 0) return;
        CreateNewEquipmentSlots(items);
    }

    private void CreateNewEquipmentSlots(List<Item> items)
    {
        Item equippedItem = MainManager.Instance.Inventory.GetEquippedItem(itemTag);

        int equippedItemIndex = items.FindIndex(item => item.id == equippedItem.id);
        if (equippedItemIndex > 0)
        {
            items.Remove(equippedItem);
            items.Insert(0, equippedItem);
        }
        itemArray = items.ToArray();
        StartCoroutine(InitialSetup());
    }
    
    protected override void SetCenterContentChild()
    {
        centeredItem = FindClosestChildToCenter();
        OnSetCenterItem.Invoke();
    }

    private Transform FindClosestChildToCenter()
    {
        float minDistance = float.MaxValue;
        Transform centerChild = null;
        Vector3 centerAreaPosition = centerArea.transform.position;

        for (int i = 0; i < contentPanelTransform.childCount; i++)
        {
            Vector3 itemPosition = contentPanelTransform.GetChild(i).position;
            float distance = Vector3.Distance(centerAreaPosition, itemPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                centerChild = contentPanelTransform.GetChild(i);
            }
        }
        
        return centerChild;
    }

    public void SetWheelFocus(bool focus)
    {
        if (image != null)
        {
            image.color = focus ? Color.red : Color.white;
            scrollEnabled = focus;
        }
    }
    
    protected override void UpdateSlot(RectTransform parent, Item item, bool asLastSibling = false)
    {
        ItemSlot slot = Instantiate(itemSlotPrefab, parent);
        
        slot.SetSlot(item.id, item.itemTag, item.itemName);
        
        RectTransform slotRectTransform = slot.gameObject.GetComponent<RectTransform>();
        
        if (asLastSibling)
            slotRectTransform.SetAsLastSibling();
        else
            slotRectTransform.SetAsFirstSibling();
    }
}
