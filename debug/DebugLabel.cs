using Godot;
using System;

public partial class DebugLabel : RichTextLabel
{
    public override void _Process(double delta)
    {
        Run run = GameManager.currentRun;

        if (run == null) return;

        base._Process(delta);

        Text = $@"Popularity: {run.Popularidad}
                Strength: {run.Fuerza}
                Riches: {run.Monedas}
                Expenses: {run.Gasto}


                Selected Card: 
                - {CardManager.selectedCard?.value} / {CardManager.selectedCard?.cardSuit} / {CardManager.selectedCard?.cardType} 
                Hovered Card:
                - {CardManager.hoveredCard?.value} / {CardManager.hoveredCard?.cardSuit} / {CardManager.hoveredCard?.cardType} 
            ";
    }

}
