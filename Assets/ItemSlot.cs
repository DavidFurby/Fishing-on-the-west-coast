using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    private int itemId;
    [SerializeField] private TextMeshProUGUI nameText;

    public int ItemId
    {
        get => itemId;
        set => itemId = value;
    }
    public void SetTextField(string itemName)
    {
        nameText.text = itemName;
    }
}
