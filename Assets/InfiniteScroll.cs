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
    private ItemSlot[] _itemArray;
    private Vector2 oldVelocity;
    private bool isUpdated;
    private Vector2 targetPosition;
    private const string slotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private ItemSlot itemSlotPrefab;

    void Start()
    {
        print("hie");
        itemSlotPrefab = Resources.Load<ItemSlot>(slotPath);
        print(itemSlotPrefab);
        InitialSetup();
        scrollRect.enabled = false;
    }

    private void InitialSetup()
    {
        InitializeItemList();
        int itemsToAdd = CalculateItemsToAdd();
        InstantiateItems(itemsToAdd);
        SetInitialContentPanelPosition(itemsToAdd);
    }

    void Update()
    {
        if (_itemArray != null && _itemArray.Length > 1)
        {
            HandleContentPanelPositionUpdate();
            ScrollOnInput();
        }
    }

    private void InitializeItemList()
    {
        isUpdated = false;
        oldVelocity = Vector2.zero;
    }

    private int CalculateItemsToAdd()
    {
        return Mathf.CeilToInt(viewPortTransform.rect.height / (itemSlotPrefab.GetComponent<RectTransform>().rect.height + VLG.spacing));
    }

    private void InstantiateItems(int itemsToAdd)
    {
        if (_itemArray.Length <= 1)
        {
            Instantiate(_itemArray[0], contentPanelTransform);
            return;
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform RT = Instantiate(_itemArray[i % _itemArray.Length], contentPanelTransform).GetComponent<RectTransform>();
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = _itemArray.Length - i - 1;
            while (num < 0) num += _itemArray.Length;

            RectTransform RT = Instantiate(_itemArray[num], contentPanelTransform).GetComponent<RectTransform>();
            RT.SetAsFirstSibling();
        }
    }

    private void SetInitialContentPanelPosition(int itemsToAdd)
    {
        int var = _itemArray.Length > 1 ? itemsToAdd : 1;
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
               0 - (_itemArray[0].GetComponent<RectTransform>().rect.height + VLG.spacing) * var,
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
        contentPanelTransform.localPosition += new Vector3(0, deltaY, 0);
        isUpdated = true;
    }

    private void ScrollOnInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, _itemArray[0].GetComponent<RectTransform>().rect.height + VLG.spacing, 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, -(_itemArray[0].GetComponent<RectTransform>().rect.height + VLG.spacing), 0);
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

    internal void AddItems(ItemSlot[] items)
    {
        _itemArray = new ItemSlot[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            ItemSlot newItem = Instantiate(itemSlotPrefab);
            newItem.Id = items[i].Id;
            newItem.tag = items[i].tag;
            newItem.NameText.text = items[i].ItemName;
            _itemArray[i] = newItem;
        }
        InitialSetup();
    }

}
