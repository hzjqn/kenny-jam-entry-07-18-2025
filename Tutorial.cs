using Godot;
using System;

public partial class Tutorial : Control
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (GameManager.isOnTutorial == false)
        {
            QueueFree();
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eventAction)
        {

            if (eventAction.IsActionPressed("ui_cancel"))
            {
                GameManager.isOnTutorial = false;
            }
        }
    }


}
