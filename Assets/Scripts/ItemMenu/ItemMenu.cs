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

    // Start is called before the first frame update
    void Start()
    {
        PopulateWheels();
        itemMenu.SetActive(false);
        focusedWheel = listOfWheels[0];
        focusedWheelIndex = 0;
        SetFocus();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

    }

    private void PopulateWheels()
    {
        for (int i = 0; i < listOfWheels.Length; i++)
        {
            if (listOfWheels[i].itemTag == ItemTag.Bait)
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundBaits.Select((bait) => new Item(bait.Id, bait.BaitName, bait.Description)).ToArray());
            }
            else if (listOfWheels[i].itemTag == ItemTag.FishingRod)
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundFishingRods.Select((fishingRod) => new Item(fishingRod.Id, fishingRod.FishingRodName, fishingRod.Description)).ToArray());
            }
            else if (listOfWheels[i].itemTag == ItemTag.Hat)
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundHats.Select((hat) => new Item(hat.Id, hat.HatName, hat.Description)).ToArray());
            }
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetMenu();
        }
        if (itemMenu.activeSelf)
        {
            ScrollBetweenWheels();

            TriggerChangeEquipedItem();

        }

    }

    private void SetMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);
        Time.timeScale = itemMenu.activeSelf ? 0 : 1;
    }

    private void TriggerChangeEquipedItem()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < listOfWheels.Length; i++)
            {
                listOfWheels[i].ChangeEquipedItem();
            }
            SetMenu();
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