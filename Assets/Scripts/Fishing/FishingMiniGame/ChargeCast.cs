
using UnityEngine;
using UnityEngine.UI;

public class ChargeCast : MonoBehaviour
{
    [SerializeField] private Scrollbar castingPowerBalance;
    [SerializeField] private FishingController controller;
    private bool castingPowerDirection = true;

    void Start()
    {
        castingPowerBalance.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        SubscribeEvents();
    }

    void OnDisable()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        FishingController.OnWhileCharging += ChargeMiniGame;
        FishingController.OnChargeRelease += StopCharging;
        FishingRodLogic.OnTriggerSetChargingBalance += StartCharging;
    }

    private void UnsubscribeEvents()
    {
        FishingController.OnChargeRelease -= StopCharging;
        FishingController.OnWhileCharging -= ChargeMiniGame;
        FishingRodLogic.OnTriggerSetChargingBalance -= StartCharging;
    }

    private void StartCharging()
    {
        castingPowerBalance.gameObject.SetActive(true);
    }
    private void StopCharging()
    {
        castingPowerBalance.gameObject.SetActive(false);
    }
    private void ChargeMiniGame()
    {
        if (castingPowerBalance.gameObject.activeSelf)
        {
            controller.chargeLevel = Mathf.Min(castingPowerBalance.value, 1 - castingPowerBalance.value) + 1f;
            castingPowerBalance.value += castingPowerDirection ? 0.05f : -0.05f;
            if (castingPowerBalance.value >= 1)
            {
                castingPowerDirection = false;
            }
            else if (castingPowerBalance.value <= 0)
            {
                castingPowerDirection = true;
            }
        }

    }
}