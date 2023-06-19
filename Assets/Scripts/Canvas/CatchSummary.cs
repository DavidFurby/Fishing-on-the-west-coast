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
    private List<Fish> fishes;
    private Fish currentlyPresentedFish;
    private int fishIndex;
    [SerializeField] private FishingControlls fishingControlls;

    public void InititateCatchSummary(List<Fish> fishes)
    {
        this.fishes = fishes;
        currentlyPresentedFish = fishes[0];
        SetSummaryActive();
        PresentSummary();
    }

    public void PresentSummary()
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
        if (fishIndex < fishes.Count - 1)
        {
            SetSummaryActive();
            currentlyPresentedFish = fishes[fishIndex++];
            SetSummaryActive();
            PresentSummary();
        }
        else
        {
            EndSummary();
        }
    }
    public void AddFishToCount()
    {
        MainManager.Instance.game.Fishes++;
    }



    //Check if fish is larger than the saved fish of the same name
    private void CheckSizeDifference(Fish fishData)
    {
        Fish fish = MainManager.Instance.game.Catches.FirstOrDefault(f => f.name == fishData.name && f.Size < fishData.Size);
        if (fish != null)
        {
            newRecord.gameObject.SetActive(true);
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
        fishes.Clear();
        currentlyPresentedFish = null;
        fishIndex = 0;
    }
}