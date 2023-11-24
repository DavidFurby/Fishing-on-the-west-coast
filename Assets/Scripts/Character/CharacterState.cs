
public class CharacterState : State
{
    protected CharacterManager controller;
    public CharacterState(CharacterManager controller)
    {
        this.controller = controller;
    }


}

public class CharacterIdle : CharacterState
{
    public CharacterIdle(CharacterManager controller) : base(controller)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        controller.SetDefaultRotation();
        controller.movement.EnabledMovement();
    }
    public override void FixedUpdate()
    {
        base.Update();
        controller.movement.Move();
    }

}

public class CharacterInDialog : CharacterState
{
    public CharacterInDialog(CharacterManager controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        controller.RotateTowardsPlayer();
        controller.movement.PauseMovement();
    }
    public override void Update()
    {
        base.Update();

        controller.expression.MouthFlaps();
    }
}