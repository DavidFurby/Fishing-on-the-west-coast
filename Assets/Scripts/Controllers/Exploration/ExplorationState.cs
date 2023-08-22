public class ExplorationState : State
{
    protected ExplorationController controller;
    public ExplorationState(ExplorationController controller)
    {
        this.controller = controller;
    }


}

public class ExplorationIdle : ExplorationState
{
    public ExplorationIdle(ExplorationController controller) : base(controller)
    {

    }

    public override void Update()
    {
        controller.HandleInput();
        controller.RaiseOpenItemMenuEvent();

    }
    public override void FixedUpdate()
    {
        controller.HandleMovement();
    }

}

public class Interacting : ExplorationState
{
    public Interacting(ExplorationController controller) : base(controller)
    {
    }

}

public class Shopping : ExplorationState
{
    public Shopping(ExplorationController controller) : base(controller)
    {
    }
    public override void Update()
    {
        controller.RaiseNavigateShopEvent();
    }
}
