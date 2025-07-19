using System;
using System.Collections.Generic;

public class Carta
{
    public int Valor { get; set; }
    public char Palo { get; set; }

    public Carta(int valor, char palo)
    {
        Valor = valor;
        Palo = palo;
    }
}

public class GameManager
{
    public List<Carta> MazoPersonajes = new List<Carta>();
    public Dictionary<char, int> Popularidades = new Dictionary<char, int>();

    public static readonly string PALOS = "eboc";
    public static int CartasPorPalo = 3;

    public double Monedas = 20;
    public int Fuerza = 20;
    public int Popularidad = 0;
    public int Gasto = 1;

    public static Dictionary<string, Func<Carta, Carta>> MazoHabilidades = new();

    public GameManager()
    {
        foreach (char palo in PALOS)
        {
            Popularidades[palo] = 0;
        }
        BuildMazoPersonajes();
        BuildMazoHabilidades();
    }

    public void BuildMazoPersonajes()
    {
        for (int valor = 0; valor < CartasPorPalo; valor++)
        {
            foreach (char palo in PALOS)
            {
                if (valor == 1 && palo == 'e') continue;
                MazoPersonajes.Add(new Carta(valor, palo));
            }
        }
    }

    public void BuildMazoHabilidades()
    {
        foreach (char palo in PALOS)
        {
            MazoHabilidades[palo.ToString()] = (Carta c) => { c.Palo = palo; return c; };
        }
        MazoHabilidades["flip"] = (Carta c) =>
        {
            Random rnd = new Random();
            c.Valor = rnd.Next(2) == 0 ? c.Valor * 2 : c.Valor;
            return c;
        };
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

    public void Ignorar(char palo)
    {
        Popularidades[palo] -= 1;
    }

    public void Pagar(Carta carta)
    {
        char palo = carta.Palo;
        int valor = carta.Valor;

        if (palo == 'c')
        {
            Monedas = (int)(0.9 * Monedas);
            Popularidad++;
        }
        else if (palo == 'e')
        {
            Monedas -= 3 * valor;
            Popularidad++;
            Fuerza++;
        }
        else if (palo == 'b')
        {
            Monedas -= 1 * valor;
            Popularidad--;
        }
        else if (palo == 'o')
        {
            Monedas -= 2 * valor;
            Popularidad++;
        }
    }

    public void Cobrar(Carta carta)
    {
        char palo = carta.Palo;
        int valor = carta.Valor;

        if (palo == 'c' || palo == 'e')
        {
            Popularidad -= 3;
            Monedas += valor;
        }
        else if (palo == 'b')
        {
            Popularidad += 1;
        }
        else if (palo == 'o')
        {
            Popularidad++;
            Gasto++;
        }
    }

    public void Violencia(Carta carta)
    {
        char palo = carta.Palo;
        int valor = carta.Valor;

        if (palo == 'c' || palo == 'e')
        {
            Popularidad -= 3;
            Monedas += valor;
        }
        else if (palo == 'b')
        {
            Monedas -= 2;
            Popularidad++;
            Fuerza -= 3;
        }
        else if (palo == 'o')
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

    public void MainLoop()
    {
        while (true)
        {
            // room(this); // Placeholder for future interaction logic
            break; // Prevent infinite loop for now
        }
    }
}
