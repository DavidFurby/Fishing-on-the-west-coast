
using UnityEngine;
using UnityEngine.UI;

public class ChargeCast : MonoBehaviour
{
    private Scrollbar castingPowerBalance;
    private bool castingPowerDirection = true;

    void Start()
    {
        castingPowerBalance = GetComponentInChildren<Scrollbar>();
        castingPowerBalance.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        SubscribeEvents();
    }

    void OnDestroy()
    {
        UnsubscribeEvents();
    }
    private void SubscribeEvents()
    {
        PlayerEventController.OnWhileCharging += ChargeMiniGame;
        PlayerEventController.OnEnterSwinging += StopCharging;
        RodLogic.OnTriggerSetChargingBalance += StartCharging;
    }

    private void UnsubscribeEvents()
    {
        PlayerEventController.OnEnterSwinging -= StopCharging;
        PlayerEventController.OnWhileCharging -= ChargeMiniGame;
        RodLogic.OnTriggerSetChargingBalance -= StartCharging;
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
            PlayerController.Instance.chargeLevel = Mathf.Min(castingPowerBalance.value, 1 - castingPowerBalance.value) + 1f;
            castingPowerBalance.value += (castingPowerDirection ?  1 : -1) * Time.deltaTime * 5f;
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