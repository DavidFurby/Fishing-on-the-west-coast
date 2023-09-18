using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Item;

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
        _itemMenu.SetActive(false);
        _allItems = GetItemsAsList(MainManager.Instance.Inventory.FoundBaits);
        _allItems.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundRods));
        _allItems.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundHats));

        ExplorationController.OpenItemMenu += HandleInputs;
    }

    void OnDestroy()
    {
        ExplorationController.OpenItemMenu -= HandleInputs;

    }
    private List<Item> GetItemsAsList(IEnumerable<Item> items)
    {
        return items.Cast<Item>().ToList();
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

    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleMenu();
        }
        if (_itemMenu != null && _itemMenu.activeSelf)
        {
            ScrollBetweenWheels();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerChangeEquippedItem();
            }
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
        _focusedWheel = _listOfWheels[0];
        _focusedWheelIndex = 0;
        yield return new WaitForEndOfFrame();
        SetFocus();
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
            _focusedWheelIndex = (_focusedWheelIndex - 1 + _listOfWheels.Length) % _listOfWheels.Length;
            _focusedWheel = _listOfWheels[_focusedWheelIndex];
            SetFocus();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _focusedWheelIndex = (_focusedWheelIndex + 1 + _listOfWheels.Length) % _listOfWheels.Length;
            _focusedWheel = _listOfWheels[_focusedWheelIndex];
            SetFocus();
        }

    }

    private void SetFocus()
    {
        if (_focusedWheel != null)
        {
            for (int i = 0; i < _listOfWheels.Length; i++)
            {
                _listOfWheels[i].SetWheelFocus(i == _focusedWheelIndex);
            }
        }
        if (_focusedWheel.centeredItem != null)
        {
            itemText.text = _focusedWheel.centeredItem.GetComponent<ItemSlot>().NameText.text;
        }
    }
}
