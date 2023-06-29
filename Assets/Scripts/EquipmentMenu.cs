using System.Linq;
using UnityEngine;
using static Game;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private GameObject equipmentWheel;
    [SerializeField] private EquipmentWheel[] listOfWheels;
    [SerializeField] private GameObject equipmentFocus;
    private EquipmentWheel focusedWheel;
    private int focusedWheelIndex;

    // Start is called before the first frame update
    void Start()
    {
        PopulateWheels();
        equipmentWheel.SetActive(false);
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
            if (listOfWheels[i].name == "BaitWheel")
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundBaits.Select((bait) => new Equipment(bait.Id, bait.BaitName, bait.Description)).ToArray());
            }
            else if (listOfWheels[i].name == "FishingRodWheel")
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundFishingRods.Select((fishingRod) => new Equipment(fishingRod.Id, fishingRod.FishingRodName, fishingRod.Description)).ToArray());
            }
            else if (listOfWheels[i].name == "HatWheel")
            {
                listOfWheels[i].SetEquipment(MainManager.Instance.game.FoundHats.Select((hat) => new Equipment(hat.Id, hat.HatName, hat.Description)).ToArray());
            }
        }
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            equipmentWheel.SetActive(!equipmentWheel.activeSelf);
            Time.timeScale = equipmentWheel.activeSelf ? 0 : 1;
        }
        if (equipmentWheel.activeSelf)
        {
            ScrollBetweenWheels();
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
        equipmentFocus.transform.SetParent(focusedWheel.transform);
        equipmentFocus.transform.position = focusedWheel.transform.position;
        for (int i = 0; i < listOfWheels.Length; i++)
        {
            listOfWheels[i].SetWheelFocus(i == focusedWheelIndex);
        }
    }
}