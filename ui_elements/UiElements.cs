using Godot;
using System;
using Godot.Collections;

public partial class UiElements : Node2D
{
    GameManager gm;
    CardManager cm;
    [Export] Array<PeopleCardElement> roomSlotElements;
    [Export] Array<PeopleCardElement> basicActionHandCardSlots;
    [Export] Array<PeopleCardElement> specialActionHandCardSlots;

    public override void _Ready()
    {
        base._Ready();
        GD.Print("UI Elements ready");
        gm = GetNode<GameManager>("/root/GameManager");
        cm = GetNode<CardManager>("/root/CardManager");
        cm.GameBoardStarted += OnGameBoardStarted;
        cm.CardsUpdated += OnCardsUpdate;
    }
    public void OnGameBoardStarted()
    {
        UpdateCardInstances();
    }

    public void OnCardsUpdate()
    {
        UpdateCardInstances();
    }

    public void UpdateCardInstances()
    {
        GD.Print("UpdateCardInstances");

        int basicActionIndex = 0;
        foreach (PeopleCardElement cardElement in basicActionHandCardSlots)
        {
            Card card = cm.BasicActionsHand[basicActionIndex];
            if (cardElement.card != card)
            {
                cardElement.card = card;
                if (!cardElement.enabled)
                {
                    cardElement.Appear();
                }
                if (cardElement.card == null)
                {
                    cardElement.Disappear();
                }
            }

            basicActionIndex++;
        }
        int specialActionIndex = 0;
        foreach (PeopleCardElement cardElement in specialActionHandCardSlots)
        {
            Card card = cm.SpecialActionsHand[specialActionIndex];
            if (cardElement.card != card)
            {
                cardElement.card = card;
                if (!cardElement.enabled)
                {
                    cardElement.Appear();
                }
                if (cardElement.card == null)
                {
                    cardElement.Disappear();
                }
            }

            specialActionIndex++;
        }
        int peopleCardIndex = 0;
        foreach (PeopleCardElement cardElement in roomSlotElements)
        {
            Card card = cm.RoomSlots[peopleCardIndex];
            if (cardElement.card != card)
            {
                cardElement.card = card;
                if (!cardElement.enabled)
                {
                    cardElement.Appear();
                }
                if (cardElement.card == null)
                {
                    cardElement.Disappear();
                }
            }

            peopleCardIndex++;
        }
    }
}
