using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScrollVertical : MonoBehaviour
{
    [SerializeField] internal ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPortTransform;
    [SerializeField] private RectTransform contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup VLG;
    internal ItemSlot[] _itemArray;
    private Vector2 oldVelocity;
    private bool isUpdated;
    private Vector2 targetPosition;
    private readonly string itemSlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private ItemSlot itemSlotPrefab;


    private void Awake()
    {
        itemSlotPrefab = Resources.Load<ItemSlot>(itemSlotPath);

        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        if (viewPortTransform == null)
        {
            viewPortTransform = scrollRect.viewport;
        }

        if (contentPanelTransform == null)
        {
            contentPanelTransform = scrollRect.content;
        }

        if (VLG == null)
        {
            VLG = contentPanelTransform.GetComponent<VerticalLayoutGroup>();
        }
        isUpdated = false;
        oldVelocity = Vector2.zero;
    }

    internal void InitialSetup()
    {
        int itemsToAdd = CalculateItemsToAdd();
        InstantiateItems(itemsToAdd);
        SetInitialContentPanelPosition(itemsToAdd);
    }

    private void LateUpdate()
    {
        if (_itemArray != null && _itemArray.Length > 1)
        {
            HandleContentPanelPositionUpdate();
            ScrollOnInput();
        }
    }

    private int CalculateItemsToAdd()
    {
        return Mathf.CeilToInt(viewPortTransform.rect.height / (itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing));
    }

    private void InstantiateItems(int itemsToAdd)
    {

        if (_itemArray.Length <= 1)
        {
            ItemSlot newSlot = Instantiate(itemSlotPrefab, contentPanelTransform);
            newSlot.GetComponent<ItemSlot>().SetTextField(_itemArray[0].ItemName);
            return;
        }
        for (int i = 0; i < itemsToAdd; i++)
        {
            ItemSlot slot = Instantiate(itemSlotPrefab, contentPanelTransform);
            slot.SetTextField(_itemArray[0].ItemName);
            slot.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        }
        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = _itemArray.Length - i - 1;
            while (num < 0) num += _itemArray.Length;
            ItemSlot slot = Instantiate(itemSlotPrefab, contentPanelTransform);
            slot.SetTextField(_itemArray[0].ItemName);
            slot.gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
        }
    }

    private void SetInitialContentPanelPosition(int itemsToAdd)
    {
        int var = _itemArray.Length > 1 ? itemsToAdd : 1;
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
               0 - (itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing) * var,
               contentPanelTransform.localPosition.z);
    }


    private void HandleContentPanelPositionUpdate()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }
        float itemHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing;
        float maxY = _itemArray.Length * itemHeight;
        float y = Mathf.Clamp(contentPanelTransform.localPosition.y, 0, maxY);
        if (y != contentPanelTransform.localPosition.y)
        {
            UpdateContentPanelPosition(y - contentPanelTransform.localPosition.y);
        }
    }

    private void UpdateContentPanelPosition(float deltaY)
    {
        Canvas.ForceUpdateCanvases();
        oldVelocity = scrollRect.velocity;
        contentPanelTransform.SetPositionAndRotation(contentPanelTransform.localPosition + new Vector3(0, deltaY, 0), contentPanelTransform.localRotation);
        isUpdated = true;
    }
    private void ScrollOnInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing, 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, -(itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing), 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
    }


    private IEnumerator ScrollToPosition(Vector2 target)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector2 startPosition = contentPanelTransform.localPosition;
        while (elapsedTime < duration)
        {
            contentPanelTransform.localPosition = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        contentPanelTransform.localPosition = target;

    }
}
