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
        PlayerEventController.OnWhileCharging += PlaySwingAnimation;
        PlayerEventController.OnEnterSwinging += PlayReversSwingAnimation;
    }
    void OnDestroy()
    {
        PlayerEventController.OnWhileCharging -= PlaySwingAnimation;
        PlayerEventController.OnEnterSwinging -= PlayReversSwingAnimation;
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
