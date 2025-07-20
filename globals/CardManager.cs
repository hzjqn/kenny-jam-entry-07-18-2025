using Godot;
using Godot.Collections;
using System;

public enum SpecialActionTypes
{
    Flip,
    Convert,
}
public partial class CardManager : Node
{
    
    public static int CardsPorPalo = 3;
    public static Array<Card> MazoPersonajes = new Array<Card>();
    public static Array<Card> SpecialActionsDeck = [];
    public static Array<Card> BasicActionsDeck = [];
    public static Array<Card> PeopleDeck = [];

    public static void StartBoard()
    {
        CardsPorPalo = 3;
        MazoPersonajes = new Array<Card>();
        SpecialActionsDeck = [];
        BasicActionsDeck = [];
        PeopleDeck = [];
    
        foreach (CardSuits palo in Enum.GetValues<CardSuits>())
        {
            GameManager.currentRun.Popularidades[palo] = 0;
        }
        BuildMazoPersonajes();
        BuildSpecialActionsDeck();
    }
        public static void BuildMazoPersonajes()
            {
                for (int valor = 0; valor < CardsPorPalo; valor++)
                {
                    foreach (CardSuits palo in Enum.GetValues<CardSuits>())
                    {
                        if (valor == 1 && palo == CardSuits.Militia) continue;
                        MazoPersonajes.Add(new Card(valor, palo));
                    }
                }
            }

    public static void BuildSpecialActionsDeck()
    {
        foreach (CardSuits palo in Enum.GetValues<CardSuits>())
        {
            SpecialActionsDeck.Add(new Card((int)SpecialActionTypes.Convert, palo, CardType.SpecialAction, "Convert target card to " + UiManager.GetCardSuitName(palo)));
        }
        SpecialActionsDeck.Add(new Card((int)SpecialActionTypes.Flip, 0, CardType.SpecialAction, "When played, 50% chance to double the value of target card or halve it..."));
    }

    public static void InvokeSpecialCard(Card card, params Card[] targets)
    {
        if (card.cardType != CardType.SpecialAction) return;

        switch (card.value)
        {
            case (int)SpecialActionTypes.Flip:
                CardManager.FlipCard(targets[0]);
                break;
            case (int)SpecialActionTypes.Convert:
                CardManager.ConvertCard(targets[0], card.cardSuit);
                break;
            default:
                break;
        }
    }

    public static void FlipCard(Card target) {
        Random rnd = new Random();
        target.value = rnd.Next(2) == 0 ? target.value * 2 : target.value;
    }
    
    public static void ConvertCard(Card target, CardSuits newSuit)
    {
        target.cardSuit = newSuit;
    }
}

