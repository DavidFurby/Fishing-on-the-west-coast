using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class handles the fishing controls for the player.
public class FishingController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private BaitCamera baitCamera;
    [SerializeField] private CatchSummary catchSummary;
    [SerializeField] private FishingMiniGame fishingMiniGame;
    [SerializeField] private FishingRodLogic fishingRodLogic;
    #endregion
    public FishingStateMachine stateMachine = new();

    public UnityEvent onCatchFish;
    public UnityEvent onCharge;
    public UnityEvent onChargeRelease;
    public UnityEvent onLoseCatch;
    public UnityEvent onInspectFish;

    #region Unity Methods
    private void Start()
    {
        stateMachine.SetState(new StandBy());

    }

    // Update is called once per frame
    void Update()
    {
        ReelInBait();
        StartFishing();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    private void ReelInBait()
    {
        if (Input.GetKeyDown(KeyCode.Space) && stateMachine.GetCurrentState() is Fishing)
        {
            if (catchArea.IsInCatchArea && catchArea.fish != null)
            {
                StartCoroutine(CatchAlert());
                fishingMiniGame.StartBalanceMiniGame(stateMachine.GetCurrentState().totalFishes);
                onCatchFish.Invoke();
                stateMachine.SetState(new ReelingFish());
            }
            else
            {
                fishingRodLogic.ReelInSpeed();
                stateMachine.SetState(new Reeling());
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
    private void StartFishing()
    {
        if (stateMachine.GetCurrentState() is StandBy || stateMachine.GetCurrentState() is Charging)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                stateMachine.SetState(new Charging());
                onCharge.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                onChargeRelease.Invoke();
                WaitForSwingAnimation();
            }
        }
    }

    /// <summary>
    /// Waits for the swing animation to finish before changing the fishing status.
    /// </summary>
    void WaitForSwingAnimation()
    {
        StartCoroutine(fishingRodLogic.SwingAnimation(fishingMiniGame.castPower, stateMachine.SetState));
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (stateMachine.GetCurrentState().totalFishes.Count > 0)
        {
            stateMachine.SetState(new InspectFish());
            onInspectFish.Invoke();
        }
    }

    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        if (stateMachine.GetCurrentState() is ReelingFish)
        {
            onLoseCatch.Invoke();
            stateMachine.SetState(new Reeling());
        }
    }
}
#endregion