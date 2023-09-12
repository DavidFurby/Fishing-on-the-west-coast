using System;
using UnityEngine;
using UnityEngine.UI;
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
    private ScrollRect _scrollRect;
    private Image _image;


    void Start()
    {
        _image = GetComponent<Image>();
        _itemSlot = Resources.Load<ItemSlot>(_SlotPath);

        _scrollRect = GetComponent<ScrollRect>();
    }

    public void ChangeEquippedItem()
    {
        int centerIndex = Mathf.RoundToInt(_scrollRect.verticalNormalizedPosition * (_listOfItemSlots.Length - 1));
        MainManager.Instance.Inventory.SetEquipment(_listOfItemSlots[centerIndex].Id, _itemTag);
    }

    public void SetItems(Item[] items)
    {
        // Check if items is null or empty
        if (items == null || items.Length == 0) return;

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
                Destroy(slot);
            }
        }
    }

    private void CalculateSpacing()
    {
        GameObject content = new("Content");
        content.transform.SetParent(_scrollRect.transform);

        RectTransform rectTransform = content.AddComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(_scrollRect.GetComponent<RectTransform>().rect.width, _scrollRect.GetComponent<RectTransform>().rect.height);
        rectTransform.localPosition = Vector3.zero;

        _scrollRect.content = rectTransform;
        _parentHeight = _scrollRect.content.GetComponent<RectTransform>().rect.height;
        _slotHeight = _itemSlot.GetComponent<RectTransform>().rect.height;
        _spacing = (_parentHeight - 3 * _slotHeight) / 2;
    }

    private void FindEquippedItemIndex(Item[] items)
    {
        var equippedItem = MainManager.Instance.Inventory.GetEquipment(_itemTag);
        if (equippedItem != null)
        {
            _middleIndex = Array.FindIndex(items, item => item.id == equippedItem.id);
        }
    }

    private void CreateNewEquipmentSlots(Item[] items)
    {


        _listOfItemSlots = new ItemSlot[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            ItemSlot newItemSlot = Instantiate(_itemSlot, _scrollRect.content.transform);

            float yPosition = _parentHeight / 2 - _spacing - _slotHeight / 2 - i * (_slotHeight + _spacing) + _middleIndex * (_slotHeight + _spacing);
            Vector2 newPosition = new(0, yPosition);
            newItemSlot.GetComponent<RectTransform>().localPosition = newPosition;

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

        if (_image != null)
        {
            _image.color = focus ? Color.red : Color.white;
        }
    }
}