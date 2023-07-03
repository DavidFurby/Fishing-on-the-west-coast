using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    [SerializeField] private RectTransform catchSummary;
    [SerializeField] private TextMeshProUGUI catchName;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI isNew;
    [SerializeField] private TextMeshProUGUI newRecord;

    [SerializeField] private FishingController fishingControls;
    [SerializeField] private CatchSummaryHandlers handlers;
    private List<Catch> caughtFishes;
    private Catch currentlyPresentedFish;
    private int fishIndex;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fishingControls.fishingStatus == FishingController.FishingStatus.InspectFish)
        {
            NextSummary();
        }
    }

    // Initialize the catch summary with a list of caught fishes
    public void InitiateCatchSummary(List<Catch> fishes)
    {
        if (fishes.Count == 0)
        {
            Debug.LogError("Fish list is empty");
            return;
        }
        caughtFishes = fishes;
        currentlyPresentedFish = fishes[0];
        handlers.StartSummar(currentlyPresentedFish);
    }

    // If there are more fishes, switch summary. Otherwise, end the summary
    public void NextSummary()
    {
        if (fishIndex < caughtFishes.Count - 1)
        {
            Destroy(currentlyPresentedFish);
            IncrementFishIndex();
            CheckSizeDifference(currentlyPresentedFish);
            CheckIfNew(currentlyPresentedFish);
            MainManager.Instance.game.Fishes++;
            handlers.StartSummar(currentlyPresentedFish);
        }
        else
        {
            EndSummary();
        }
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        fishIndex++;
        currentlyPresentedFish = caughtFishes[fishIndex];
    }

    // Check if fish is larger than the saved fish of the same name
    private void CheckSizeDifference(Catch fishData)
    {
        if (fishData == null)
        {
            Debug.LogError("Fish data is null");
            return;
        }

        Catch existingFish = MainManager.Instance.game.Catches.FirstOrDefault(f => f.name == fishData.name && f.Size < fishData.Size);
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

    // Check if fish hasn't been caught before
    private void CheckIfNew(Catch fishData)
    {
        if (fishData == null)
        {
            Debug.LogError("Fish data is null");
            return;
        }

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

    // Reset data and set fishingStatus to Standby to end summary
    private void EndSummary()
    {
        SetSummaryActive();
        ResetValues();
        handlers.EndSummary();
        fishingControls.SetFishingStatus(FishingController.FishingStatus.StandBy);
        Debug.Log(fishingControls.fishingStatus);
    }

    public void SetSummaryActive()
    {
        catchSummary.gameObject.SetActive(!catchSummary.gameObject.activeSelf);
    }

    // Reset values
    private void ResetValues()
    {
        caughtFishes.Clear();
        currentlyPresentedFish = null;
        fishIndex = 0;
    }
}