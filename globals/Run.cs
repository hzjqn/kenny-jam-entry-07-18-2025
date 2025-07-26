using System;
using Godot;
using Godot.Collections;

public partial class Run : Node2D
{
    [Signal]
    public delegate void UpdateEventHandler();
    CardManager cm;
    GameManager gm;
    public double Monedas = 20;
    public int Fuerza = 20;
    public int Popularidad = 0;
    public int Gasto = 1;

    public override void _Ready()
    {
        base._Ready();
        cm = GetNode<CardManager>("/root/CardManager");
        gm = GetNode<GameManager>("/root/GameManager");
    }


    public void PagarDiezmo()
    {
        Monedas *= 0.9;
    }

    public void PagarImpuestos()
    {
        Monedas *= 0.7;
    }

    public void CobrarImpuestos()
    {
        Monedas += 1;
    }

    public void Ignorar(Card target)
    {
        Popularidad -= 1;
    }

    public void Pagar(Card target)
    {
        CardSuits palo = target.cardSuit;
        int valor = target.value;

        if (palo == CardSuits.Clergy)
        {
            Monedas = (int)(0.9 * Monedas);
            Popularidad++;
        }
        else if (palo == CardSuits.Militia)
        {
            Monedas -= 3 * valor;
            Popularidad++;
            Fuerza++;
        }
        else if (palo == CardSuits.Opponent)
        {
            Monedas -= 1 * valor;
            Popularidad--;
        }
        else if (palo == CardSuits.Merchant)
        {
            Monedas -= 2 * valor;
            Popularidad++;
        }
    }

    public void Cobrar(Card Card)
    {
        CardSuits palo = Card.cardSuit;
        int valor = Card.value;

        if (palo == CardSuits.Clergy || palo == CardSuits.Militia)
        {
            Popularidad -= 3;
            Monedas += valor;
        }
        else if (palo == CardSuits.Opponent)
        {
            Popularidad += 1;
        }
        else if (palo == CardSuits.Merchant)
        {
            Popularidad++;
            Gasto++;
        }
    }

    public void Violencia(Card target)
    {
        CardSuits palo = target.cardSuit;
        int valor = target.value;

        if (palo == CardSuits.Clergy || palo == CardSuits.Militia)
        {
            Popularidad -= 3;
            Monedas += valor;
        }
        else if (palo == CardSuits.Opponent)
        {
            Monedas -= 2;
            Popularidad++;
            Fuerza -= 3;
        }
        else if (palo == CardSuits.Merchant)
        {
            Popularidad -= 2;
            Gasto++;
        }


    }

    public void UseAction(Card card, Card target)
    {
        GD.Print(card, target);
        switch (card.cardType)
        {
            case CardType.BasicAction:
                UseBasicAction(card, target);
                break;

            case CardType.SpecialAction:
                UseSpecialAction(card, target);
                break;
        }
    }

    public void UseSpecialAction(Card card, Card target)
    {
        switch (card.value)
        {
            case (int)SpecialActionTypes.Convert:
                ConvertCard(target, card.cardSuit);
                break;
            default:
                break;
        }


        if (cm.SpecialActionsHand.Contains(card))
        {
            int index = cm.SpecialActionsHand.IndexOf(card);
            if (cm.selectedCard == card)
            {
                cm.selectedCard = null;
            }
            cm.SpecialActionsHand[index] = null;
        }

        EmitSignal(SignalName.Update);
    }

    public static void ConvertCard(Card target, CardSuits newSuit)
    {
        target.cardSuit = newSuit;
    }

    public void UseBasicAction(Card card, Card target)
    {
        if (card.cardType != CardType.BasicAction) return;
        switch (card.value)
        {
            case (int)BasicActionTypes.Tax:
                {
                    Cobrar(target);
                    break;
                }
            case (int)BasicActionTypes.Bribe:
                {
                    Pagar(target);
                    break;
                }
            case (int)BasicActionTypes.Threat:
                {
                    Violencia(target);
                    break;
                }
            case (int)BasicActionTypes.Ignore:
                {
                    Ignorar(target);
                    break;
                }
        }

        if (cm.BasicActionsHand.Contains(card))
        {
            int index = cm.BasicActionsHand.IndexOf(card);
            if (cm.selectedCard == card)
            {
                cm.selectedCard = null;
            }
            cm.BasicActionsHand[index] = null;
        }

        if (cm.RoomSlots.Contains(target))
        {
            int index = cm.RoomSlots.IndexOf(target);
            cm.RoomSlots[index] = null;
        }

        EmitSignal(SignalName.Update);
    }
}