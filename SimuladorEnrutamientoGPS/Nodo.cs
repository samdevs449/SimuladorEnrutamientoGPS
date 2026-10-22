using System;

namespace SimuladorEnrutamientoGPS
{
    public class Nodo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool EsPared { get; set; }
        public bool Visitado { get; set; }

        // Constructor: Define cómo nace el nodo
        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
            // Según nuestro pseudocódigo, al inicio todo el mapa es pared
            EsPared = true;
            Visitado = false;
        }
    }
}