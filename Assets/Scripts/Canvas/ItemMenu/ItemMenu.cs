using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Item;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemMenu;
    [SerializeField] private ItemWheel[] listOfWheels;
    [SerializeField] private GameObject ItemFocus;
    private ItemWheel focusedWheel;
    private int focusedWheelIndex;
    private List<Item> allItems;


    void Start()
    {
        itemMenu.SetActive(false);
        allItems = MainManager.Instance.game.Inventory.FoundBaits.Cast<Item>().ToList();
        allItems.AddRange(MainManager.Instance.game.Inventory.FoundFishingRods);
        allItems.AddRange(MainManager.Instance.game.Inventory.FoundHats);
    }

    private void PopulateWheels()
    {
        foreach (var wheel in listOfWheels)
        {
            switch (wheel.itemTag)
            {
                case ItemTag.Bait:
                    wheel.SetItems(allItems.OfType<Bait>().ToArray());
                    break;
                case ItemTag.FishingRod:
                    Debug.Log(allItems.OfType<FishingRod>().ToArray()[0].itemTag);
                    wheel.SetItems(allItems.OfType<FishingRod>().ToArray());
                    break;
                case ItemTag.Hat:
                    wheel.SetItems(allItems.OfType<Hat>().ToArray());
                    break;
            }
        }
    }


    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ResetValues();
            ToggleMenu();
        }
        if (itemMenu.activeSelf)
        {
            ScrollBetweenWheels();
            TriggerChangeEquippedItem();
        }
    }

    private void ToggleMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);
        Time.timeScale = itemMenu.activeSelf ? 0 : 1;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var wheel in listOfWheels)
            {
                wheel.ChangeEquippedItem();
            }
            ToggleMenu();
        }
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
        ItemFocus.transform.SetParent(focusedWheel.transform);
        ItemFocus.transform.position = focusedWheel.transform.position;
        for (int i = 0; i < listOfWheels.Length; i++)
        {
            listOfWheels[i].SetWheelFocus(i == focusedWheelIndex);
        }
    }
}
