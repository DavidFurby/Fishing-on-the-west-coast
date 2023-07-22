using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishingSystem : FishingStateMachine
{
    #region Serialized Fields
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
    public UnityEvent onCatchFish;
    public UnityEvent onCharge;
    public UnityEvent onChargeRelease;
    #endregion

    [HideInInspector] public List<FishDisplay> caughtFishes = new();
    [HideInInspector] public Bait bait;
    [HideInInspector] public FishingRod fishingRod;

    private void Awake()
    {
        SetState(new Idle(this));
        bait = MainManager.Instance.game.EquippedBait;
        fishingRod = MainManager.Instance.game.EquippedFishingRod;
    }

    #region Public Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchArea.IsInCatchArea && catchArea.fish != null)
            {
                StartCoroutine(fishingCamera.CatchAlert());
                fishingMiniGame.StartBalanceMiniGame(caughtFishes);
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
    public void StartFishing()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SetState(new Charging(this));
            onCharge.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            onChargeRelease.Invoke();
            WaitForSwingAnimation();
        }
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (caughtFishes.Count > 0)
        {
            SetState(new InspectFish(this));
        }
    }

    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        catchArea.ResetValues();
        ClearCaughtFishes();
        SetState(new Reeling(this));
    }

    //Add fish to totalFishes list
    public void AddFish(FishDisplay fish)
    {
        caughtFishes.Add(fish);
    }
    public void ClearCaughtFishes()
    {
        for (int i = 0; i < caughtFishes.Count; i++)
        {
            caughtFishes[i].ReturnToPool();
        }
        caughtFishes.Clear();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Waits for the swing animation to finish before changing the fishing status.
    /// </summary>
    private void WaitForSwingAnimation()
    {
        StartCoroutine(fishingRodLogic.SwingAnimation(fishingMiniGame.castPower));
    }
    #endregion
}
