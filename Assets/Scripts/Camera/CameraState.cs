public abstract class CameraState : State
{
    protected PlayerCamera system;
    public CameraState(PlayerCamera playerCamera)
    {
        this.system = playerCamera;
    }

    public class Player : CameraState
    {
        public Player(PlayerCamera camera) : base(camera) { }

        public override void LateUpdate()
        {
            base.Update();
            system.FollowPlayer();
        }
    }

    public class ShopItem : CameraState
    {
        public ShopItem(PlayerCamera camera) : base(camera) { }
    }
    public override void LateUpdate() {
        system.FollowShopItem();
    }
    public class Bait : CameraState
    {
        public Bait(PlayerCamera camera) : base(camera) { }
    }

    public class Fish : CameraState
    {
        public Fish(PlayerCamera camera) : base(camera) { }
    }
}