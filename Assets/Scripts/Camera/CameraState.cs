public class CameraState : State
{
    protected CameraController controller;
    public CameraState()
    {
        controller = CameraController.Instance;
    }
}

public class PlayerCamera : CameraState
{
    public PlayerCamera() : base() { }

    public override void LateUpdate()
    {
        base.Update();
        controller.explorationCamera.FollowPlayer();
    }
}

public class ShopItemCamera : CameraState
{
    public ShopItemCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.explorationCamera.FollowShopItem();

    }
}

public class CastingBaitCamera : CameraState
{
    public CastingBaitCamera() : base() { }


    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringCasting();
    }
}

public class FishingBaitCamera : CameraState
{
    public FishingBaitCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringFishing();
    }
}
public class ReelingBaitCamera : CameraState
{
    public ReelingBaitCamera() : base() { }

    public override void LateUpdate()
    {
        base.LateUpdate();
        controller.fishingCamera.UpdateCameraDuringReeling();
    }
}

public class FishCamera : CameraState
{
    public FishCamera() : base() { }
}