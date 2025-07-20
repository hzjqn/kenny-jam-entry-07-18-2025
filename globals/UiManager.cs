using Godot;
using System;

public partial class UiManager : Node
{
    public Card selectedActionCard { get; set; }

    public static string GetCardSuitName(CardSuits suit)
    {
        return suit switch
        {
            CardSuits.Clergy => "Clergy",
            CardSuits.Militia => "Militia",
            CardSuits.Merchant => "Merchant",
            CardSuits.Opponent => "Opponent",
            _ => "Desconocido"
        };
    }
}
