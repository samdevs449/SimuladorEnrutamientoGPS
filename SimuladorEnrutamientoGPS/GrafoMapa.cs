using System;

namespace SimuladorEnrutamientoGPS
{
    public class GrafoMapa
    {
        // La matriz bidimensional que representa nuestro mapa
        public Nodo[,] MatrizNodos { get; private set; }
        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        // Constructor: Crea la cuadrícula del tamaño que el usuario pida
        public GrafoMapa(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            MatrizNodos = new Nodo[filas, columnas];

            // Llenar el mapa iterando fila por fila y columna por columna
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    MatrizNodos[i, j] = new Nodo(i, j);
                }
            }
        }
    }
}