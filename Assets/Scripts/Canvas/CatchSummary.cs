using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI isNewText;
    [SerializeField] private TextMeshProUGUI newRecordText;

    [SerializeField] private FishingSystem fishingSystem;
    [SerializeField] private CatchSummaryHandlers summaryDialogHandlers;
    [HideInInspector] public FishDisplay currentlyDisplayedFish;
    private int currentFishIndex = 0;

    // Initialize the catch summary with a list of caught fishes.
    public void InitiateCatchSummary()
    {
        if (fishingSystem == null || summaryDialogHandlers == null)
        {
            Debug.LogError("Missing references");
            return;
        }

        if (fishingSystem.caughtFishes.Count <= 0)
        {
            Debug.LogError("Fish list is empty");
            return;
        }
        currentlyDisplayedFish = fishingSystem.caughtFishes[currentFishIndex];
        UpdateDataValues();
        summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
    }


    // If there are more fishes, switch summary. Otherwise, end the summary.
    public void NextSummary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentFishIndex < fishingSystem.caughtFishes.Count - 1)
            {
                IncrementFishIndex();
                UpdateDataValues();
                summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
            }
            else
            {
                fishingSystem.SetState(new Idle(fishingSystem));
            }
        }
    }

    // Update the UI values and the game data
    private void UpdateDataValues()
    {
        newRecordText.gameObject.SetActive(IsNewRecord(currentlyDisplayedFish.fish));
        isNewText.gameObject.SetActive(IsNewCatch(currentlyDisplayedFish.fish));
        MainManager.Instance.game.TotalCatches++;
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        currentFishIndex++;
        currentlyDisplayedFish = fishingSystem.caughtFishes[currentFishIndex];
    }

    // Check if fish is larger than the saved fish of the same name and return true or false
    private bool IsNewRecord(Fish fish)
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

    // Check if fish hasn't been caught before and return true or false
    private bool IsNewCatch(Fish fish)
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

    // Reset data and set fishingStatus to Standby to end summary.
    public void EndSummary()
    {
        ResetValues();
        summaryDialogHandlers.EndSummary();
        fishingSystem.fishingCamera.MoveCameraToOriginal();
    }

    // Reset values
    private void ResetValues()
    {
        currentlyDisplayedFish = null;
        currentFishIndex = 0;
    }
}
