using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    private float originalChargingThrowSpeed; 
    protected PlayerManager manager;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }

    void Start()
    {
        originalChargingThrowSpeed = playerAnimator.GetFloat("chargingThrowSpeed");
    }

    internal void OnStartCharging()
    {
        SetChargingThrowAnimation(true);
    }

    internal void OnChargeRelease()
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
