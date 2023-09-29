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
        PlayerEventController.OnEnterSwinging += WaitForSwingAnimation;
        PlayerEventController.OnEnterReeling += ReelInSpeed;
        PlayerEventController.OnEnterSwinging += PlaySwingAudio;
        CatchArea.OnCatchWhileReeling += CalculateReelInSpeed;

    }
    void Start()
    {
        swingAudio = GetComponent<AudioSource>();
        rodAnimations = GetComponent<RodAnimations>();
    }

    void OnDestroy()
    {
        PlayerEventController.OnEnterSwinging -= WaitForSwingAnimation;
        PlayerEventController.OnEnterReeling -= ReelInSpeed;
        PlayerEventController.OnEnterSwinging -= PlaySwingAudio;
        CatchArea.OnCatchWhileReeling -= CalculateReelInSpeed;

    }

    public void TriggerSetChargingBalance()
    {
        OnTriggerSetChargingBalance?.Invoke();
    }

    public void CalculateReelInSpeed()
    {
        foreach (FishDisplay @catch in PlayerController.Instance.fishesOnHook)
        {
            PlayerController.Instance.reelInSpeed = (PlayerController.Instance.initialReelInSpeed * MainManager.Instance.PlayerLevel.ReelingSpeedModifier()) - (@catch.fish.size / 10);
        }
    }

    public void ReelInSpeed()
    {
        PlayerController.Instance.reelInSpeed = 50;
    }

    private IEnumerator SwingAnimation()
    {
        PlayerController.Instance.castingPower *= PlayerController.Instance.chargeLevel * MainManager.Instance.PlayerLevel.ThrowRangeModifier();
        while (!rodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (rodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        PlayerController.Instance.SetState(new Casting());
    }

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
