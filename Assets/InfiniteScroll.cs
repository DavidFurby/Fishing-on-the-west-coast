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
    private void ScrollOnInput()
    {
        if (scrollRect == null)
        {
            throw new System.Exception("Setup ScrollRectKeyController first!");
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("up");
            contentPanelTransform.localPosition += Vector3.up; 
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("down");
            contentPanelTransform.localPosition  += Vector3.down;
        }
    }
}