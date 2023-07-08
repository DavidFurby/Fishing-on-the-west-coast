using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class ItemWheel : MonoBehaviour
{
    [SerializeField] private ItemSlot ItemSlot;
    [SerializeField] public ItemTag itemTag;
    private ItemSlot[] ListOfEquipmentSlots;
    private bool wheelIsFocused;
    float parentHeight;
    float slotHeight;
    float spacing;
    int middleIndex;

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
    public void ChangeEquipedItem()
    {
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            if (i == middleIndex)
            {
                MainManager.Instance.game.SetEquipment(ListOfEquipmentSlots[i].Id, itemTag);
            }
        }
    }

    private void ScrollList(int direction)
    {
        // Rotate the order of the equipment slots in the array
        if (direction == 1)
        {
            var firstSlot = ListOfEquipmentSlots[0];
            for (int i = 0; i < ListOfEquipmentSlots.Length - 1; i++)
            {
                ListOfEquipmentSlots[i] = ListOfEquipmentSlots[i + 1];
            }
            ListOfEquipmentSlots[^1] = firstSlot;
        }
        else if (direction == -1)
        {
            var lastSlot = ListOfEquipmentSlots[^1];
            for (int i = ListOfEquipmentSlots.Length - 1; i > 0; i--)
            {
                ListOfEquipmentSlots[i] = ListOfEquipmentSlots[i - 1];
            }
            ListOfEquipmentSlots[0] = lastSlot;
        }
        middleIndex = ListOfEquipmentSlots.Length / 2;

        // Update the position of the equipment slots
        for (int i = 0; i < ListOfEquipmentSlots.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            Vector2 newPosition = new(0, yPosition);
            ListOfEquipmentSlots[i].transform.localPosition = newPosition;
        }
    }

    public void SetEquipment(Item[] item)
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
        slotHeight = ItemSlot.GetComponent<RectTransform>().rect.height;
        spacing = (parentHeight - 3 * slotHeight) / 2;

        middleIndex = item.Length / 2;

        // Create new equipment slots
        ListOfEquipmentSlots = new ItemSlot[item.Length];
        for (int i = 0; i < item.Length; i++)
        {
            float yPosition = parentHeight / 2 - spacing - slotHeight / 2 - i * (slotHeight + spacing) + middleIndex * (slotHeight + spacing);
            var newItemSlot = Instantiate(ItemSlot, transform);
            newItemSlot.transform.localScale = new Vector2(0.5f, 0.5f);
            newItemSlot.transform.localPosition = new Vector2 { x = 0, y = yPosition };
            if (newItemSlot.TryGetComponent<Image>(out var slotImage))
            {
                slotImage.color = Random.ColorHSV();
            }
            newItemSlot.Id = item[i].id;
            newItemSlot.ItemTag = item[i].itemTag;
            SetText(i, newItemSlot, item);
            ListOfEquipmentSlots[i] = newItemSlot;
        }
    }

    private void SetText(int i, ItemSlot newItemSlot, Item[] equipment)
    {
        newItemSlot.SetTextField(equipment[i].name);
    }

    public void SetWheelFocus(bool focus)
    {
        wheelIsFocused = focus;
    }
}