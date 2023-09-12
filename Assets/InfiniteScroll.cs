using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;
    public RectTransform[] itemList;

    Vector2 OldVelocity;
    bool isUpdated;

    // Start is called before the first frame update
    void Start()
    {
        isUpdated = false;
        OldVelocity = Vector2.zero;
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemList[0].rect.width + HLG.spacing));
        for (int i = 0; i < ItemsToAdd; i++)
        {
            RectTransform RT = Instantiate(itemList[i % itemList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }
        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = itemList.Length - i - 1;
            while (num < 0)
            {
                num += itemList.Length;
            }
            RectTransform RT = Instantiate(itemList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3((0 - (itemList[0].rect.width + HLG.spacing) * ItemsToAdd),
        contentPanelTransform.localPosition.y,
        contentPanelTransform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = OldVelocity;
        }
        if (contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(itemList.Length * (itemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
        if (contentPanelTransform.localPosition.x < 0 - (itemList.Length * (itemList[0].rect.width + HLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(itemList.Length * (itemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
    }
}
