using System;
using System.Collections;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
    [SerializeField] private FishingController controller;
    public static event Action OnTriggerSetChargingBalance;

    void OnEnable()
    {
        FishingController.OnChargeRelease += WaitForSwingAnimation;
        FishingController.OnReeling += ReelInSpeed;
        CatchArea.OnCatchWhileReeling += CalculateReelInSpeed;
    }

    void OnDisable()
    {
        FishingController.OnChargeRelease -= WaitForSwingAnimation;
        FishingController.OnReeling -= ReelInSpeed;
        CatchArea.OnCatchWhileReeling -= CalculateReelInSpeed;
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

    // Set the reel in speed to a fixed value
    public void ReelInSpeed()
    {
        controller.reelInSpeed = 50;
    }



    // Play the swing animation and wait for it to finish
    private IEnumerator SwingAnimation()
    {
        controller.castingPower *= controller.chargeLevel * MainManager.Instance.game.playerLevel.ThrowRangeModifier();
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
