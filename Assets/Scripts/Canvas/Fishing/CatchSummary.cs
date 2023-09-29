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

    public void InitiateCatchSummary()
    {
        if (FishingController.Instance.fishesOnHook.Count <= 0)
        {
            Debug.LogError($"{nameof(FishingController.Instance.fishesOnHook)} is empty");
            return;
        }
        SetCurrentlyDisplayedFish();
        UpdateDataValues();
        summaryDialogHandlers.StartSummary(currentlyDisplayedFish.fish);
    }

    public void NextSummary()
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
            else
            {
                FishingController.Instance.SetState(new FishingIdle());
            }
        }
    }

    public void EndSummary()
    {
        ResetValues();
        summaryDialogHandlers.EndSummary();
    }

    private void SubscribeToEvents()
    {
        FishingEventController.OnEnterInspecting += InitiateCatchSummary;
        FishingEventController.OnNextSummary += NextSummary;
        FishingEventController.OnEndSummary += EndSummary;
    }

    private void Initialize()
    {
        summaryDialogHandlers = GetComponent<CatchSummaryHandlers>();
        isNewText.gameObject.SetActive(false);
        newRecordText.gameObject.SetActive(false);
    }

    private void UnsubscribeFromEvents()
    {
        FishingEventController.OnEnterInspecting -= InitiateCatchSummary;
        FishingEventController.OnNextSummary -= NextSummary;
        FishingEventController.OnEndSummary -= EndSummary;
    }

    private void SetCurrentlyDisplayedFish()
    {
        currentlyDisplayedFish = FishingController.Instance.fishesOnHook[currentFishIndex];
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
        return currentFishIndex < FishingController.Instance.fishesOnHook.Count - 1;
    }

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
    private bool IsNewCatch(Fish fish)
    {
        if (MainManager.Instance.CaughtFishes.All(f => f.id != fish.id))
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
