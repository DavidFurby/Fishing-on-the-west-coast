using UnityEngine;

public class FishingRodAnimations : MonoBehaviour
{
    private Animator fishingRodAnimator;
    [SerializeField] FishingMiniGame miniGame;

    private void Start()
    {
        fishingRodAnimator = GetComponent<Animator>();
    }

    public void TriggerSetChargingBalance()
    {
        miniGame.SetChargingBalance(true);
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
