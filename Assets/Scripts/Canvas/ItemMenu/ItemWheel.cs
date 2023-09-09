using System;
using UnityEngine;
using static Item;

public class ItemWheel : MonoBehaviour
{
    private const string _SlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private const float _ScaleFactor = 0.5f;

    private ItemSlot _itemSlot;
    public ItemTag _itemTag;
    private ItemSlot[] _listOfItemSlots;
    private bool _wheelIsFocused;
    private float _parentHeight;
    private float _slotHeight;
    private float _spacing;
    private int _middleIndex;

    void OnEnable()
    {
        _itemSlot = Resources.Load<ItemSlot>(_SlotPath);
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
            if (i == _middleIndex)
            {
                MainManager.Instance.Inventory.SetEquipment(_listOfItemSlots[i].Id, _itemTag);
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
        _middleIndex = _listOfItemSlots.Length / 2;

        // Update the position of the equipment slots
        for (int i = 0; i < _listOfItemSlots.Length; i++)
        {
            float yPosition = _parentHeight / 2 - _spacing - _slotHeight / 2 - i * (_slotHeight + _spacing) + _middleIndex * (_slotHeight + _spacing);
            Vector2 newPosition = new(0, yPosition);
            _listOfItemSlots[i].transform.localPosition = newPosition;
        }
    }

    public void SetItems(Item[] items)
    {
        // Check if items is null or empty
        if(items == null || items.Length == 0) return;

        // Destroy any existing equipment slots
        DestroyExistingSlots();

        CalculateSpacing();

        // Find the index of the equipped item
        FindEquippedItemIndex(items);

        // Create new equipment slots
        CreateNewEquipmentSlots(items);
    }

    private void DestroyExistingSlots()
    {
         if (_listOfItemSlots != null && _listOfItemSlots.Length > 0)
         {
             foreach (ItemSlot slot in _listOfItemSlots)
             {
                 DestroyImmediate(slot);
             }
         }
     }

     private void CalculateSpacing()
     {
         _parentHeight = GetComponent<RectTransform>().rect.height;
         _slotHeight = _itemSlot.GetComponent<RectTransform>().rect.height;
         _spacing = (_parentHeight - 3 * _slotHeight) / 2;
     }

     private void FindEquippedItemIndex(Item[] items)
     {
         var equippedItem = MainManager.Instance.Inventory.GetEquipment(_itemTag);
         if(equippedItem != null)
         {
             _middleIndex = Array.FindIndex(items, item => item.id == equippedItem.id);
         }
     }

     private void CreateNewEquipmentSlots(Item[] items)
     {
         _listOfItemSlots = new ItemSlot[items.Length];
         for (int i = 0; i < items.Length; i++)
         {
             float yPosition = _parentHeight / 2 - _spacing - _slotHeight / 2 - i * (_slotHeight + _spacing) + _middleIndex * (_slotHeight + _spacing);
             ItemSlot newItemSlot = Instantiate(_itemSlot, transform);
             newItemSlot.transform.localScale = new Vector2(_ScaleFactor, _ScaleFactor);
             newItemSlot.transform.localPosition = new Vector2 { x = 0, y = yPosition };
             newItemSlot.Id = items[i].id;
             newItemSlot.ItemTag = items[i].itemTag;
             SetText(i, newItemSlot, items);
             _listOfItemSlots[i] = newItemSlot;
         }
     }

     private void SetText(int i, ItemSlot newItemSlot, Item[] equipment)
     {
         string itemName = equipment[i].name ?? "Unnamed";
         newItemSlot.SetTextField(itemName);
     }

     public void SetWheelFocus(bool focus)
     {
         _wheelIsFocused = focus;
     }
}
