using System.Linq;
using TMPro;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    [SerializeField] private GameObject catchSummary;
    [SerializeField] private TextMeshProUGUI catchName;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI isNew;
    [SerializeField] private TextMeshProUGUI newRecord;

    private void Start()
    {
        SetSummaryActive(false);
    }

    public void PresentSummary(GameObject fish)
    {
        var fishData = fish.GetComponent<Fish>();
        if (fishData != null)
        {
            SetSummaryActive(true);
            catchName.text = fishData.FishName;
            sizeText.text += fishData.Size;
            if (MainManager.Instance.game.Catches.Any(f => f.Size < fishData.Size))
            {
                newRecord.gameObject.SetActive(true);
            }
            else
            {
                newRecord.gameObject.SetActive(false);
            }
            if (!MainManager.Instance.game.Catches.Any(f => f.FishName == fishData.name))
            {
                fishData.AddFishToInstance();
                isNew.gameObject.SetActive(true);
            }
            else
            {
                isNew.gameObject.SetActive(false);
            }
        }
    }

    public void SetSummaryActive(bool active)
    {
        catchSummary.SetActive(active);
    }
}