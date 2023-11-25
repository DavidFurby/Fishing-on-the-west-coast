using UnityEngine;

public class PlayerSubscriptions : MonoBehaviour
{
    protected PlayerManager manager;

    public void Initialize(PlayerManager manager)
    {
        this.manager = manager;
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        // WaterCollision events
        WaterCollision.OnBaitEnterSea += manager.fishingController.EnterSea;

        // FishingSpot events
        FishingSpot.StartFishing += (_, _) => manager.SetState(new FishingIdle());
        FishingSpot.StartFishing += manager.SetPlayerPositionAndRotation;

        // PlayerEventController events
        PlayerEventController.OnEnterReelingFish += manager.fishingController.CatchFish;
        PlayerEventController.OnEnterReelingFish += () => manager.SetState(new ReelingFish());
        PlayerEventController.OnEnterIdle += manager.fishingController.ResetValues;
        PlayerEventController.OnWhileCharging += manager.fishingController.ChargeCasting;
        PlayerEventController.OnWhileCharging += manager.fishingController.Release;
        PlayerEventController.OnWhileFishing += manager.fishingController.StartReeling;
        PlayerEventController.OnStartCharging += manager.animations.OnStartCharging;
        PlayerEventController.OnEnterSwinging += manager.animations.OnChargeRelease;
        PlayerEventController.OnEnterIdle += manager.animations.ResetChargingThrowSpeed;
        PlayerEventController.OnWhileCharacterDialog += manager.movement.RotateTowardsInteractive;

        // CharacterDialog events
        CharacterDialog.OnStartConversation += (_) => manager.SetState(new PlayerInDialog());

        // DialogManager events
        DialogManager.OnEndDialog += manager.ReturnControls;

        // Interactive events
        Interactive.OnEnterInteractive += manager.SetInteractive;
        Interactive.OnExitInteractive += manager.RemoveInteractive;
    }

    private void UnsubscribeFromEvents()
    {
        // WaterCollision events
        WaterCollision.OnBaitEnterSea -= manager.fishingController.EnterSea;

        // FishingSpot events
        FishingSpot.StartFishing -= (_, _) => manager.SetState(new FishingIdle());
        FishingSpot.StartFishing -= manager.SetPlayerPositionAndRotation;

        // PlayerEventController events
        PlayerEventController.OnEnterReelingFish -= manager.fishingController.CatchFish;
        PlayerEventController.OnEnterReelingFish -= () => manager.SetState(new ReelingFish());
        PlayerEventController.OnEnterIdle -= manager.fishingController.ResetValues;
        PlayerEventController.OnWhileCharging -= manager.fishingController.ChargeCasting;
        PlayerEventController.OnWhileCharging -= manager.fishingController.Release;
        PlayerEventController.OnWhileFishing -= manager.fishingController.StartReeling;
        PlayerEventController.OnStartCharging -= manager.animations.OnStartCharging;
        PlayerEventController.OnEnterSwinging -= manager.animations.OnChargeRelease;
        PlayerEventController.OnEnterIdle -= manager.animations.ResetChargingThrowSpeed;
        PlayerEventController.OnWhileCharacterDialog -= manager.movement.RotateTowardsInteractive;


        // CharacterDialog events
        CharacterDialog.OnStartConversation -= (_) => manager.SetState(new PlayerInDialog());

        // DialogManager events
        DialogManager.OnEndDialog -= manager.ReturnControls;

        // Interactive events
        Interactive.OnEnterInteractive -= manager.SetInteractive;
        Interactive.OnExitInteractive -= manager.RemoveInteractive;
    }
}
