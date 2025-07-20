using Godot;
using System;

public partial class MessageElement : Node2D
{
    [Export]
    public bool isFirstMessage = false;
    
    [Export]
    public TextureButton okButton;

    [Export]
    public AnimationPlayer animationPlayer;

    [Export]
    public MessageElement nextMessage;

    public override void _Ready()
    {
        base._Ready();
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
        Console.Write("Entered..."+ this.Name);
        animationPlayer.Play("enter");
    }

    public void TransitionOut()
    {
        animationPlayer.Play("exit");
    }

    public void OnAnimationFinished(StringName animName)
    {
        if (animName == "exit")
        {
            QueueFree(); // Remove this message element from the scene
        }
    }
}
