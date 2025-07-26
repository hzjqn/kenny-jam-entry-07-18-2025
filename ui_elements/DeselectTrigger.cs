using Godot;
using System;

public partial class DeselectTrigger : TextureButton
{
    CardManager cm;
    public override void _Ready()
    {
        base._Ready();

        ButtonDown += Deselect;
        cm = GetNode<CardManager>("/root/CardManager");
    }


    public void Deselect()
    {
        cm.selectedCard = null;
    }
}
