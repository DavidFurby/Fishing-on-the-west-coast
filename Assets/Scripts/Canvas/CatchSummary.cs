using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI catchName;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI isNew;
    [SerializeField] private TextMeshProUGUI newRecord;

    [SerializeField] private FishingSystem system;
    [SerializeField] private CatchSummaryHandlers dialogHandlers;
    private Fish currentlyInspectedFish;
    private int fishIndex;

    /// <summary>
    /// Initialize the catch summary with a list of caught fishes.
    /// </summary>
    public void InitiateCatchSummary()
    {
        if (system.totalFishes.Count <= 0)
        {
            Debug.LogError("Fish list is empty");
            return;
        }
        currentlyInspectedFish = system.totalFishes[0].fish;
        UpdateDataValues();
        dialogHandlers.StartSummary(currentlyInspectedFish);
    }

    /// <summary>
    /// If there are more fishes, switch summary. Otherwise, end the summary.
    /// </summary>
    public void NextSummary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fishIndex < system.totalFishes.Count - 1)
            {
                IncrementFishIndex();
                UpdateDataValues();
                dialogHandlers.StartSummary(currentlyInspectedFish);
            }
            else
            {
                system.SetState(new Idle(system));
            }
        }
    }

    private void UpdateDataValues()
    {
        newRecord.gameObject.SetActive(CheckSizeDifference(currentlyInspectedFish));
        isNew.gameObject.SetActive(CheckIfNew(currentlyInspectedFish));
        MainManager.Instance.game.TotalCatches++;
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        fishIndex++;
        currentlyInspectedFish = system.totalFishes[fishIndex].fish;
    }

    // Check if fish is larger than the saved fish of the same name
    private bool CheckSizeDifference(Fish fish)
    {
        if (fish == null)
        {
            Debug.LogError("Fish data is null");
            return false;
        }

        Fish existingFish = MainManager.Instance.game.CaughtFishes.FirstOrDefault(f => f.id == fish.id && f.size < fish.size);
        if (existingFish != null)
        {
            int index = Array.IndexOf(MainManager.Instance.game.CaughtFishes.ToArray(), existingFish);
            fish.ReplaceFishInInstance(index);
            return true;
        }

        return false;
    }

    // Check if fish hasn't been caught before
    private bool CheckIfNew(Fish fishData)
    {
        if (fishData == null)
        {
            Debug.LogError("Fish data is null");
            return false;
        }
        if (!MainManager.Instance.game.CaughtFishes.Any(f => f.id == fishData.id))
        {
            fishData.AddFishToInstance();
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
        dialogHandlers.EndSummary();
    }

    // Reset values
    private void ResetValues()
    {
        system.totalFishes.Clear();
        currentlyInspectedFish = null;
        fishIndex = 0;
    }
}
