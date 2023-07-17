using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class handles the fishing controls for the player.
public class FishingController : FishingStateMachine
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private BaitCamera baitCamera;
    [SerializeField] private CatchSummary catchSummary;
    [SerializeField] private FishingMiniGame fishingMiniGame;
    [SerializeField] private FishingRodLogic fishingRodLogic;
    #endregion
    public UnityEvent onCatchFish;
    public UnityEvent onCharge;
    public UnityEvent onChargeRelease;
    public UnityEvent onLoseCatch;
    public UnityEvent onInspectFish;

    private void Start()
    {
        SetState(new Idle(this));
    }
    #region Unity Methods
    #endregion

    #region Private Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    public void StartReeling()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchArea.IsInCatchArea && catchArea.fish != null)
            {
                StartCoroutine(CatchAlert());
                fishingMiniGame.StartBalanceMiniGame(GetCurrentState().totalFishes);
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

    public IEnumerator CatchAlert()
    {
        baitCamera.CatchAlertSound();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
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

    /// <summary>
    /// Waits for the swing animation to finish before changing the fishing status.
    /// </summary>
    public void WaitForSwingAnimation()
    {
        StartCoroutine(fishingRodLogic.SwingAnimation(fishingMiniGame.castPower, SetState));
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (GetCurrentState().totalFishes.Count > 0)
        {
            onInspectFish.Invoke();
            SetState(new InspectFish(this));
        }
    }

    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        if (GetCurrentState() is ReelingFish)
        {
            onLoseCatch.Invoke();
            SetState(new Reeling(this));
        }
    }
}
#endregion