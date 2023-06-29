using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Game;

public class EquipmentWheel : MonoBehaviour
{
    [SerializeField] private GameObject equipmentSlot;
    [SerializeField] private Equipment equipmentTag;
    private GameObject[] ListOfEquipmentSlots;
    private bool wheelIsFocused;
    float parentHeight;
    float slotHeight;
    float spacing;

    private void Update()
    {
        if (wheelIsFocused)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ScrollList(-1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ScrollList(1);
            }
        }
    }

    public void ScrollList(int direction)
    {
        // Rotate the order of the equipment slots in the array
        if (direction == 1)
        {
            var firstSlot = ListOfEquipmentSlots[0];
            for (int i = 0; i < ListOfEquipmentSlots.Length - 1; i++)
            {
                ListOfEquipmentSlots[i] = ListOfEquipmentSlots[i + 1];
            }
            ListOfEquipmentSlots[ListOfEquipmentSlots.Length - 1] = firstSlot;
        }
        else if (direction == -1)
        {
            var lastSlot = ListOfEquipmentSlots[ListOfEquipmentSlots.Length - 1];
            for (int i = ListOfEquipmentSlots.Length - 1; i > 0; i--)
            {
                ListOfEquipmentSlots[i] = ListOfEquipmentSlots[i - 1];
            }
            ListOfEquipmentSlots[0] = lastSlot;
        }
        int middleIndex = ListOfEquipmentSlots.Length / 2;

        // Update the position of the equipment slots
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            Vector2 newPosition = new(0, yPosition);
            ListOfEquipmentSlots[i].transform.localPosition = newPosition;
        }
    }

    public void SetEquipment(Equipment[] equipment)
    {
        // Destroy any existing equipment slots
        if (ListOfEquipmentSlots != null)
        {
            foreach (var slot in ListOfEquipmentSlots)
            {
                DestroyImmediate(slot);
            }
        }
        parentHeight = GetComponent<RectTransform>().rect.height;
        slotHeight = equipmentSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 2;

        int middleIndex = equipment.Length / 2;

        // Create new equipment slots
        ListOfEquipmentSlots = new GameObject[equipment.Length];
        for (int i = 0; i < equipment.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            var newEquipmentSlot = Instantiate(equipmentSlot, transform);
            newEquipmentSlot.transform.localScale = new Vector2(0.5f, 0.5f);
            newEquipmentSlot.transform.localPosition = new Vector2 { x = 0, y = yPosition };
            if (newEquipmentSlot.TryGetComponent<Image>(out var slotImage))
            {
                slotImage.color = Random.ColorHSV();
            }
            SetText(i, newEquipmentSlot, equipment);
            ListOfEquipmentSlots[i] = newEquipmentSlot;
        }
    }

    private void SetText(int i, GameObject newEquipmentSlot, Equipment[] equipment)
    {
        TextMeshProUGUI text = newEquipmentSlot.GetComponentInChildren<TextMeshProUGUI>();
        text.text = equipment[i].name;
    }

    public void SetWheelFocus(bool focus)
    {
        wheelIsFocused = focus;
    }
}