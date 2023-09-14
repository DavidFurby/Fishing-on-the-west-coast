using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollVertical : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPortTransform;
    [SerializeField] private RectTransform contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup VLG;

    private RectTransform[] itemList;
    private readonly string itemSlotPath = "GameObjects/Canvas/Components/ItemMenu/ItemSlot";
    private Vector2 oldVelocity;
    private bool isUpdated;
    private Vector2 targetPosition;
    private GameObject itemSlot;

    void Start()
    {

        InitializeItemList();
        int itemsToAdd = CalculateItemsToAdd();
        InstantiateItems(itemsToAdd);
        SetInitialContentPanelPosition(itemsToAdd);

        // Disable scrolling if there is only one item in the list

        scrollRect.enabled = false;

    }

    void Update()
    {
        if (itemList.Length > 1)
        {
            HandleContentPanelPositionUpdate();
            ScrollOnInput();
        }

    }

    private void InitializeItemList()
    {
        itemSlot = Resources.Load<GameObject>(itemSlotPath);
        isUpdated = false;
        oldVelocity = Vector2.zero;
        itemList = new RectTransform[1];
        itemList[0] = itemSlot.GetComponent<RectTransform>();
    }

    private int CalculateItemsToAdd()
    {
        return Mathf.CeilToInt(viewPortTransform.rect.height / (itemList[0].rect.height + VLG.spacing));
    }

    private void InstantiateItems(int itemsToAdd)
    {
        if (itemList.Length <= 1)
        {
            Instantiate(itemList[0], contentPanelTransform);
            return;
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform RT = Instantiate(itemList[i % itemList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = itemList.Length - i - 1;
            while (num < 0) num += itemList.Length;

            RectTransform RT = Instantiate(itemList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }
    }


    private void SetInitialContentPanelPosition(int itemsToAdd)
    {
        int var = itemList.Length > 1 ? itemsToAdd : 1;
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
               (0 - (itemList[0].rect.height + VLG.spacing) * var),
               contentPanelTransform.localPosition.z);
        Debug.Log(contentPanelTransform.localPosition);
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
            UpdateContentPanelPosition(itemList.Length * (itemList[0].rect.height + VLG.spacing));
        }
        else if (contentPanelTransform.localPosition.y > (itemList.Length * (itemList[0].rect.height + VLG.spacing)))
        {
            UpdateContentPanelPosition(-itemList.Length * (itemList[0].rect.height + VLG.spacing));
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
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, itemList[0].rect.height + VLG.spacing, 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetPosition = contentPanelTransform.localPosition + new Vector3(0, -(itemList[0].rect.height + VLG.spacing), 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
        Debug.Log(contentPanelTransform.localPosition);
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
