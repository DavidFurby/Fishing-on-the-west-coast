using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private float originalChargingThrowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        FishingController.OnStartCharging += OnStartCharging;
        FishingController.OnChargeRelease += OnChargeRelease;
        FishingController.OnEnterIdle += ResetChargingThrowSpeed;
        originalChargingThrowSpeed = playerAnimator.GetFloat("chargingThrowSpeed");
    }

    void OnDestroy()
    {
        FishingController.OnStartCharging -= OnStartCharging;
        FishingController.OnChargeRelease -= OnChargeRelease;
        FishingController.OnEnterIdle -= ResetChargingThrowSpeed;
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
