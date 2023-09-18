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
    [SerializeField] private GameObject centerArea;

    private void Start()
    {

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
        itemArray = items.ToArray();
        InitialSetup();

    }
    protected override void SetCenterContentChild()
    {
        float minDistance = float.MaxValue;
        int minIndex = -1;
        Transform centerChild = null;
        Vector3 centerAreaPosition = centerArea.transform.position;
        print(centerAreaPosition.y);
        for (int i = 0; i < contentPanelTransform.childCount; i++)
        {
            Vector3 itemPosition = contentPanelTransform.GetChild(i).position;
            float distance = Vector3.Distance(centerAreaPosition, itemPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
                centerChild = contentPanelTransform.GetChild(i);
            }
        }
        print(centerChild.GetComponent<ItemSlot>().NameText.text);
        centeredItem = centerChild;
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
            slot.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        else
            slot.gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
    }

}
