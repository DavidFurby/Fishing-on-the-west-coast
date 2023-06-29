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
        float scrollAmount = (slotHeight - parentHeight) * direction;
        foreach (var slot in ListOfEquipmentSlots)
        {
            Vector2 newPosition = slot.transform.localPosition;
            newPosition.y += scrollAmount;
            if (newPosition.y > parentHeight / 2 + slotHeight / 2)
            {
                newPosition.y -= parentHeight + slotHeight + spacing;
            }
            else if (newPosition.y < -parentHeight / 2 - slotHeight / 2)
            {
                newPosition.y += parentHeight + slotHeight + spacing;
            }
            slot.transform.localPosition = newPosition;
        }
    }
    public void SetEquipment(Equipment[] equipment)
    {
        // Destroy any existing equipment slots
        if (ListOfEquipmentSlots != null)
        {
            foreach (GameObject slot in ListOfEquipmentSlots)
            {
                DestroyImmediate(slot);
            }
        }
        parentHeight = GetComponent<RectTransform>().rect.height;
        slotHeight = equipmentSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 2;
        int equippedIndex = 0;
        for (int i = 0; i < equipment.Length; i++)
        {
            if (equipment[i].id == MainManager.Instance.game.EquippedFishingRod.Id)
            {
                equippedIndex = i;
                break;
            }
        }

        // Create new equipment slots
        ListOfEquipmentSlots = new GameObject[equipment.Length];
        for (int i = 0; i < equipment.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + equippedIndex * (slotHeight + spacing);
            Debug.Log(yPosition);
            GameObject newEquipmentSlot = Instantiate(equipmentSlot, transform);
            newEquipmentSlot.transform.localScale = new Vector2(0.5f, 0.5f);
            newEquipmentSlot.transform.localPosition = new Vector2(0, yPosition);
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
        TextMeshProUGUI textComponent = newEquipmentSlot.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = equipment[i].name;
        }
    }
    public void SetWheelFocus(bool focus)
    {
        wheelIsFocused = focus;
    }
}


