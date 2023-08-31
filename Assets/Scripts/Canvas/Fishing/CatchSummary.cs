using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CatchSummary : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private TextMeshProUGUI isNewText;
    [SerializeField] private TextMeshProUGUI newRecordText;
    [HideInInspector] public FishDisplay currentlyDisplayedFish;
    [SerializeField] private LevelSlider levelSlider;
    #endregion
    #region Private Fields
    private CatchSummaryHandlers summaryDialogHandlers;

    private int currentFishIndex = 0;
    #endregion

    #region Unity Methods
    void OnEnable()
    {
        FishingController.OnStartInspecting += InitiateCatchSummary;
        FishingController.OnNextSummary += NextSummary;
        FishingController.OnEndSummary += EndSummary;
    }
    void Start()
    {
        summaryDialogHandlers = GetComponent<CatchSummaryHandlers>();
        isNewText.gameObject.SetActive(false);
        newRecordText.gameObject.SetActive(false);
    }
    void OnDisable()
    {
        FishingController.OnStartInspecting -= InitiateCatchSummary;
        FishingController.OnNextSummary -= NextSummary;
        FishingController.OnEndSummary -= EndSummary;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Initialize the catch summary with a list of caught fishes.
    /// </summary>
    public void InitiateCatchSummary()
    {
        if (FishingController.Instance.fishesOnHook.Count <= 0)
        {
            Debug.LogError($"{nameof(FishingController.Instance.fishesOnHook)} is empty");
            return;
        }
        currentlyDisplayedFish = FishingController.Instance.fishesOnHook[currentFishIndex];
        UpdateDataValues();
        summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
    }

    /// <summary>
    /// If there are more fishes, switch summary. Otherwise, end the summary.
    /// </summary>
    public void NextSummary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentFishIndex < FishingController.Instance.fishesOnHook.Count - 1)
            {
                IncrementFishIndex();
                UpdateDataValues();
                summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
            }
            else
            {
                FishingController.Instance.SetState(new FishingIdle(FishingController.Instance));
            }
        }
    }

    public void EndSummary()
    {
        ResetValues();
        summaryDialogHandlers.EndSummary();
    }
    #endregion

    #region Private Methods
    // Update the UI values and the game data
    private void UpdateDataValues()
    {
        newRecordText.gameObject.SetActive(IsNewRecord(currentlyDisplayedFish.fish));
        isNewText.gameObject.SetActive(IsNewCatch(currentlyDisplayedFish.fish));
        MainManager.Instance.TotalCatches++;
        MainManager.Instance.playerLevel.AddExp(currentlyDisplayedFish.fish.exp);
        levelSlider.SetLevel();
    }

    // Increment the fish index and update the currently presented fish
    private void IncrementFishIndex()
    {
        currentFishIndex++;
        currentlyDisplayedFish = FishingController.Instance.fishesOnHook[currentFishIndex];
    }

    // Check if fish is larger than the saved fish of the same name and return true or false
    private bool IsNewRecord(Fish fish)
    {
        if (MainManager.Instance.CaughtFishes.FirstOrDefault(f => f.id == fish.id && f.size < fish.size) is Fish existingFish)
        {
            int index = Array.IndexOf(MainManager.Instance.CaughtFishes.ToArray(), existingFish);
            fish.ReplaceFishInInstance(index);
            return true;
        }

        return false;
    }

    // Check if fish hasn't been caught before and return true or false
    private bool IsNewCatch(Fish fish)
    {
        if (MainManager.Instance.CaughtFishes.All(f => f.id != fish.id))
        {
            fish.AddFishToInstance();
            return true;
        }

        return false;
    }

    // Reset values
    private void ResetValues()
    {
        currentlyDisplayedFish = null;
        currentFishIndex = 0;
    }
    #endregion
}
