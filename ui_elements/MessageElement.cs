using Godot;
using System;

public partial class MessageElement : Node2D
{
    GameManager gm;
    [Export]
    public bool isFirstMessage = false;

    [Export]
    public bool isLastMessage = false;

    public bool isActiveMessage = false;


    [Export]
    public TextureButton okButton;

    [Export]
    public AnimationPlayer animationPlayer;

    [Export]
    public MessageElement nextMessage;

    public override void _Ready()
    {
        base._Ready();
        gm = GetNode<GameManager>("/root/GameManager");
        if (okButton == null) return;

        okButton.Pressed += OnOkButtonPressed;
        animationPlayer.AnimationFinished += OnAnimationFinished;
        if (isFirstMessage)
        {
            TransitionIn();
        }
    }

    public void OnOkButtonPressed()
    {
        TransitionOut();
        if (nextMessage != null)
        {
            nextMessage.TransitionIn();
        }
    }

    public void TransitionIn()
    {
        isActiveMessage = true;
        animationPlayer.Play("enter");
    }

    public void TransitionOut()
    {
        isActiveMessage = false;
        animationPlayer.Play("exit");
    }

    public void OnAnimationFinished(StringName animName)
    {
        if (animName == "exit")
        {
            if (isLastMessage)
                gm.isOnTutorial = false;
            QueueFree(); // Remove this message element from the scene
        }
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eventAction)
        {

            if (eventAction.IsActionPressed("ui_accept"))
            {
                if (isActiveMessage)
                    OnOkButtonPressed();
            }
        }
    }
}
