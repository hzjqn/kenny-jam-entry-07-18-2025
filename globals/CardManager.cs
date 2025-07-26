using Godot;
using Godot.Collections;
using System;

public enum SpecialActionTypes
{
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
    [Signal]
    public delegate void CardsUpdatedEventHandler();
    [Signal]
    public delegate void GameBoardStartedEventHandler();

    GameManager gm;
    public int daysGoverned = 0;
    public Card selectedCard;
    public Card hoveredCard;
    public int CardsPorPalo = 3;
    public Array<Card> MazoPersonajes = new Array<Card>();
    public Array<Card> SpecialActionsDeck = [];
    public Array<Card> BasicActionsDeck = [];
    public Array<Card> PeopleDeck = [];
    [Export]
    public Array<Card> RoomSlots = [
        null,
        null,
        null,
        null,
    ];
    [Export]
    public Array<Card> BasicActionsHand = [
        null,
        null,
        null,
        null,
    ];
    [Export]
    public Array<Card> SpecialActionsHand = [
        null,
        null,
    ];


    public override void _Ready()
    {
        base._Ready();
        gm = GetNode<GameManager>("/root/GameManager");
    }

    public void StartBoard(Run run)
    {
        run.Update += () =>
        {
            EmitSignal(SignalName.CardsUpdated);
        };
        CardsPorPalo = 3;
        MazoPersonajes = new Array<Card>();
        SpecialActionsDeck = GenerateSpecialActionsDeck();
        BasicActionsDeck = GenerateBasicActionsDeck();
        PeopleDeck = GeneratePeopleDeck();
        gm.currentRun.Popularidad = 0;

        Deal();
        EmitSignal(SignalName.GameBoardStarted);
    }

    public void Deal()
    {
        SpecialActionsDeck.Shuffle();

        int cardSlotIndex = 0;
        foreach (Card card in RoomSlots)
        {
            Card addedCard = PeopleDeck[0];
            PeopleDeck.Remove(addedCard);
            RoomSlots[cardSlotIndex] = addedCard;
            cardSlotIndex++;
        }

        int basicCardSlotIndex = 0;
        foreach (Card card in BasicActionsHand)
        {
            Card addedCard = BasicActionsDeck[basicCardSlotIndex];
            BasicActionsHand[basicCardSlotIndex] = addedCard;
            basicCardSlotIndex++;
        }

        int specialCardSlotIndex = 0;
        SpecialActionsDeck.Shuffle();
        foreach (Card card in SpecialActionsHand)
        {
            Card addedCard = SpecialActionsDeck[specialCardSlotIndex];
            SpecialActionsHand[specialCardSlotIndex] = addedCard;
            specialCardSlotIndex++;
        }

        EmitSignal(SignalName.CardsUpdated);
    }

    public Array<Card> GeneratePeopleDeck()
    {
        Array<Card> deck = [];
        for (int value = 0; value < CardsPorPalo; value++)
        {
            foreach (CardSuits suit in Enum.GetValues<CardSuits>())
            {
                deck.Add(new Card(value, suit));
            }
        }

        return deck;
    }

    public Array<Card> GenerateBasicActionsDeck()
    {

        return [
            new Card((int)BasicActionTypes.Ignore, 0, CardType.BasicAction, "Ignore a person."),
            new Card((int)BasicActionTypes.Tax, 0, CardType.BasicAction, "Tax a person."),
            new Card((int)BasicActionTypes.Bribe, 0, CardType.BasicAction, "Bribe a person."),
            new Card((int)BasicActionTypes.Threat, 0, CardType.BasicAction, "Threat a person."),
        ];
    }

    public Array<Card> GenerateSpecialActionsDeck()
    {
        Array<Card> deck = [];
        foreach (CardSuits palo in Enum.GetValues<CardSuits>())
        {
            deck.Add(new Card((int)SpecialActionTypes.Convert, palo, CardType.SpecialAction, "Convert target card to " + UiManager.GetCardSuitName(palo)));
        }

        return deck;
    }

    public void useSelectedCardOnHoveredCard()
    {
        GD.Print("useSelectedCardOnHoveredCard");
        if (selectedCard.cardType == CardType.BasicAction)
        {
            gm.currentRun.UseAction(selectedCard, hoveredCard);
        }
    }
}

