using Godot;
using System;

public enum CardType
{
    Person,
    SpecialAction,
    BasicAction
}

public enum CardSuits
{
    Clergy,
    Militia,
    Merchant,
    Opponent,
}

public partial class Card : Resource
{
    [Export] public CardType cardType = CardType.Person;
    [Export] public CardSuits cardSuit = CardSuits.Clergy;
    [Export] public int value { get; set; } = 2;
    [Export] public string rules { get; set; } = "";

    public Card(int value = 1, CardSuits suit = CardSuits.Clergy, CardType cardType = CardType.Person, string rules = "")
    {
        this.value = value;
        this.cardSuit = suit;
    }
}