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
    private readonly string itemSlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private GameObject itemSlotPrefab;


    private void Start()
    {
        itemSlotPrefab = Resources.Load<GameObject>(itemSlotPath);
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
    GameObject[] itemSlotGameObjects = new GameObject[items.Length];

    for (int i = 0; i < items.Length; i++)
    {
        // Get the item slot prefab
        GameObject newItemSlotObject = itemSlotPrefab;
        ItemSlot newItemSlot = newItemSlotObject.GetComponent<ItemSlot>();

        // Set the properties of the ItemSlot
        newItemSlot.Id = items[i].id;
        newItemSlot.ItemTag = items[i].itemTag;
        newItemSlot.ItemName = items[i].itemName;

        // Add the new item slot object to the list
        listOfItemSlots[i] = newItemSlot;
        itemSlotGameObjects[i] = newItemSlotObject;
    }

    if (listOfItemSlots.Length > 0)
    {
        print(itemSlotGameObjects[0].GetComponent<ItemSlot>().ItemName);
        AddItems(itemSlotGameObjects);
    }
}


    public void SetWheelFocus(bool focus)
    {
        if (image != null)
        {
            image.color = focus ? Color.red : Color.white;
        }
    }
}
