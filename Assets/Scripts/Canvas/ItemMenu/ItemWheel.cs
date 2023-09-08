using System;
using UnityEngine;
using static Item;

public class ItemWheel : MonoBehaviour
{
    private const string SlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";

    private ItemSlot ItemSlot;
    public ItemTag itemTag;
    private ItemSlot[] _listOfItemSlots;
    private bool _wheelIsFocused;
    float parentHeight;
    float slotHeight;
    float spacing;
    int middleIndex;

    void OnEnable()
    {
        ItemSlot = Resources.Load<ItemSlot>(SlotPath);
      
    }

    private void Update()
    {
        if (_wheelIsFocused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ScrollList(-1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ScrollList(1);
            }
        }

    }
    public void ChangeEquippedItem()
    {
        for (int i = 0; i < _listOfItemSlots.Length; i++)
        {
            if (i == middleIndex)
            {
                MainManager.Instance.Inventory.SetEquipment(_listOfItemSlots[i].Id, itemTag);
            }
        }
    }

    private void ScrollList(int direction)
    {
        // Rotate the order of the equipment slots in the array
        if (direction == 1)
        {
            ItemSlot firstSlot = _listOfItemSlots[0];
            for (int i = 0; i < _listOfItemSlots.Length - 1; i++)
            {
                _listOfItemSlots[i] = _listOfItemSlots[i + 1];
            }
            _listOfItemSlots[^1] = firstSlot;
        }
        else if (direction == -1)
        {
            ItemSlot lastSlot = _listOfItemSlots[^1];
            for (int i = _listOfItemSlots.Length - 1; i > 0; i--)
            {
                _listOfItemSlots[i] = _listOfItemSlots[i - 1];
            }
            _listOfItemSlots[0] = lastSlot;
        }
        middleIndex = _listOfItemSlots.Length / 2;

        // Update the position of the equipment slots
        for (int i = 0; i < _listOfItemSlots.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            Vector2 newPosition = new(0, yPosition);
            _listOfItemSlots[i].transform.localPosition = newPosition;
        }
    }

    public void SetItems(Item[] items)
    {
        // Destroy any existing equipment slots
        if (_listOfItemSlots != null && _listOfItemSlots.Length > 0)
        {
            foreach (ItemSlot slot in _listOfItemSlots)
            {
                DestroyImmediate(slot);
            }
        }

        parentHeight = GetComponent<RectTransform>().rect.height;
        slotHeight = ItemSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 2;

        // Find the index of the equipped item
        middleIndex = Array.FindIndex(items, item => item.id == MainManager.Instance.Inventory.GetEquipment(item.itemTag).id);
        // Create new equipment slots
        _listOfItemSlots = new ItemSlot[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            ItemSlot newItemSlot = Instantiate(ItemSlot, transform);
            newItemSlot.transform.localScale = new Vector2(0.5f, 0.5f);
            newItemSlot.transform.localPosition = new Vector2 { x = 0, y = yPosition };
            newItemSlot.Id = items[i].id;
            newItemSlot.ItemTag = items[i].itemTag;
            SetText(i, newItemSlot, items);
            _listOfItemSlots[i] = newItemSlot;
        }
    }


    private void SetText(int i, ItemSlot newItemSlot, Item[] equipment)
    {
        newItemSlot.SetTextField(equipment[i].name);
    }

    public void SetWheelFocus(bool focus)
    {
        _wheelIsFocused = focus;
    }
}