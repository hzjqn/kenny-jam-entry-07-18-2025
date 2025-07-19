using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame
{
    // Representa cada carta del mazo de personajes
    class Card
    {
        public int Valor { get; set; }
        public char Palo { get; set; }

        public Card(int valor, char palo)
        {
            Valor = valor;
            Palo = palo;
        }

        public override string ToString() => $"[{Valor},{Palo}]";
    }

    // Representa cada “habilidad” (acción especial)
    class Skill
    {
        public string Name { get; }
        public int Argc { get; }
        public Action<Card> Effect { get; }

        public Skill(string name, int argc, Action<Card> effect)
        {
            Name   = name;
            Argc   = argc;
            Effect = effect;
        }

        public override string ToString() => Name;
    }

    class GameManager
    {
        // Constantes de juego
        private const string PALOS             = "eboc";
        private const int    CARTAS_POR_PALO   = 3;
        private const int    CARDS_PER_ROOM    = 4;
        private const int    SKILLS_PER_ROOM   = 2;

        private static readonly Random rng = new();

        // Estado del jugador
        private int monedas      = 20;
        private int fuerza       = 20;
        private int popularidad  = 0;
        private int gasto        = 1;

        // Mazos
        private readonly List<Card>  mazoPersonajes;
        private readonly List<Skill> mazoHabilidades;

        // Estado de la “sala” actual
        private Dictionary<int, Card?>  peopleRoom  = new();
        private Dictionary<int, Skill?> actionsRoom = new();

        /* ---------- Constructor y métodos de construcción de mazos ---------- */

        public GameManager()
        {
            mazoPersonajes  = BuildMazoPersonajes();
            mazoHabilidades = BuildMazoHabilidades();
        }

        private static List<Card> BuildMazoPersonajes()
        {
            var mazo = new List<Card>();
            for (int valor = 0; valor < CARTAS_POR_PALO; valor++)
            {
                foreach (var palo in PALOS)
                    mazo.Add(new Card(valor, palo));
            }
            return mazo;
        }

        private static List<Skill> BuildMazoHabilidades()
        {
            var mazo = new List<Skill>();

            // Un “convertir” por cada palo
            foreach (var palo in PALOS)
            {
                char captured = palo;                       // Captura para la lambda
                mazo.Add(new Skill($"convertir_{captured}", 1,
                                   card => card.Palo = captured));
            }

            // Flip: 50 % de chances de duplicar el valor
            mazo.Add(new Skill("flip", 1, card =>
            {
                if (rng.Next(2) == 1) card.Valor *= 2;
            }));

            return mazo;
        }

        /* ------------------------ Acciones básicas ------------------------ */

        private void Ignorar(Card _) => popularidad -= 1;

        private void Pagar(Card carta)
        {
            char palo  = carta.Palo;
            int  valor = carta.Valor;

            switch (palo)
            {
                case 'c':
                    monedas = (int)(0.9 * monedas);
                    popularidad += 1;
                    break;

                case 'e':
                    monedas -= 3 * valor;
                    popularidad += 1;
                    fuerza += 1;
                    break;

                case 'b':
                    monedas -= valor;
                    popularidad -= 1;
                    break;

                case 'o':
                    monedas -= 2 * valor;
                    popularidad += 1;
                    break;
            }
        }

        private void Cobrar(Card carta)
        {
            char palo  = carta.Palo;
            int  valor = carta.Valor;

            switch (palo)
            {
                case 'c':
                case 'e':
                    popularidad -= 3;
                    monedas     += valor;
                    break;

                case 'b':
                    popularidad += 1;   // arreglo: antes no sumaba nada
                    break;

                case 'o':
                    popularidad += 1;
                    gasto       += 1;
                    break;
            }
        }

        private void Violencia(Card carta)
        {
            char palo  = carta.Palo;
            int  valor = carta.Valor;

            switch (palo)
            {
                case 'c':
                    popularidad -= 3;
                    monedas     += valor;
                    break;

                case 'e':
                    popularidad += 3;
                    monedas     += valor;
                    break;

                case 'b':
                    monedas     += 1;
                    popularidad += 3;
                    fuerza      -= 3;
                    break;

                case 'o':
                    popularidad += 2;
                    monedas     += 2 * valor;
                    gasto       += 1;
                    break;
            }
        }

        /* ----------------------- Utilidades internas ----------------------- */

        private Card  RobarPersonaje() => RobarDeLista(mazoPersonajes);
        private Skill RobarHabilidad()  => RobarDeLista(mazoHabilidades);

        private static T RobarDeLista<T>(List<T> lista)
        {
            int i = rng.Next(lista.Count);
            T elem = lista[i];
            lista.RemoveAt(i);
            return elem;
        }

        private void PrintState()
        {
            Console.WriteLine($"deck_size: {mazoPersonajes.Count}");
            Console.WriteLine($"monedas:    {monedas}");
            Console.WriteLine($"popularidad:{popularidad}");
            Console.WriteLine($"gasto:      {gasto}");
            Console.WriteLine($"fuerza:     {fuerza}");

            Console.WriteLine("people in room: " +
                string.Join(", ", peopleRoom.Select(kv => $"{kv.Key}:{kv.Value}")));
            Console.WriteLine("actions avail:  " +
                string.Join(", ", actionsRoom.Select(kv => $"{kv.Key}:{kv.Value}")));
            Console.WriteLine(new string('─', 40));
        }

        /* -------------------------- Bucle principal ------------------------- */

        public void MainLoop()
        {
            int nCartas = 0;

            while (fuerza > 0 && (mazoPersonajes.Count + nCartas) > 0)
            {
                // Refrescar la sala cuando no queden cartas activas
                if (nCartas == 0)
                {
                    LlenarSala();
                    nCartas = CARDS_PER_ROOM;
                }

                PrintState();

                Console.Write("Elegí acción (0‑5): ");
                if (!int.TryParse(Console.ReadLine(), out int actIdx)) continue;

                Console.Write("Elegí carta (0‑3):  ");
                if (!int.TryParse(Console.ReadLine(), out int cardIdx)) continue;

                // Validaciones básicas
                if (!actionsRoom.ContainsKey(actIdx) || actionsRoom[actIdx] == null) continue;
                if (!peopleRoom.ContainsKey(cardIdx) ||  peopleRoom[cardIdx]  == null) continue;

                // Ejecutar
                actionsRoom[actIdx]!.Effect(peopleRoom[cardIdx]!);

                // Marcar como usadas
                actionsRoom[actIdx] = null;
                if (actIdx <= 3)
                {
                    peopleRoom[cardIdx] = null;
                    nCartas--;
                }
            }

            Console.WriteLine("\n¡Juego terminado!");
        }

        private void LlenarSala()
        {
            peopleRoom  = new Dictionary<int, Card?>();
            actionsRoom = new Dictionary<int, Skill?>
            {
                { 0, new Skill("ignorar",  1, Ignorar)  },
                { 1, new Skill("pagar",    1, Pagar)    },
                { 2, new Skill("cobrar",   1, Cobrar)   },
                { 3, new Skill("violencia",1, Violencia)}
            };

            // Rellenar personajes
            for (int i = 0; i < CARDS_PER_ROOM && mazoPersonajes.Count > 0; i++)
                peopleRoom[i] = RobarPersonaje();

            // Rellenar habilidades especiales (slots 4 y 5)
            for (int i = 0; i < SKILLS_PER_ROOM; i++)
            {
                int key = CARDS_PER_ROOM + i;
                actionsRoom[key] = mazoHabilidades.Count > 0 ? RobarHabilidad() : null;
            }
        }

        /* ------------------------------ Main ------------------------------ */
        static void Main()
        {
            new GameManager().MainLoop();
        }
    }
}
