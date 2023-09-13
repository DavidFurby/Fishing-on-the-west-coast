using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollVertical : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPortTransform;
    [SerializeField] private RectTransform contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup VLG;

    [SerializeField] private RectTransform[] ItemList;
    Vector2 OldVelocity;
    bool isUpdated;

    private Vector2 targetPosition;


    void Start()
    {
        isUpdated = false;
        OldVelocity = Vector2.zero;
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.height / (ItemList[0].rect.height + VLG.spacing));

        for (int i = 0; i < ItemsToAdd; i++)
        {
            RectTransform RT = Instantiate(ItemList[i % ItemList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ItemList.Length - i - 1;
            while (num < 0)
            {
                num += ItemList.Length;
            }
            RectTransform RT = Instantiate(ItemList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
            (0 - (ItemList[0].rect.height + VLG.spacing) * ItemsToAdd),
            contentPanelTransform.localPosition.z);
    }

    void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = OldVelocity;
        }

        if (contentPanelTransform.localPosition.y < 0)
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(0, ItemList.Length * (ItemList[0].rect.height + VLG.spacing), 0);
            isUpdated = true;
        }

        if (contentPanelTransform.localPosition.y > (ItemList.Length * (ItemList[0].rect.height + VLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(0, ItemList.Length * (ItemList[0].rect.height + VLG.spacing), 0);
            isUpdated = true;
        }
        ScrollOnInput();
    }
private IEnumerator ScrollToPosition(Vector2 target)
{
    float elapsedTime = 0f;
    float duration = 0.5f; // Duration of the animation in seconds
    Vector2 startPosition = contentPanelTransform.localPosition;

    while (elapsedTime < duration)
    {
        contentPanelTransform.localPosition = Vector2.Lerp(startPosition, target, elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    // Ensure the final position is set correctly
    contentPanelTransform.localPosition = target;
}

private void ScrollOnInput()
{
    if (scrollRect == null)
    {
        throw new System.Exception("Setup ScrollRectKeyController first!");
    }
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        targetPosition = contentPanelTransform.localPosition + new Vector3(0, 1 * (ItemList[0].rect.height + VLG.spacing), 0);
        StartCoroutine(ScrollToPosition(targetPosition));
    }
    else if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        targetPosition = contentPanelTransform.localPosition + new Vector3(0, -1 * (ItemList[0].rect.height + VLG.spacing), 0);
        StartCoroutine(ScrollToPosition(targetPosition));
    }
     // Force an immediate update of the layout
    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentPanelTransform.transform);
}
}