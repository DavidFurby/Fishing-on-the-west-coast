using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the fishing controls for the player.
public class FishingControlls : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource castSound;
    [SerializeField] private BaitCamera baitCamera;
    [SerializeField] private CatchSummary catchSummary;
    private bool pauseControls = false;
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
        if (!pauseControls)
        {
            ReelInBait();
            StartFishing();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            EndCatch();
        }
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
            if (catchArea.isInCatchArea)
            {
                baitCamera.CatchAlert();
                StartCoroutine(StopTimeForOneSecond());
                catchArea.CatchFish();
            }
            SetFishingStatus(FishingStatus.Reeling);

        }
    }
    IEnumerator StopTimeForOneSecond()
    {

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
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

    public void HandleCatch()
    {
        if (catchArea.fish != null)
        {
            SetControlsActive();
            catchSummary.PresentSummary(catchArea.fish);

        }
    }
    public void EndCatch()
    {
        catchSummary.SetSummaryActive(false);
        SetControlsActive();
        catchArea.RemoveCatch();
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

    public void SetControlsActive()
    {
        pauseControls = !pauseControls;
    }
    public void SetFishingStatus(FishingStatus fishingStatus)
    {
        this.fishingStatus = fishingStatus;
    }
    #endregion
}
