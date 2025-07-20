using Godot;
using Godot.Collections;
using System;

public enum SpecialActionTypes
{
    Flip,
    Convert,
}

public enum BasicActionTypes
{
    Threat,
    Tax,
    Bribe,
    Ignore
}

public partial class CardManager : Node
{
    public static Card selectedCard;
    public static Card hoveredCard;
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

    public static void BuildBasicActionsDeck()
    {
        BasicActionsDeck = [
            new Card((int)BasicActionTypes.Ignore, 0, CardType.BasicAction, "Ignore a person."),
            new Card((int)BasicActionTypes.Tax, 0, CardType.BasicAction, "Tax a person."),
            new Card((int)BasicActionTypes.Bribe, 0, CardType.BasicAction, "Bribe a person."),
            new Card((int)BasicActionTypes.Threat, 0, CardType.BasicAction, "Threat a person."),
        ];
    }

    public static void BuildSpecialActionsDeck()
    {
        foreach (CardSuits palo in Enum.GetValues<CardSuits>())
        {
            SpecialActionsDeck.Add(new Card((int)SpecialActionTypes.Convert, palo, CardType.SpecialAction, "Convert target card to " + UiManager.GetCardSuitName(palo)));
        }
        SpecialActionsDeck.Add(new Card((int)SpecialActionTypes.Flip, 0, CardType.SpecialAction, "When played, 50% chance to double the value of target card or halve it..."));
    }

    public static void useSelectedCardOnHoveredCard()
    {
        if (selectedCard.cardType == CardType.BasicAction)
        {
            GameManager.currentRun.UseAction(selectedCard, hoveredCard);
        }
    }
}

