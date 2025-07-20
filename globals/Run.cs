using System;
using Godot.Collections;

public class Run
{
    public Dictionary<CardSuits, int> Popularidades = new Dictionary<CardSuits, int>();
    public double Monedas = 20;
    public int Fuerza = 20;
    public int Popularidad = 0;
    public int Gasto = 1;
    
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
        Popularidades[target.cardSuit] -= 1;
    }

    public void Pagar(Card Card)
    {
        CardSuits palo = Card.cardSuit;
        int valor = Card.value;

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

    public void Violencia(Card Card)
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

    public int CalcularPopularidadTotal()
    {
        int total = 0;
        foreach (var val in Popularidades.Values)
        {
            total += val;
        }
        return total;
    }

    public void UseAction(Card card, Card target)
    {
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
            case (int)SpecialActionTypes.Flip:
                FlipCard(target);
                break;
            case (int)SpecialActionTypes.Convert:
                ConvertCard(target, card.cardSuit);
                break;
            default:
                break;
        }
    }

    public static void FlipCard(Card target)
    {
        Random rnd = new Random();
        target.value = rnd.Next(2) == 0 ? target.value * 2 : target.value;
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
    }

    public void MainLoop()
    {
        while (true)
        {
            // room(this); // Placeholder for future interaction logic
            break; // Prevent infinite loop for now
        }
    }
}