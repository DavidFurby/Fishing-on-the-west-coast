using System;
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

    public void PresentSummary(Fish fishData)
    {
        SetSummaryActive(true);
        catchName.text = fishData.FishName;
        sizeText.text += fishData.Size;

        CheckSizeDifference(fishData);

        CheckIfNew(fishData);
    }



    //Check if fish is larger than the saved fish of the same name
    private void CheckSizeDifference(Fish fishData)
    {
        Fish fish = MainManager.Instance.game.Catches.FirstOrDefault(f => f.name == fishData.name && f.Size < fishData.Size);
        if (fish != null)
        {
            int index = Array.IndexOf(MainManager.Instance.game.Catches, fish);
            fishData.ReplaceFishInInstance(index);
        }
        else
        {
            newRecord.gameObject.SetActive(false);
        }
    }
    //Check if fish hasnt been caught before
    private void CheckIfNew(Fish fishData)
    {
        if (!MainManager.Instance.game.Catches.Any(f => f.Id == fishData.Id))
        {
            fishData.AddFishToInstance();
        }
        else
        {
            isNew.gameObject.SetActive(false);
        }
    }

    public void SetSummaryActive(bool active)
    {
        catchSummary.SetActive(active);
    }
}