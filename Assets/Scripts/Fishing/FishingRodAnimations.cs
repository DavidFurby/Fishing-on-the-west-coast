using UnityEngine;

public class FishingRodAnimations : MonoBehaviour
{
    private Animator fishingRodAnimator;

    private void Start()
    {
        fishingRodAnimator = GetComponent<Animator>();
        FishingController.OnChargeRelease += (value) => PlayReversSwingAnimation();
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
