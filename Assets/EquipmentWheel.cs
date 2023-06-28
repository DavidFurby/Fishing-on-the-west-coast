using TMPro;
using UnityEngine;

public class EquipmentWheel : MonoBehaviour
{
    [SerializeField] private GameObject equipmentSlot;
    private GameObject[] ListOfEquipmentSlots;
    private float parentHeight;
    private float slotHeight;
    private float spacing;
    private bool wheelIsFocused;

    private void Start()
    {
        parentHeight = GetComponent<RectTransform>().rect.height;
        slotHeight = equipmentSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 4;
    }

    private void Update()
    {
        ScrollEquipments();
    }

    private void ScrollEquipments()
    {
        if (wheelIsFocused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ScrollDown();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ScrollUp();
            }
        }
    }

    private void ScrollDown()
    {
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            RectTransform slotTransform = ListOfEquipmentSlots[i].GetComponent<RectTransform>();
            float yPosition = slotTransform.anchoredPosition.y - (slotHeight + spacing);
            slotTransform.anchoredPosition = new Vector2(0, yPosition);
        }
    }

    private void ScrollUp()
    {
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            RectTransform slotTransform = ListOfEquipmentSlots[i].GetComponent<RectTransform>();
            float yPosition = slotTransform.anchoredPosition.y + (slotHeight + spacing);
            slotTransform.anchoredPosition = new Vector2(0, yPosition);
        }
    }

    public void SetEquipment(WheelEquipment[] equipment)
    {
        // Destroy any existing equipment slots
        if (ListOfEquipmentSlots != null)
        {
            foreach (GameObject slot in ListOfEquipmentSlots)
            {
                Destroy(slot);
            }
        }

        // Create new equipment slots
        ListOfEquipmentSlots = new GameObject[equipment.Length];
        for (int i = 0; i < equipment.Length; i++)
        {
            GameObject newEquipmentSlot = Instantiate(equipmentSlot, transform);
            SetText(i, newEquipmentSlot, equipment);
            ListOfEquipmentSlots[i] = newEquipmentSlot;
            // Make the equipment slot smaller
            newEquipmentSlot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            //position the slots
            CalculateThePositionOfSlots(i, newEquipmentSlot);
        }
    }

    private void SetText(int i, GameObject newEquipmentSlot, WheelEquipment[] equipment)
    {
        TextMeshProUGUI textComponent = newEquipmentSlot.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = equipment[i].name;
        }
    }

    private void CalculateThePositionOfSlots(int i, GameObject equipmentSlot)
    {

        float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing);
        equipmentSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPosition);
    }

    public void SetWheelFocus(bool focus)
    {
        wheelIsFocused = focus;
    }
}

public class WheelEquipment
{
    public string name;
    public string description;

    public WheelEquipment(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}