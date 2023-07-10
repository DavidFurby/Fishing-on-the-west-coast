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
    private List<FishDisplay> caughtFishes;
    private FishDisplay currentlyPresentedFish;
    private int fishIndex;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && fishingControls.fishingStatus == FishingController.FishingStatus.InspectFish)
        {
            NextSummary();
        }
    }

    /// <summary>
    /// Initialize the catch summary with a list of caught fishes.
    /// </summary>
    /// <param name="fishes">The list of caught fishes.</param>
    public void InitiateCatchSummary(List<FishDisplay> fishes)
    {
        if (fishes.Count == 0)
        {
            Debug.LogError("Fish list is empty");
            return;
        }
        caughtFishes = fishes;
        currentlyPresentedFish = fishes[0];
        UpdateDataValues();
        handlers.StartSummar(currentlyPresentedFish);
    }

    /// <summary>
    /// If there are more fishes, switch summary. Otherwise, end the summary.
    /// </summary>
    public void NextSummary()
    {
        if (fishIndex < caughtFishes.Count - 1)
        {
            IncrementFishIndex();
            UpdateDataValues();
            handlers.StartSummar(currentlyPresentedFish);
        }
        else
        {
            EndSummary();
        }
    }

    private void UpdateDataValues()
    {
        newRecord.gameObject.SetActive(CheckSizeDifference(currentlyPresentedFish));
        isNew.gameObject.SetActive(CheckIfNew(currentlyPresentedFish));
        MainManager.Instance.game.TotalCatches++;
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        fishIndex++;
        currentlyPresentedFish = caughtFishes[fishIndex];
    }

    // Check if fish is larger than the saved fish of the same name
    private bool CheckSizeDifference(FishDisplay fishData)
    {
        if (fishData == null)
        {
            Debug.LogError("Fish data is null");
            return false;
        }

        Fish existingFish = MainManager.Instance.game.CaughtFishes.FirstOrDefault(f => f.id == fishData.fish.id && f.size < fishData.fish.size);
        if (existingFish != null)
        {
            int index = Array.IndexOf(MainManager.Instance.game.CaughtFishes.ToArray(), existingFish);
            fishData.fish.ReplaceFishInInstance(index);
            return true;
        }

        return false;
    }

    // Check if fish hasn't been caught before
    private bool CheckIfNew(FishDisplay catchData)
    {
        if (catchData == null)
        {
            Debug.LogError("Fish data is null");
            return false;
        }
        if (!MainManager.Instance.game.CaughtFishes.Any(f => f.id == catchData.fish.id))
        {
            catchData.fish.AddFishToInstance();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Reset data and set fishingStatus to Standby to end summary.
    /// </summary>
    public void EndSummary()
    {
        ResetValues();
        handlers.EndSummary();
        fishingControls.SetFishingStatus(FishingController.FishingStatus.StandBy);
        Debug.Log(fishingControls.fishingStatus);
    }

    // Reset values
    private void ResetValues()
    {
        caughtFishes.Clear();
        currentlyPresentedFish = null;
        fishIndex = 0;
    }
}