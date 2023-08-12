using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public override void Update()
    {
        controller.HandleInput();
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
        controller.HandleShoppingInput();
    }
}