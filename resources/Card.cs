using Godot;
using System;
[GlobalClass]

public partial class Card : Resource
{


    [Export] public CardType cardType = CardType.Person;
    [Export] public CardSuits cardSuit = CardSuits.Clergy;
    [Export] public int value = 2;
    [Export] public string rules = "";

    public Card()
    { }
    public Card(int value = 1, CardSuits suit = CardSuits.Clergy, CardType cardType = CardType.Person, string rules = "")
    {
        this.value = value;
        this.cardType = cardType;
        this.cardSuit = suit;
        this.rules = rules;
    }

    public override string ToString()
    {
        return $@"{value}:{cardSuit}:{cardType}";
    }

}