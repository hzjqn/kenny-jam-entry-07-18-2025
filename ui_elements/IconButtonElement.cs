using Godot;
using System;

public partial class IconButtonElement : Node2D
{
    [Export]
    public AnimationPlayer animationPlayer;

    float rotationSeed = 0.0f;



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

    public void AniamteHover()
    {
        animationPlayer.Play("hover");
    }

    public void AniamteHoverOut()
    {
        animationPlayer.Play("hover_out");
    }
}
