using UnityEngine;

public class RodAnimations : MonoBehaviour
{
    private Animator rodAnimator;

    private void Start()
    {
        rodAnimator = GetComponent<Animator>();

    }
    void OnEnable()
    {
        FishingController.OnWhileCharging += PlaySwingAnimation;
        FishingController.OnChargeRelease += PlayReversSwingAnimation;
    }
    void OnDestroy()
    {
        FishingController.OnWhileCharging -= PlaySwingAnimation;
        FishingController.OnChargeRelease -= PlayReversSwingAnimation;
    }
    public void PlaySwingAnimation()
    {
        rodAnimator.Play("Swing");
    }
    public void PlayReversSwingAnimation()
    {
        rodAnimator.Play("Reverse Swing");
    }
    public AnimatorStateInfo GetCurrentAnimationState()
    {
        return rodAnimator.GetCurrentAnimatorStateInfo(0);
    }
}
