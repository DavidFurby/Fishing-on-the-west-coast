using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(RodAnimations))]
public class RodLogic : MonoBehaviour
{
    private RodAnimations rodAnimations;
    private AudioSource swingAudio;
    public static event Action OnTriggerSetChargingBalance;

    void OnEnable()
    {
        FishingController.OnChargeRelease += WaitForSwingAnimation;
        FishingController.OnReeling += ReelInSpeed;
        FishingController.OnChargeRelease += PlaySwingAudio;
        CatchArea.OnCatchWhileReeling += CalculateReelInSpeed;

    }
    void Start()
    {
        swingAudio = GetComponent<AudioSource>();
        rodAnimations = GetComponent<RodAnimations>();
    }

    void OnDisable()
    {
        FishingController.OnChargeRelease -= WaitForSwingAnimation;
        FishingController.OnReeling -= ReelInSpeed;
        FishingController.OnChargeRelease -= PlaySwingAudio;
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
            FishingController.Instance.reelInSpeed = (FishingController.Instance.initialReelInSpeed * MainManager.Instance.PlayerLevel.ReelingSpeedModifier()) - (@catch.fish.size / 10);
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
        FishingController.Instance.castingPower *= FishingController.Instance.chargeLevel * MainManager.Instance.PlayerLevel.ThrowRangeModifier();
        while (!rodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (rodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        FishingController.Instance.SetState(new Casting(FishingController.Instance));
    }

    // Play the reverse swing animation
    public void PlayerReverseSwingAnimation()
    {
        rodAnimations.PlayReversSwingAnimation();
    }

    public void WaitForSwingAnimation()
    {
        StartCoroutine(SwingAnimation());
    }
    private void PlaySwingAudio()
    {
        swingAudio.Play();
    }
}
