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
    [SerializeField] private MusicController musicController;
    #endregion

    #region Public Fields
    public float castingPower = 20f;
    public FishingStatus fishingStatus;
    #endregion

    #region Enums
    public enum FishingStatus
    {
        StandBy,
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
            if (catchArea.isInCatchArea && catchArea.fish != null)
            {
                StartCoroutine(CatchAlert());
                catchArea.CatchFish();
                fishingMiniGame.StartFishingMiniGame(catchArea.totalFishes);
                musicController.PlayMiniGameMusic();
                SetFishingStatus(FishingStatus.ReelingFish);
            }
            else
            {
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
        Debug.Log("Alert");
    }

    /// <summary>
    /// Starts fishing if the space key is held down.
    /// </summary>
    private void StartFishing()
    {
        if (fishingStatus == FishingStatus.StandBy)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ChargeCasting();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
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
        SetFishingStatus(FishingStatus.Casting);
    }

    //Trigger methods when fish has been reeled in to inspect fishes
    public void HandleCatch()
    {
        if (catchArea.totalFishes.Count > 0)
        {
            SetFishingStatus(FishingStatus.InspectFish);
            fishingMiniGame.EndFishingMiniGame();
            musicController.StopFishingMiniGameMusic();
            catchSummary.InititateCatchSummary(catchArea.totalFishes);
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
            musicController.StopFishingMiniGameMusic();
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
            castingPower++;
    }
    public void SetFishingStatus(FishingStatus fishingStatus)
    {
        this.fishingStatus = fishingStatus;
    }

    #endregion
}
