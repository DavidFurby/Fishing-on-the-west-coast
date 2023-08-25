using System;
using System.Collections;
using UnityEngine;
using static FishingController;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private FishingController fishingSystem;
    [HideInInspector] public float reelInSpeed;
    [HideInInspector] public float castingPower;
    private readonly float initialCastingPower = 20;
    private readonly float initialReelInSpeed = 15f;
    public static event Action OnTriggerSetChargingBalance;


    void Start()
    {
        FishingController.OnChargeRelease += WaitForSwingAnimation;
    }

    void OnDestroy()
    {
        FishingController.OnChargeRelease -= WaitForSwingAnimation;

    }
    void OnDisable()
    {
        FishingController.OnChargeRelease -= WaitForSwingAnimation;
    }
    public void TriggerSetChargingBalance()
    {
        OnTriggerSetChargingBalance.Invoke();
        fishingSystem.fishingMiniGame.SetChargingBalance(true);
    }
    // Calculate the reel in speed based on the size of the caught fishes
    public void CalculateReelInSpeed()
    {
        foreach (FishDisplay @catch in fishingSystem.fishesOnHook)
        {
            reelInSpeed = (initialReelInSpeed * MainManager.Instance.game.playerLevel.ReelingSpeedModifier()) - (@catch.fish.size / 10);
        }
    }

    // Set the initial values for the reel in speed and casting power
    public void ResetValues()
    {
        reelInSpeed = initialReelInSpeed;
        castingPower = initialCastingPower;
        playerAnimations.ResetChargingThrowSpeed();
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
    public void ChargeCasting()
    {
        fishingRodAnimations.PlaySwingAnimation();
        if (castingPower < MainManager.Instance.game.Inventory.EquippedFishingRod.throwRange)
        {
            castingPower++;
            playerAnimations.SetChargingThrowSpeed();
        }
    }

    // Play the swing animation and wait for it to finish
    private IEnumerator SwingAnimation()
    {
        castingPower *= fishingSystem.fishingMiniGame.castPower * MainManager.Instance.game.playerLevel.ThrowRangeModifier();
        while (!fishingRodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (fishingRodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        fishingSystem.SetState(new Casting(fishingSystem));
    }

    // Play the reverse swing animation
    public void PlayerReverseSwingAnimation()
    {
        fishingRodAnimations.PlayReversSwingAnimation();
    }

    public void WaitForSwingAnimation()
    {
        StartCoroutine(SwingAnimation());
    }

}