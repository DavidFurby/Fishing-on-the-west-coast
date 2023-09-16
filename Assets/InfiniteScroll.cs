using System.Collections;
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
    internal bool scrollEnabled = true;
    private float itemSlotHeight;
    private float itemSpacing;
    internal ItemSlot centeredItem;

    private void Awake()
    {
        InitializeVariables();
    }

    internal void InitialSetup()
    {
        int itemsToAdd = CalculateItemsToAdd();
        InstantiateItems(itemsToAdd);
        SetInitialContentPanelPosition(itemsToAdd);
        SetCenterItemIndex();

    }

    private void LateUpdate()
    {
        if (_itemArray != null && _itemArray.Length > 1)
        {
            HandleContentPanelPositionUpdate();
            ScrollOnInput();
        }
    }

    private void InitializeVariables()
    {
        itemSlotPrefab = Resources.Load<ItemSlot>(itemSlotPath);
        itemSlotHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
        itemSpacing = VLG.spacing;

        scrollRect =
        scrollRect != null ? scrollRect : GetComponent<ScrollRect>();
        viewPortTransform = viewPortTransform != null ? viewPortTransform : scrollRect.viewport;
        contentPanelTransform
        = contentPanelTransform
!= null ? contentPanelTransform : scrollRect.content;
        VLG = VLG != null ? VLG : contentPanelTransform.GetComponent<VerticalLayoutGroup>();

        isUpdated = false;
        oldVelocity = Vector2.zero;
    }

    private int CalculateItemsToAdd()
    {
        return Mathf.CeilToInt(viewPortTransform.rect.height / (itemSlotHeight + itemSpacing));
    }

    private void InstantiateItems(int itemsToAdd)
    {
        if (_itemArray.Length <= 1)
        {
            InstantiateItemSlot(contentPanelTransform, _itemArray[0]);
            return;
        }
        for (int i = 0; i < itemsToAdd; i++)
        {
            InstantiateItemSlot(contentPanelTransform, _itemArray[i % _itemArray.Length], true);
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = _itemArray.Length - i - 1;
            while (num < 0) num += _itemArray.Length;
            InstantiateItemSlot(contentPanelTransform, _itemArray[num % _itemArray.Length], false);
        }
    }

    private void InstantiateItemSlot(RectTransform parent, ItemSlot itemSlot, bool asLastSibling = false)
    {
        ItemSlot slot = Instantiate(itemSlotPrefab, parent);
        slot.SetTextField(itemSlot.ItemName);
        slot.Id = itemSlot.Id;
        slot.ItemTag = itemSlot.ItemTag;
        slot.ItemName = itemSlot.ItemName;
        if (asLastSibling) slot.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        else slot.gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
    }

    private void SetInitialContentPanelPosition(int itemsToAdd)
    {
        int var = _itemArray.Length > 1 ? itemsToAdd : 1;
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
               0 - (itemSlotHeight + itemSpacing) * var,
               contentPanelTransform.localPosition.z);
    }

    private void HandleContentPanelPositionUpdate()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }

        if (contentPanelTransform.localPosition.y < 0)
        {
            UpdateContentPanelPosition(_itemArray.Length * (itemSlotHeight + itemSpacing));
        }
        else if (contentPanelTransform.localPosition.y > (_itemArray.Length * (itemSlotHeight + itemSpacing)))
        {
            UpdateContentPanelPosition(-_itemArray.Length * (itemSlotHeight + itemSpacing));
        }
    }

    private void UpdateContentPanelPosition(float deltaY)
    {
        Canvas.ForceUpdateCanvases();
        oldVelocity = scrollRect.velocity;
        contentPanelTransform.localPosition += new Vector3(0, deltaY, 0);
        isUpdated = true;
    }


    private void ScrollOnInput()
    {
        if (scrollEnabled)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                targetPosition = contentPanelTransform.localPosition + new Vector3(0, itemSlotHeight + itemSpacing, 0);
                StartCoroutine(ScrollToPosition(targetPosition));
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
                SetCenterItemIndex();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetPosition = contentPanelTransform.localPosition + new Vector3(0, -(itemSlotHeight + itemSpacing), 0);
                StartCoroutine(ScrollToPosition(targetPosition));
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
                SetCenterItemIndex();
            }


        }
    }

    private IEnumerator ScrollToPosition(Vector2 target)
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector2 startPosition = contentPanelTransform.localPosition;
        while (elapsedTime < duration)
        {
            contentPanelTransform.localPosition = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        contentPanelTransform.localPosition = target;
    }
    private void SetCenterItemIndex()
    {
        float centerPosition = -contentPanelTransform.localPosition.y + viewPortTransform.rect.height / 2;
        int index = Mathf.RoundToInt(centerPosition / (itemSlotHeight + itemSpacing)) % _itemArray.Length;
        if (index < 0) index += _itemArray.Length;
        centeredItem = _itemArray[index];
    }
    internal void ClearScroll()
    {
        foreach (Transform child in contentPanelTransform)
        {
            Destroy(child.gameObject);
        }
        contentPanelTransform.localPosition = Vector3.zero;
        _itemArray = null;
    }

}