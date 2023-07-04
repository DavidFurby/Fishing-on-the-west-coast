using System.Linq;
using UnityEngine;
using static Game;

public class ItemMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemMenu;
    [SerializeField] private ItemWheel[] listOfWheels;
    [SerializeField] private GameObject ItemFocus;
    private ItemWheel focusedWheel;
    private int focusedWheelIndex;

    void Start()
    {
        itemMenu.SetActive(false);
    }

    void Update()
    {
        HandleInputs();
    }

    private void PopulateWheels()
    {
        foreach (var wheel in listOfWheels)
        {
            switch (wheel.itemTag)
            {
                case ItemTag.Bait:
                    wheel.SetEquipment(MainManager.Instance.game.FoundBaits.Select(bait => Item.CreateItem(bait.Id, bait.BaitName, bait.Price, bait.Description)).ToArray());
                    break;
                case ItemTag.FishingRod:
                    wheel.SetEquipment(MainManager.Instance.game.FoundFishingRods.Select(fishingRod => Item.CreateItem(fishingRod.Id, fishingRod.FishingRodName, fishingRod.Price, fishingRod.Description)).ToArray());
                    break;
                case ItemTag.Hat:
                    wheel.SetEquipment(MainManager.Instance.game.FoundHats.Select(hat => Item.CreateItem(hat.Id, hat.HatName, hat.Price, hat.Description)).ToArray());
                    break;
            }
        }
    }


    private void HandleInputs()
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
                wheel.ChangeEquipedItem();
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
