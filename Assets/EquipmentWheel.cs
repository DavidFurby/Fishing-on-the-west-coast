using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentWheel : MonoBehaviour
{
    [SerializeField] private GameObject equipmentSlot;
    private GameObject[] ListOfEquipmentSlots;
    private float parentHeight;
    private float slotHeight;
    private float spacing;
    private bool wheelIsFocused;

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
                Scroll(-1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Scroll(1);
            }
        }
    }

    private void Scroll(int direction)
    {
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            RectTransform slotTransform = ListOfEquipmentSlots[i].GetComponent<RectTransform>();
            float yPosition = slotTransform.anchoredPosition.y + direction * (slotHeight + spacing);
            slotTransform.anchoredPosition = new Vector2(0, yPosition);

            // Check if the last slot has gone past the bottom
            if (i == ListOfEquipmentSlots.Length - 1 && yPosition < -parentHeight / 2)
            {
                // Update its y position
                yPosition = ListOfEquipmentSlots[0].GetComponent<RectTransform>().anchoredPosition.y - direction * (slotHeight + spacing);
                // Move the last slot to the start of the list
                ListOfEquipmentSlots[i].transform.SetAsFirstSibling();
                slotTransform.anchoredPosition = new Vector2(0, yPosition);
            }
        }
    }

    public void SetEquipment(WheelEquipment[] equipment)
    {
        // Destroy any existing equipment slots
        if (ListOfEquipmentSlots != null)
        {
            foreach (GameObject slot in ListOfEquipmentSlots)
            {
                DestroyImmediate(slot);
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
            // Set the color of the slot to a random color
            if (newEquipmentSlot.TryGetComponent<Image>(out var slotImage))
            {
                slotImage.color = Random.ColorHSV();
            }
            //position the slots
            CalculateThePositionOfSlot(i, newEquipmentSlot);
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

    private void CalculateThePositionOfSlot(int i, GameObject equipmentSlot)
    {
        parentHeight = GetComponent<RectTransform>().rect.height;
        slotHeight = equipmentSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 4;
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
    public int id;
    public string name;
    public string description;

    public WheelEquipment(int id, string name, string description)
    {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}