using System;
using System.Collections.Generic;
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
    private List<Fish> caughtFishes;
    private Fish currentlyPresentedFish;
    private int fishIndex;
    [SerializeField] private FishingControlls fishingControlls;

    public void InitiateCatchSummary(List<Fish> fishes)
    {
        caughtFishes = fishes;
        currentlyPresentedFish = fishes[0];
        SetSummaryActive();
        PresentSummary();
    }

    private void PresentSummary()
    {
        catchName.text = currentlyPresentedFish.FishName;
        sizeText.text = currentlyPresentedFish.Size.ToString("f2");
        CheckSizeDifference(currentlyPresentedFish);
        CheckIfNew(currentlyPresentedFish);
        AddFishToCount();
    }

    //If there are more fishes switch summary. otherwise end the summary
    public void NextSummary()
    {
        if (fishIndex < caughtFishes.Count - 1)
        {
            fishIndex++;
            currentlyPresentedFish = caughtFishes[fishIndex];
            PresentSummary();
        }
        else
        {
            EndSummary();
        }
    }

    private void AddFishToCount()
    {
        MainManager.Instance.game.Fishes++;
    }

    // Check if fish is larger than the saved fish of the same name
    private void CheckSizeDifference(Fish fishData)
    {
        if (fishData != null)
        {
            var existingFish = MainManager.Instance.game.Catches.FirstOrDefault(f => f.name == fishData.name && f.Size < fishData.Size);
            if (existingFish != null)
            {
                newRecord.gameObject.SetActive(true);
                int index = Array.IndexOf(MainManager.Instance.game.Catches, existingFish);
                fishData.ReplaceFishInInstance(index);
            }
            else
            {
                newRecord.gameObject.SetActive(false);
            }
        }

    }

    //Check if fish hasn't been caught before
    private void CheckIfNew(Fish fishData)
    {
        if (!MainManager.Instance.game.Catches.Any(f => f.Id == fishData.Id))
        {
            isNew.gameObject.SetActive(true);
            fishData.AddFishToInstance();
        }
        else
        {
            isNew.gameObject.SetActive(false);
        }
    }

    //Reset data and set fishingStatus to Standby to end summary
    private void EndSummary()
    {
        SetSummaryActive();
        ResetValues();
        fishingControlls.SetFishingStatus(FishingControlls.FishingStatus.StandBy);
    }

    public void SetSummaryActive()
    {
        catchSummary.SetActive(!catchSummary.activeSelf);
    }

    //Reset values
    private void ResetValues()
    {
        caughtFishes.Clear();
        currentlyPresentedFish = null;
        fishIndex = 0;
    }
}