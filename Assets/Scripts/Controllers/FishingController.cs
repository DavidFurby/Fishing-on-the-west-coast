using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the fishing controls for the player.
public class FishingController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private AudioSource castSound;
    [SerializeField] private BaitCamera baitCamera;
    [SerializeField] private CatchSummary catchSummary;
    [SerializeField] private FishingMiniGame fishingMiniGame;
    [SerializeField] private FishingRodLogic fishingRodLogic;
    [SerializeField] private PlayerAnimations playerAnimations;
    #endregion

    #region Public Fields
    [HideInInspector] public FishingStatus fishingStatus;
    #endregion

    #region Enums
    public enum FishingStatus
    {
        StandBy,
        Charging,
        Casting,
        Fishing,
        Reeling,
        ReelingFish,
        InspectFish,
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        fishingStatus = FishingStatus.StandBy;
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
        if (Input.GetKeyDown(KeyCode.Space) && fishingStatus == FishingStatus.Fishing)
        {
            if (catchArea.IsInCatchArea && catchArea.fish != null)
            {
                StartCoroutine(CatchAlert());
                catchArea.CatchFish();
                fishingMiniGame.StartBalanceMiniGame(catchArea.totalCatches);
                fishingRodLogic.CalculateReelInSpeed();
                SetFishingStatus(FishingStatus.ReelingFish);
            }
            else
            {
                fishingRodLogic.ReelInSpeed();
                SetFishingStatus(FishingStatus.Reeling);
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
        if (fishingStatus == FishingStatus.StandBy || fishingStatus == FishingStatus.Charging)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SetFishingStatus(FishingStatus.Charging);
                playerAnimations.SetChargingThrowAnimation(true);
                fishingRodLogic.ChargeCasting(playerAnimations.SetChargingThrowSpeed);

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                fishingMiniGame.SetChargingBalance(false);
                playerAnimations.SetChargingThrowAnimation(false);
                fishingRodLogic.PlayerReverseSwingAnimation();
                castSound.Play();
                WaitForSwingAnimation();
            }
        }
    }

    /// <summary>
    /// Waits for the swing animation to finish before changing the fishing status.
    /// </summary>
    void WaitForSwingAnimation()
    {
        StartCoroutine(fishingRodLogic.SwingAnimation(fishingMiniGame.castPower, SetFishingStatus));
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (catchArea.totalCatches.Count > 0 && fishingStatus == FishingStatus.StandBy)
        {
            SetFishingStatus(FishingStatus.InspectFish);
            fishingMiniGame.EndBalanceMiniGame();
            catchSummary.InitiateCatchSummary(catchArea.totalCatches);
        }
    }


    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        if (fishingStatus == FishingStatus.ReelingFish)
        {
            fishingMiniGame.EndBalanceMiniGame();
            catchArea.RemoveCatch();
            SetFishingStatus(FishingStatus.Reeling);
        }

    }

    public void SetFishingStatus(FishingStatus fishingStatus)
    {
        this.fishingStatus = fishingStatus;
    }
    #endregion
}
