using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameObject _itemMenu;
    [SerializeField] private ItemScroll[] _listOfWheels;
    [SerializeField] private TextMeshProUGUI itemText;

    private ItemScroll _focusedWheel;
    private int _focusedWheelIndex;
    private List<Item> _allItems;

    void Start()
    {
        InitializeItemMenu();
        ItemScroll.OnSetCenterItem += UpdateItemText;
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
        ItemScroll.OnSetCenterItem -= UpdateItemText;
    }

    private void InitializeItemMenu()
    {
        _itemMenu.SetActive(false);
        _allItems = GetAllItems();
        SubscribeToEvents();
    }

    private List<Item> GetAllItems()
    {
        List<Item> items = GetItemsAsList(MainManager.Instance.Inventory.FoundBaits);
        items.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundRods));
        items.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundHats));
        return items;
    }

    private List<Item> GetItemsAsList(IEnumerable<Item> items)
    {
        return items.Cast<Item>().ToList();
    }

    private void SubscribeToEvents()
    {
        ExplorationController.OpenItemMenu += HandleInputs;
    }

    private void UnsubscribeFromEvents()
    {
        ExplorationController.OpenItemMenu -= HandleInputs;
    }

    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleMenu();
        }

        if (IsMenuActive())
        {
            ScrollBetweenWheels();
            CheckForEquippedItemChange();
        }
    }

    private bool IsMenuActive()
    {
        return _itemMenu != null && _itemMenu.activeSelf;
    }

    private void CheckForEquippedItemChange()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerChangeEquippedItem();
        }
    }

    private void ToggleMenu()
    {
        _itemMenu.SetActive(!_itemMenu.activeSelf);
        Time.timeScale = _itemMenu.activeSelf ? 0 : 1;

        if (_itemMenu.activeSelf)
        {
            StartCoroutine(ResetValues());
        }
    }

    private IEnumerator ResetValues()
    {
        PopulateWheels();
        SetInitialFocus();
        yield return new WaitForEndOfFrame();
        SetFocus();
    }

    private void PopulateWheels()
    {
        foreach (ItemScroll wheel in _listOfWheels)
        {
            if (wheel != null)
            {
                var filteredItems = _allItems.Where(item => item.itemTag == wheel.itemTag).ToList();
                wheel.SetItems(filteredItems);
            }
        }
    }

    private void SetInitialFocus()
    {
        _focusedWheel = _listOfWheels[0];
        _focusedWheelIndex = 0;
    }

    private void TriggerChangeEquippedItem()
    {
        foreach (ItemScroll wheel in _listOfWheels)
        {
            wheel.ChangeEquippedItem();
        }

        ToggleMenu();
    }

    private void ScrollBetweenWheels()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeFocus(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeFocus(1);
        }
    }


    private void ChangeFocus(int direction)
    {
        _focusedWheelIndex = (_focusedWheelIndex + direction + _listOfWheels.Length) % _listOfWheels.Length;
        _focusedWheel = _listOfWheels[_focusedWheelIndex];
        SetFocus();
    }

    private void SetFocus()
    {
        UpdateWheelFocus();
        UpdateItemText();
    }

    private void UpdateWheelFocus()
    {
        for (int i = 0; i < _listOfWheels.Length; i++)
        {
            _listOfWheels[i].SetWheelFocus(i == _focusedWheelIndex);
        }
    }

    private void UpdateItemText()
    {
        if (_focusedWheel.centeredItem != null)
        {
            itemText.text = _focusedWheel.centeredItem.GetComponent<ItemSlot>().NameText.text;
        }
    }
}
