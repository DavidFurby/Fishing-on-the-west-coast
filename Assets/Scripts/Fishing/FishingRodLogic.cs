using System.Collections;
using UnityEngine;
using static FishingController;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private CatchArea catchArea;
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
    [SerializeField] private FishingMiniGame gameMiniGame;
    [HideInInspector] public float reelInSpeed;
    [HideInInspector] public float castingPower;
    private FishingRod currentFishingRod;
    private readonly float initialCastingPower = 20;
    private readonly float initialReelInSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        currentFishingRod = MainManager.Instance.game.EquippedFishingRod;
        if (currentFishingRod == null)
        {
            Debug.LogError("Current fishing rod is null");
            return;
        }
        SetInitialValues();
    }

    public void TriggerSetChargingBalance()
    {
        gameMiniGame.SetChargingBalance(true);
    }

    // Calculate the reel in speed based on the size of the caught fishes
    public void CalculateReelInSpeed()
    {
        foreach (Catch @catch in catchArea.totalCatches)
        {
            reelInSpeed = initialReelInSpeed - (@catch.Size / 10);
        }
    }

    // Set the initial values for the reel in speed and casting power
    public void SetInitialValues()
    {
        reelInSpeed = initialReelInSpeed;
        castingPower = initialCastingPower;
    }

    // Set the reel in speed to a fixed value
    public void ReelInSpeed()
    {
        reelInSpeed = 50;
    }

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>
    /// <param name="setChargingThrowSpeed">The action to perform while charging the casting power.</param>
    public void ChargeCasting(System.Action setChargingThrowSpeed)
    {
        fishingRodAnimations.PlaySwingAnimation();
        if (castingPower < currentFishingRod.ThrowRange)
        {
            castingPower++;
            setChargingThrowSpeed();
        }
    }

    // Play the swing animation and wait for it to finish
    public IEnumerator SwingAnimation(float castPower, System.Action<FishingController.FishingStatus> setFishingStatus)
    {
        castingPower *= castPower;
        while (!fishingRodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (fishingRodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        setFishingStatus(FishingStatus.Casting);

    }

    // Play the reverse swing animation
    public void PlayerReverseSwingAnimation()
    {
        fishingRodAnimations.PlayReversSwingAnimation();
    }
}