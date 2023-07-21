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
    [HideInInspector] public FishDisplay currentlyInspectedFish;
    private int fishIndex;

    /// <summary>
    /// Initialize the catch summary with a list of caught fishes.
    /// </summary>
    public void InitiateCatchSummary()
    {
        if (system.caughtFishes.Count <= 0)
        {
            Debug.LogError("Fish list is empty");
            return;
        }
        currentlyInspectedFish = system.caughtFishes[0];
        system.fishingCamera.MoveCameraCloserToFish(system.caughtFishes[0]);
        UpdateDataValues();
        dialogHandlers.StartSummary(currentlyInspectedFish.fish);
    }

    /// <summary>
    /// If there are more fishes, switch summary. Otherwise, end the summary.
    /// </summary>
    public void NextSummary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fishIndex < system.caughtFishes.Count - 1)
            {
                IncrementFishIndex();
                UpdateDataValues();
                dialogHandlers.StartSummary(currentlyInspectedFish.fish);
            }
            else
            {
                system.SetState(new Idle(system));
            }
        }
    }

    private void UpdateDataValues()
    {
        newRecord.gameObject.SetActive(CheckSizeDifference(currentlyInspectedFish.fish));
        isNew.gameObject.SetActive(CheckIfNew(currentlyInspectedFish.fish));
        MainManager.Instance.game.TotalCatches++;
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        fishIndex++;
        currentlyInspectedFish = system.caughtFishes[fishIndex];
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
            Debug.Log(existingFish.size + " Existing");
            int index = Array.IndexOf(MainManager.Instance.game.CaughtFishes.ToArray(), existingFish);
            fish.ReplaceFishInInstance(index);
            return true;
        }

        return false;
    }

    // Check if fish hasn't been caught before
    private bool CheckIfNew(Fish fish)
    {
        if (fish == null)
        {
            Debug.LogError("Fish data is null");
            return false;
        }
        if (!MainManager.Instance.game.CaughtFishes.Any(f => f.id == fish.id))
        {
            fish.AddFishToInstance();
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
        system.fishingCamera.MoveCameraToOriginal();
    }

    // Reset values
    private void ResetValues()
    {
        system.ClearCaughtFishes();
        currentlyInspectedFish = null;
        fishIndex = 0;
    }
}
