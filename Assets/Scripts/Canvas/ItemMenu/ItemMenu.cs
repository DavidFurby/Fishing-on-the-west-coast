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
        allItems = MainManager.Instance.game.Inventory.FoundBaits.Cast<Item>().ToList();
        allItems.AddRange(MainManager.Instance.game.Inventory.FoundFishingRods);
        allItems.AddRange(MainManager.Instance.game.Inventory.FoundHats);
        ExplorationController.OpenItemMenu += HandleInputs;
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
                    case ItemTag.FishingRod:
                        wheel.SetItems(allItems.OfType<FishingRod>().ToArray());
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
