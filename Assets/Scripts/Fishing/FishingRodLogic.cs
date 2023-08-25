using System;
using System.Collections;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private FishingController controller;
    public static event Action OnTriggerSetChargingBalance;
    public static event Action TriggerSetChargingBalanceEvent
    {
        add { OnTriggerSetChargingBalance += value; }
        remove { OnTriggerSetChargingBalance -= value; }
    }

    void OnEnable()
    {
        FishingController.OnChargeRelease += WaitForSwingAnimation;
        FishingController.OnEnterIdle += ResetValues;
        FishingController.OnReeling += ReelInSpeed;
        FishingController.OnWhileCharging += ChargeCasting;
    }

    void OnDisable()
    {
        FishingController.OnChargeRelease -= WaitForSwingAnimation;
        FishingController.OnEnterIdle -= ResetValues;
        FishingController.OnReeling -= ReelInSpeed;
        FishingController.OnWhileCharging -= ChargeCasting;
        OnTriggerSetChargingBalance = null;

    }

    public void TriggerSetChargingBalance()
    {
        OnTriggerSetChargingBalance?.Invoke();
    }

    // Calculate the reel in speed based on the size of the caught fishes
    public void CalculateReelInSpeed()
    {
        foreach (FishDisplay @catch in controller.fishesOnHook)
        {
            controller.reelInSpeed = (controller.initialReelInSpeed * MainManager.Instance.game.playerLevel.ReelingSpeedModifier()) - (@catch.fish.size / 10);
        }
    }

    // Set the initial values for the reel in speed and casting power
    public void ResetValues()
    {
        playerAnimations.ResetChargingThrowSpeed();
    }

    // Set the reel in speed to a fixed value
    public void ReelInSpeed()
    {
        controller.reelInSpeed = 50;
    }

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>
    /// <param name="setChargingThrowSpeed">The action to perform while charging the casting power.</param>
    public void ChargeCasting()
    {
        fishingRodAnimations.PlaySwingAnimation();
        if (controller.castingPower < MainManager.Instance.game.Inventory.EquippedFishingRod.throwRange)
        {
            controller.castingPower++;
            playerAnimations.SetChargingThrowSpeed();
        }
    }

    // Play the swing animation and wait for it to finish
    private IEnumerator SwingAnimation()
    {
        controller.castingPower *= controller.castPower * MainManager.Instance.game.playerLevel.ThrowRangeModifier();
        while (!fishingRodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (fishingRodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        controller.SetState(new Casting(controller));
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
