
public class CharacterState : State
{
    protected CharacterController controller;
    public CharacterState(CharacterController controller)
    {
        this.controller = controller;
    }


}

public class CharacterIdle : CharacterState
{
    public CharacterIdle(CharacterController controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.SetDefaultRotation();
    }

}

public class CharacterInDialog : CharacterState
{
    public CharacterInDialog(CharacterController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RotateTowardsPlayer();
    }
    public override void Update()
    {
        base.Update();

        controller.expression.MouthFlaps();
    }
}