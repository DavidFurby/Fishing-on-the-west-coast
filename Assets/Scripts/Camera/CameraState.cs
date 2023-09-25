public class CameraState : State
{
    protected CameraController controller;
    public CameraState(CameraController controller)
    {
        this.controller = controller;
    }
}

public class PlayerCamera : CameraState
{
    public PlayerCamera(CameraController controller) : base(controller) { }

    public override void LateUpdate()
    {
        base.Update();
        controller.explorationCamera.FollowPlayer();
    }
}

public class ShopItemCamera : CameraState
{
    public ShopItemCamera(CameraController controller) : base(controller) { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.explorationCamera.FollowShopItem();

    }
}

public class CastingBaitCamera : CameraState
{
    public CastingBaitCamera(CameraController controller) : base(controller) { }


    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringCasting();
    }
}

public class FishingBaitCamera : CameraState
{
    public FishingBaitCamera(CameraController controller) : base(controller) { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringFishing();
    }
}
public class ReelingBaitCamera : CameraState
{
    public ReelingBaitCamera(CameraController controller) : base(controller) { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringReeling();
    }
}

public class FishCamera : CameraState
{
    public FishCamera(CameraController controller) : base(controller) { }
}