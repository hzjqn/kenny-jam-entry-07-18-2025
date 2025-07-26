using Godot;
using System;

public partial class DebugLabel : RichTextLabel
{
    GameManager gm;
    CardManager cm;


    public override void _Ready()
    {
        base._Ready();
        gm = GetNode<GameManager>("/root/GameManager");
        cm = GetNode<CardManager>("/root/CardManager");

    }

    public override void _Process(double delta)
    {
        Run run = gm.currentRun;

        if (run == null) return;

        base._Process(delta);

        Text = $@"Popularity: {run.Popularidad}
                Strength: {run.Fuerza}
                Riches: {run.Monedas}
                Expenses: {run.Gasto}


                Selected Card: 
                - {cm.selectedCard?.value} / {cm.selectedCard?.cardSuit} / {cm.selectedCard?.cardType} 
                Hovered Card:
                - {cm.hoveredCard?.value} / {cm.hoveredCard?.cardSuit} / {cm.hoveredCard?.cardType} 
                - Card slots
                [{cm.RoomSlots.ToString()}] 
                - hand slots
                [{cm.BasicActionsHand.ToString()}] 
                - sp slots
                [{cm.SpecialActionsHand.ToString()}] 
            ";
    }

}
