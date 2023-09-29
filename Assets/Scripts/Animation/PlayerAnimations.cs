using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private float originalChargingThrowSpeed;

    void Start()
    {
        PlayerEventController.OnStartCharging += OnStartCharging;
        PlayerEventController.OnEnterSwinging += OnChargeRelease;
        PlayerEventController.OnEnterIdle += ResetChargingThrowSpeed;
        originalChargingThrowSpeed = playerAnimator.GetFloat("chargingThrowSpeed");
    }

    void OnDestroy()
    {
        PlayerEventController.OnStartCharging -= OnStartCharging;
        PlayerEventController.OnEnterSwinging -= OnChargeRelease;
        PlayerEventController.OnEnterIdle -= ResetChargingThrowSpeed;
    }

    private void OnStartCharging()
    {
        SetChargingThrowAnimation(true);
    }

    private void OnChargeRelease()
    {
        SetChargingThrowAnimation(false);
    }

    public void SetChargingThrowAnimation(bool active)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("chargingThrow", active);
        }
    }

    public void IncreaseChargingThrowSpeed()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("chargingThrowSpeed", playerAnimator.GetFloat("chargingThrowSpeed") + 0.02f);
        }
    }

    public void ResetChargingThrowSpeed()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("chargingThrowSpeed", originalChargingThrowSpeed);
        }
    }

    public void SetPlayerWalkAnimation(bool active)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("walking", active);
        }
    }
}
