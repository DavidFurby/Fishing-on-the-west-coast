using System.Collections;
using UnityEngine;
using static FishingSystem;

public class FishingRodLogic : MonoBehaviour
{
    [SerializeField] private FishingRodAnimations fishingRodAnimations;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private FishingSystem fishingSystem;
    [HideInInspector] public float reelInSpeed;
    [HideInInspector] public float castingPower;
    private readonly float initialCastingPower = 20;
    private readonly float initialReelInSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        if (fishingSystem.fishingRod == null)
        {
            Debug.LogError("Current fishing rod is null");
            return;
        }
    }

    public void TriggerSetChargingBalance()
    {
        fishingSystem.fishingMiniGame.SetChargingBalance(true);
    }

    // Calculate the reel in speed based on the size of the caught fishes
    public void CalculateReelInSpeed()
    {
        foreach (FishDisplay @catch in fishingSystem.caughtFishes)
        {
            reelInSpeed = initialReelInSpeed - (@catch.fish.size / 10);
        }
    }

    // Set the initial values for the reel in speed and casting power
    public void ResetValues()
    {
        reelInSpeed = initialReelInSpeed;
        castingPower = initialCastingPower;
        playerAnimations.ResetChargingThrowSpeed();
    }

    // Set the reel in speed to a fixed value
    public void ReelInSpeed()
    {
        reelInSpeed = 50;
    }

    /// <summary>
    /// Charges the casting power while the space key is held down.
    /// </summary>
    /// <param name="setChargingThrowSpeed">The action to perform while charging the casting power.</param>
    public void ChargeCasting()
    {
        fishingRodAnimations.PlaySwingAnimation();
        if (castingPower < fishingSystem.fishingRod.throwRange)
        {
            castingPower++;
            playerAnimations.SetChargingThrowSpeed();
        }
    }

    // Play the swing animation and wait for it to finish
    public IEnumerator SwingAnimation(float castPower)
    {
        castingPower *= castPower;
        while (!fishingRodAnimations.GetCurrentAnimationState().IsName("Reverse Swing"))
        {
            yield return null;
        }
        while (fishingRodAnimations.GetCurrentAnimationState().normalizedTime < 1.0f)
        {
            yield return null;
        }
        fishingSystem.SetState(new Casting(fishingSystem));

    }

    // Play the reverse swing animation
    public void PlayerReverseSwingAnimation()
    {
        fishingRodAnimations.PlayReversSwingAnimation();
    }
}