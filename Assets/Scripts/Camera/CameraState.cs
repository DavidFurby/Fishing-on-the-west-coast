public class CameraState : State
{
    protected CameraManager controller;
    public CameraState()
    {
        controller = CameraManager.Instance;
    }
}

public class PlayerCamera : CameraState
{
    public PlayerCamera() : base() { }

    public override void LateUpdate()
    {
        base.Update();
        controller.exploration.FollowPlayer();
    }
}
public class CharacterDialogCamera : CameraState
{
    public CharacterDialogCamera() : base()
    {

    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.exploration.UpdateCameraDuringDialog();
    }
}

public class ShopItemCamera : CameraState
{
    public ShopItemCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.exploration.FollowShopItem();

    }
}

public class CastingBaitCamera : CameraState
{
    public CastingBaitCamera() : base() { }


    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishing.UpdateCameraDuringCasting();
    }
}

public class FishingBaitCamera : CameraState
{
    public FishingBaitCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishing.UpdateCameraDuringFishing();
    }
}
public class ReelingBaitCamera : CameraState
{
    public ReelingBaitCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishing.UpdateCameraDuringReeling();
    }
}

public class FishCamera : CameraState
{
    public FishCamera() : base() { }
}