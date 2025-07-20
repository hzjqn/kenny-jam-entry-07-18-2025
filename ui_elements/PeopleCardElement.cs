using Godot;
using System;

[Tool]
public partial class PeopleCardElement : Node2D
{

    [Export]
    Node2D body;

    [Export]
    Node2D shadow;

    [Export]
    Label rulesLabel;

    [Export]
    Label valueLabel;

    [Export]
    CardType cardType = CardType.Person;


    [Export]
    AnimationPlayer animationPlayer;

    [Export]

    float rotationSeed = 0;


    public override void _Ready()
    {
        rotationSeed = (float)GD.RandRange(0, 1000) * 10;
        TextureButton control = GetNode<TextureButton>("Control");
        if (control != null)
        {
            control.MouseEntered += AniamteHover;
            control.MouseExited += AniamteHoverOut;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        shadow.GlobalRotation = Mathf.Sin((Time.GetTicksMsec() + rotationSeed) / 1000.0f) * 0.05f;
        body.GlobalRotation = shadow.GlobalRotation;
    }

    public void AniamteHover()
    {
        GD.Print("Hover");
        animationPlayer.Play("hover");
    }

    public void AniamteHoverOut()
    {
        GD.Print("Hover out");
        animationPlayer.Play("hover_out");
    }
}
