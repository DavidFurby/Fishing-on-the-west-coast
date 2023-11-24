using UnityEngine;

public class PlayerSubscriptions : MonoBehaviour
{
    protected PlayerController controller;

    public void Initialize(PlayerController controller)
    {
        this.controller = controller;
        print(this.controller);
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
        WaterCollision.OnBaitEnterSea += controller.EnterSea;
        FishingSpot.StartFishing += (_, _) => controller.SetState(new FishingIdle());
        FishingSpot.StartFishing += controller.SetPlayerPositionAndRotation;
        PlayerEventController.OnEnterReelingFish += controller.CatchFish;
        PlayerEventController.OnEnterReelingFish += () => controller.SetState(new ReelingFish());
        PlayerEventController.OnEnterIdle += controller.ResetValues;
        PlayerEventController.OnWhileCharging += controller.ChargeCasting;
        PlayerEventController.OnWhileCharging += controller.Release;
        PlayerEventController.OnWhileFishing += controller.StartReeling;
        CharacterDialog.OnStartConversation += (_) => controller.SetState(new PlayerInDialog());
        DialogManager.OnEndDialog += controller.ReturnControls;
        Interactive.OnEnterInteractive += controller.SetInteractive;
        Interactive.OnExitInteractive += controller.RemoveInteractive;
    }

    private void UnsubscribeFromEvents()
    {
        WaterCollision.OnBaitEnterSea -= controller.EnterSea;
        FishingSpot.StartFishing -= (_, _) => controller.SetState(new FishingIdle());
        FishingSpot.StartFishing -= controller.SetPlayerPositionAndRotation;
        PlayerEventController.OnEnterReelingFish -= controller.CatchFish;
        PlayerEventController.OnEnterReelingFish -= () => controller.SetState(new ReelingFish());
        PlayerEventController.OnEnterIdle -= controller.ResetValues;
        PlayerEventController.OnWhileCharging -= controller.ChargeCasting;
        PlayerEventController.OnWhileCharging -= controller.Release;
        PlayerEventController.OnWhileFishing -= controller.StartReeling;
        CharacterDialog.OnStartConversation -= (_) => controller.SetState(new PlayerInDialog());
        DialogManager.OnEndDialog -= controller.ReturnControls;
        Interactive.OnEnterInteractive -= controller.SetInteractive;
        Interactive.OnExitInteractive -= controller.RemoveInteractive;
    }
}