using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the fishing controls for the player.
public class FishingControlls : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource castSound;
    [SerializeField] private BaitCamera baitCamera;
    [SerializeField] private CatchSummary catchSummary;
    [SerializeField] private FishingMiniGame fishingMiniGame;
    [SerializeField] private Animator playerAnimator;
    private readonly float initialReelInSpeed = 15f;

    #endregion

    #region Public Fields
    public float castingPower;
    public FishingStatus fishingStatus;
    public float reelInSpeed;
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
        reelInSpeed = initialReelInSpeed;
        GameObject playerModel = GameObject.FindWithTag("PlayerModel");

        // Check if the player model was found
        if (playerModel != null)
        {
            // Get the Animator component attached to the player model
            playerAnimator = playerModel.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("(*`n*) - where the FUCK is GUBBEN's model!?");
        }

        fishingStatus = FishingStatus.StandBy;

    }

    // Update is called once per frame
    void Update()
    {
        ReelInBait();
        StartFishing();
        EndCatch();

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
                fishingMiniGame.StartFishingMiniGame(catchArea.totalFishes);
                CalculateReelInSpeed();
                SetFishingStatus(FishingStatus.ReelingFish);
            }
            else
            {
                reelInSpeed = 50f;
                SetFishingStatus(FishingStatus.Reeling);
            }

        }
    }


    public void CalculateReelInSpeed()
    {
        for (int i = 0; i < catchArea.totalFishes.Count; i++)
        {
            reelInSpeed = initialReelInSpeed - (catchArea.totalFishes[i].Size / 10);
        }
        Debug.Log(reelInSpeed);
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
            SetFishingStatus(FishingStatus.Charging);
            if (Input.GetKey(KeyCode.Space))
            {
                playerAnimator.SetBool("chargingThrow", true);
                ChargeCasting();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                playerAnimator.SetBool("chargingThrow", false);
                animator.Play("Reverse Swing");
                castSound.Play();
                StartCoroutine(WaitForSwingAnimation());
            }
        }
    }

    /// <summary>
    /// Waits for the swing animation to finish before changing the fishing status.
    /// </summary>
    IEnumerator WaitForSwingAnimation()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        castingPower *= fishingMiniGame.chargeRate;
        fishingMiniGame.SetChargingBalance(false);
        SetFishingStatus(FishingStatus.Casting);
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (catchArea.totalFishes.Count > 0 && fishingStatus == FishingStatus.StandBy)
        {
            SetFishingStatus(FishingStatus.InspectFish);
            fishingMiniGame.EndFishingMiniGame();
            catchSummary.InitiateCatchSummary(catchArea.totalFishes);
        }
    }

    //Triggre functions to continue fishing after a fish has been collected
    public void EndCatch()
    {
        if (fishingStatus == FishingStatus.InspectFish && Input.GetKeyDown(KeyCode.Space))
        {
            catchSummary.NextSummary();
        }

    }


    //Drop the fish if you fail the minigame
    public void LoseCatch()
    {
        if (fishingStatus == FishingStatus.ReelingFish)
        {
            fishingMiniGame.EndFishingMiniGame();
            catchArea.RemoveCatch();
            SetFishingStatus(FishingStatus.Reeling);
        }

    }

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>

    private void ChargeCasting()
    {
        animator.Play("Swing");
        if (castingPower < 200)
        {
            castingPower++;
            playerAnimator.SetFloat("chargingThrowSpeed", playerAnimator.GetFloat("chargingThrowSpeed") + 0.01f);
        }
        else
        {
            fishingMiniGame.SetChargingBalance(true);
        }
    }
    public void SetFishingStatus(FishingStatus fishingStatus)
    {
        this.fishingStatus = fishingStatus;
    }
    public void ResetValues()
    {
        castingPower = 0;
        reelInSpeed = initialReelInSpeed;
    }

    #endregion
}
