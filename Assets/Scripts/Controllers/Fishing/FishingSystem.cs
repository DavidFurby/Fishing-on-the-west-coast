using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishingSystem : FishingStateMachine
{
    #region Serialized Fields
    [Header("Fishing Components")]
    public CatchArea catchArea;
    public FishingCamera fishingCamera;
    public CatchSummary catchSummary;
    public FishingMiniGame fishingMiniGame;
    public FishingRodLogic fishingRodLogic;
    public BaitLogic baitLogic;
    public ItemMenu itemMenu;
    public SeaLogic seaSpawner;
    #endregion

    #region Events
    [Header("Fishing Events")]
    public UnityEvent onCatchFish;
    public UnityEvent onStartCharging;
    public UnityEvent onChargeRelease;
    #endregion

    [HideInInspector] public FishDisplay FishAttachedToBait { get; set; }
    [HideInInspector] public List<FishDisplay> fishesOnHook = new();

    [HideInInspector] public bool IsInCatchArea { get; set; }

    [HideInInspector] public bool IsInBaitArea { get; set; }

    [HideInInspector] public Bait bait;
    [HideInInspector] public FishingRod fishingRod;

    private void Awake()
    {
        SetState(new Idle(this));
        bait = MainManager.Instance.game.Inventory.EquippedBait;
        fishingRod = MainManager.Instance.game.Inventory.EquippedFishingRod;
    }

    #region Public Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsInCatchArea && FishAttachedToBait != null)
            {
                StartCoroutine(fishingCamera.CatchAlert());
                fishingMiniGame.StartBalanceMiniGame();
                onCatchFish.Invoke();
                SetState(new ReelingFish(this));
            }
            else
            {
                fishingRodLogic.ReelInSpeed();
                SetState(new Reeling(this));
            }
        }
    }

    /// <summary>
    /// Starts fishing if the space key is held down.
    /// </summary>
    public void StartCharging()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            onStartCharging.Invoke();
            SetState(new Charging(this));
        }
    }
    public void Charge()
    {
        fishingRodLogic.ChargeCasting();
    }
    public void Release()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetState(new Swinging(this));
            onChargeRelease.Invoke();
            fishingRodLogic.WaitForSwingAnimation();
        }
    }
    public void CatchFish()
    {
        if (GetCurrentState() is Fishing)
        {
            FishAttachedToBait.GetComponent<FishMovement>().SetState(new Hooked(FishAttachedToBait.GetComponent<FishMovement>()));
            AddFish(FishAttachedToBait);
        }
    }
    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (fishesOnHook.Count > 0)
        {
            SetState(new InspectFish(this));
        }
    }

    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        ResetValues();
        SetState(new Reeling(this));
    }

    //Add fish to totalFishes list
    public void AddFish(FishDisplay fish)
    {
        fishesOnHook.Add(fish);
    }

    public void ClearCaughtFishes()
    {
        foreach (var fish in fishesOnHook)
        {
            fish.ReturnToPool();
        }
        fishesOnHook.Clear();
    }
    public void ResetValues()
    {
        ClearCaughtFishes();
        FishAttachedToBait = null;
        IsInCatchArea = false;
        IsInBaitArea = false;
    }
}
#endregion

