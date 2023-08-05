using UnityEngine;
using UnityEngine.ProBuilder;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] public Item item;

    protected void SetNewItemModel(Item newItem)
    {

        item = newItem;
    }
}