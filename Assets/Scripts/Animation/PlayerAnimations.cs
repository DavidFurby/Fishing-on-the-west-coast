using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
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
        playerAnimator.SetFloat("chargingThrowSpeed", playerAnimator.GetFloat("chargingThrowSpeed") + 0.01f);
    }

    public void SetPlayerWalkAnimation(bool active)
    {
        playerAnimator.SetBool("walking", active);
    }
}
