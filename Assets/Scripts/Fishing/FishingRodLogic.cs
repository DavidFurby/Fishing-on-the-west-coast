using System;
using System.Collections;
using UnityEngine;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
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
        foreach (FishDisplay @catch in FishingController.Instance.fishesOnHook)
        {
            FishingController.Instance.reelInSpeed = (FishingController.Instance.initialReelInSpeed * MainManager.Instance.playerLevel.ReelingSpeedModifier()) - (@catch.fish.size / 10);
        }
    }

    // Set the reel in speed to a fixed value
    public void ReelInSpeed()
    {
        FishingController.Instance.reelInSpeed = 50;
    }



    // Play the swing animation and wait for it to finish
    private IEnumerator SwingAnimation()
    {
        FishingController.Instance.castingPower *= FishingController.Instance.chargeLevel * MainManager.Instance.playerLevel.ThrowRangeModifier();
        while (!fishingRodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (fishingRodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        FishingController.Instance.SetState(new Casting(FishingController.Instance));
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
