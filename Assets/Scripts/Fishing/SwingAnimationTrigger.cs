using UnityEngine;

public class SwingAnimationTrigger : MonoBehaviour
{
    [SerializeField] FishingMiniGame miniGame;

    public void TriggerSetChargingBalance()
    {
        miniGame.SetChargingBalance();
    }
}
