using Godot;
using System;

public partial class DeselectTrigger : TextureButton
{
    public override void _Ready()
    {
        base._Ready();

        ButtonDown += Deselect;
    }


    public void Deselect()
    {
        GD.Print("Deselected");
        CardManager.selectedCard = null;
    }
}
