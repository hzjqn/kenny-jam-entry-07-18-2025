using Godot;
using System;

public partial class Tutorial : Control
{
    GameManager gm;

    public override void _Ready()
    {
        gm = GetNode<GameManager>("/root/GameManager");
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (gm.isOnTutorial == false)
        {
            QueueFree();
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eventAction)
        {

            if (eventAction.IsActionPressed("ui_skip"))
            {
                gm.isOnTutorial = false;
            }
        }
    }


}
