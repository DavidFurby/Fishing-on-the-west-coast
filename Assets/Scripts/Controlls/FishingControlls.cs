using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles the fishing controls for the player.
/// </summary>
public class FishingControls : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private Bait bait;
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource castSound;
    #endregion

    #region Public Fields
    public float castingPower;
    public FishingStatus fishingStatus;
    #endregion

    #region Enums
    public enum FishingStatus
    {
        StandBy,
        Casting,
        Fishing,
        Reeling
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
        if (fishingStatus == FishingStatus.Fishing)
        {
            ReelInBait();
        }
        else if (fishingStatus == FishingStatus.StandBy)
        {
            StartFishing();
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Reels in the bait if the space key is pressed.
    /// </summary>
    private void ReelInBait()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (catchArea.isInCatchArea)
            {
                catchArea.CatchFish();
            }
            fishingStatus = FishingStatus.Reeling;
        }
    }

    /// <summary>
    /// Starts fishing if the space key is held down.
    /// </summary>
    private void StartFishing()
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
        fishingStatus = FishingStatus.Casting;
    }

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>

    private void ChargeCasting()
    {
        animator.Play("Swing");
        if (castingPower < 100)
            castingPower++;
    }


}
