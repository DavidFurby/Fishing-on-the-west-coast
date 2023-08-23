using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private float originalChargingThrowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        FishingSystem.OnStartCharging += SetChargingThrowAnimation;
        FishingSystem.OnChargeRelease += SetChargingThrowAnimation;
        originalChargingThrowSpeed = playerAnimator.GetFloat("chargingThrowSpeed");
    }

    public void SetChargingThrowAnimation(bool active)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chargingThrow", active);
        }
    }

    public void SetChargingThrowSpeed()
    {
        playerAnimator.SetFloat("chargingThrowSpeed", playerAnimator.GetFloat("chargingThrowSpeed") + 0.02f);
    }

    public void ResetChargingThrowSpeed()
    {
        playerAnimator.SetFloat("chargingThrowSpeed", originalChargingThrowSpeed);
    }

    public void SetPlayerWalkAnimation(bool active)
    {
        playerAnimator.SetBool("walking", active);
    }
}
