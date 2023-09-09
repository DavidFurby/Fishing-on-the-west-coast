using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Item;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemMenu;
    [SerializeField] private GameObject ItemFocus;
    [SerializeField] private ItemWheel[] listOfWheels;

    private ItemWheel focusedWheel;
    private int focusedWheelIndex;
    private List<Item> allItems;

    void Start()
    {
        itemMenu.SetActive(false);
        allItems = GetItemsAsList(MainManager.Instance.Inventory.FoundBaits);
        allItems.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundRods));
        allItems.AddRange(GetItemsAsList(MainManager.Instance.Inventory.FoundHats));
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
        foreach (ItemWheel wheel in listOfWheels)
        {
            if (wheel != null)
            {
                switch (wheel.itemTag)
                {
                    case ItemTag.Bait:
                        wheel.SetItems(allItems.OfType<Bait>().ToArray());
                        break;
                    case ItemTag.Rod:
                        wheel.SetItems(allItems.OfType<Rod>().ToArray());
                        break;
                    case ItemTag.Hat:
                        wheel.SetItems(allItems.OfType<Hat>().ToArray());
                        break;
                }
            }
        }
    }

    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleMenu();
        }
        if (itemMenu != null && itemMenu.activeSelf)
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
        itemMenu.SetActive(!itemMenu.activeSelf);
        Time.timeScale = itemMenu.activeSelf ? 0 : 1;
        if (itemMenu.activeSelf)
        {
            ResetValues();
        }
    }

    private void ResetValues()
    {
        PopulateWheels();
        focusedWheel = listOfWheels[0];
        focusedWheelIndex = 0;
        SetFocus();
    }

    private void TriggerChangeEquippedItem()
    {
        foreach (ItemWheel wheel in listOfWheels)
        {
            wheel.ChangeEquippedItem();
        }
        ToggleMenu();
    }

    private void ScrollBetweenWheels()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            focusedWheelIndex = (focusedWheelIndex - 1 + listOfWheels.Length) % listOfWheels.Length;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            focusedWheelIndex = (focusedWheelIndex + 1 + listOfWheels.Length) % listOfWheels.Length;
        }
        focusedWheel = listOfWheels[focusedWheelIndex];
        SetFocus();
    }

    private void SetFocus()
    {
        if (focusedWheel != null)
        {
            ItemFocus.transform.SetParent(focusedWheel.transform);
            ItemFocus.transform.position = focusedWheel.transform.position;
            for (int i = 0; i < listOfWheels.Length; i++)
            {
                listOfWheels[i].SetWheelFocus(i == focusedWheelIndex);
            }
        }
    }
}
