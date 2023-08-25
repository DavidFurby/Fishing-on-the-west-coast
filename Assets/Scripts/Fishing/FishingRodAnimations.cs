using UnityEngine;

public class FishingRodAnimations : MonoBehaviour
{
    private Animator fishingRodAnimator;

    private void Start()
    {
        fishingRodAnimator = GetComponent<Animator>();

    }
    void OnEnable()
    {
        FishingController.OnWhileCharging += PlaySwingAnimation;
        FishingController.OnChargeRelease += PlayReversSwingAnimation;
    }
    void OnDisable()
    {
        FishingController.OnWhileCharging -= PlaySwingAnimation;
        FishingController.OnChargeRelease -= PlayReversSwingAnimation;
    }
    public void PlaySwingAnimation()
    {
        fishingRodAnimator.Play("Swing");
    }
    public void PlayReversSwingAnimation()
    {
        fishingRodAnimator.Play("Reverse Swing");
    }
    public AnimatorStateInfo GetCurrentAnimationState()
    {
        return fishingRodAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
