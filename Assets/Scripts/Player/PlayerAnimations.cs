using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private float originalChargingThrowSpeed;
    protected PlayerManager manager;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        originalChargingThrowSpeed = animator.GetFloat("chargingThrowSpeed");
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
        if (animator != null)
        {
            animator.SetBool("chargingThrow", active);
        }
    }

    public void ResetChargingThrowSpeed()
    {
        if (animator != null)
        {
            animator.SetFloat("chargingThrowSpeed", originalChargingThrowSpeed);
        }
    }

    public void SetWalkAnimation(bool active)
    {
        if (animator != null)
        {
            animator.SetBool("walking", active);
        }
    }
}
