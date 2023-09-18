using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public abstract class InfiniteScrollVertical<T> : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] internal RectTransform viewPortTransform;
    [SerializeField] internal RectTransform contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup VLG;
    internal T[] itemArray;
    private Vector2 oldVelocity;
    private bool isUpdated;
    private Vector2 targetPosition;

    internal bool scrollEnabled = true;
    private bool isScrolling = false;

    internal float itemHeight;
    internal float itemSpacing;
    internal Transform centeredItem;

    private void Awake()
    {
        InitializeVariables();

    }

    internal IEnumerator InitialSetup()
    {
        int itemsToAdd = CalculateItemsToAdd();
        InstantiateItems(itemsToAdd);
        SetInitialContentPanelPosition();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
        yield return new WaitForEndOfFrame();
        SetCenterContentChild();
    }

    private void LateUpdate()
    {
        if (itemArray != null && itemArray.Length > 1)
        {
            HandleContentPanelPositionUpdate();
            ScrollOnInput();
        }
    }
    private int CalculateItemsToAdd()
    {
        return itemArray.Length;
    }
    private void InitializeVariables()
    {

        itemSpacing = VLG.spacing;
        scrollRect =
        scrollRect != null ? scrollRect : GetComponent<ScrollRect>();
        viewPortTransform = viewPortTransform != null ? viewPortTransform : scrollRect.viewport;
        contentPanelTransform = contentPanelTransform != null ? contentPanelTransform : scrollRect.content;
        VLG = VLG != null ? VLG : contentPanelTransform.GetComponent<VerticalLayoutGroup>();

        isUpdated = false;
        oldVelocity = Vector2.zero;
    }

    private void InstantiateItems(int itemsToAdd)
    {
        if (itemArray.Length <= 1)
        {
            UpdateSlot(contentPanelTransform, itemArray[0]);
            return;
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            UpdateSlot(contentPanelTransform, itemArray[i % itemArray.Length], true);
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = itemArray.Length - i - 1;
            while (num < 0) num += itemArray.Length;
            UpdateSlot(contentPanelTransform, itemArray[num % itemArray.Length], false);
        }
    }


    protected abstract void UpdateSlot(RectTransform parent, T item, bool asLastSibling = false);

    private void SetInitialContentPanelPosition()
    {
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
               -(itemHeight + itemSpacing),
               contentPanelTransform.localPosition.z);
        print(contentPanelTransform.localPosition.y);
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
            UpdateContentPanelPosition(itemArray.Length * (itemHeight + itemSpacing));
        }
        else if (contentPanelTransform.localPosition.y > (itemArray.Length * (itemHeight + itemSpacing)))
        {
            UpdateContentPanelPosition(-itemArray.Length * (itemHeight + itemSpacing));
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
        if (scrollEnabled && !isScrolling)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                targetPosition = contentPanelTransform.localPosition + new Vector3(0, itemHeight + itemSpacing, 0);
                StartCoroutine(ScrollToPosition(targetPosition, SetCenterContentChild));
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                targetPosition = contentPanelTransform.localPosition + new Vector3(0, -(itemHeight + itemSpacing), 0);
                StartCoroutine(ScrollToPosition(targetPosition, SetCenterContentChild));
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanelTransform);
            }
        }
    }

    private IEnumerator ScrollToPosition(Vector2 target, Action onScrollComplete = null)
    {
        isScrolling = true;

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
        onScrollComplete?.Invoke();
        isScrolling = false;
    }
    protected abstract void SetCenterContentChild();




    internal void ClearScroll()
    {
        foreach (Transform child in contentPanelTransform)
        {
            Destroy(child.gameObject);
        }
        contentPanelTransform.localPosition = Vector3.zero;
        itemArray = null;
    }
}
