using System;
using System.Linq;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(CatchSummaryHandlers))]
public class CatchSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI isNewText;
    [SerializeField] private TextMeshProUGUI newRecordText;
    [HideInInspector] public FishDisplay currentlyDisplayedFish;
    private CatchSummaryHandlers summaryDialogHandlers;
    private int currentFishIndex = 0;

    void OnEnable()
    {
        SubscribeToEvents();
    }

    void Start()
    {
        Initialize();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitiateCatchSummary()
    {
        if (PlayerManager.Instance.fishingController.fishesOnHook.Count <= 0)
        {
            Debug.LogError($"{nameof(PlayerManager.Instance.fishingController.fishesOnHook)} is empty");
            return;
        }
        SetCurrentlyDisplayedFish();
        UpdateDataValues();
        summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
    }

    private void NextSummary()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HasMoreFishes())
            {
                IncrementFishIndex();
                SetCurrentlyDisplayedFish();
                UpdateDataValues();
                summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
            }
        }

        else if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerManager.Instance.SetState(new FishingIdle());
        }

    }

    private void SubscribeToEvents()
    {
        PlayerEventController.OnEnterSummary += InitiateCatchSummary;
        PlayerEventController.OnNextSummary += NextSummary;
        PlayerEventController.OnEndSummary += ResetValues;
    }

    private void Initialize()
    {
        summaryDialogHandlers = GetComponent<CatchSummaryHandlers>();
        isNewText.gameObject.SetActive(false);
        newRecordText.gameObject.SetActive(false);
    }

    private void UnsubscribeFromEvents()
    {
        PlayerEventController.OnEnterSummary -= InitiateCatchSummary;
        PlayerEventController.OnNextSummary -= NextSummary;
        PlayerEventController.OnEndSummary -= ResetValues;
    }

    private void SetCurrentlyDisplayedFish()
    {
        currentlyDisplayedFish = PlayerManager.Instance.fishingController.fishesOnHook[currentFishIndex];
    }

    private void UpdateDataValues()
    {
        newRecordText.gameObject.SetActive(IsNewRecord(currentlyDisplayedFish.fish));
        isNewText.gameObject.SetActive(IsNewCatch(currentlyDisplayedFish.fish));
        MainManager.Instance.TotalCatches++;
        MainManager.Instance.PlayerLevel.AddExp(currentlyDisplayedFish.fish.exp);
    }

    private void IncrementFishIndex()
    {
        currentFishIndex++;
    }

    private bool HasMoreFishes()
    {
        return currentFishIndex < PlayerManager.Instance.fishingController.fishesOnHook.Count - 1;
    }

    private bool IsNewRecord(Fish fish)
    {
        if (MainManager.Instance.CaughtFishes.FirstOrDefault(f => f.fishId == fish.fishId && f.size < fish.size) is Fish existingFish)
        {
            int index = Array.IndexOf(MainManager.Instance.CaughtFishes.ToArray(), existingFish);
            fish.ReplaceFishInInstance(index);
            return true;
        }
        return false;
    }
    private bool IsNewCatch(Fish fish)
    {
        if (MainManager.Instance.CaughtFishes.All(f => f.fishId != fish.fishId))
        {
            fish.AddFishToInstance();
            return true;
        }
        return false;
    }

    private void ResetValues()
    {
        currentlyDisplayedFish = null;
        currentFishIndex = 0;
    }
}
