using System;
using System.Collections.Generic; // Importante: Nos permite usar las Pilas (Stack) y Listas

namespace SimuladorEnrutamientoGPS
{
    public class GrafoMapa
    {
        // La matriz bidimensional que representa nuestro mapa
        public Nodo[,] MatrizNodos { get; private set; }
        public int Filas { get; private set; }
        public int Columnas { get; private set; }
        private Random aleatorio = new Random(); // Para elegir caminos al azar

        // Constructor: Crea la cuadrícula llena de paredes
        public GrafoMapa(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            MatrizNodos = new Nodo[filas, columnas];

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    MatrizNodos[i, j] = new Nodo(i, j);
                }
            }
        }

        // --- MÉTODO PRINCIPAL: GENERADOR DE LABERINTO (DFS) ---
        public void GenerarLaberintoDFS()
        {
            // 1. ESTRUCTURA APLICADA: PILA (Stack - LIFO) para el Backtracking
            Stack<Nodo> pilaPendientes = new Stack<Nodo>();

            // 2. Seleccionar posición inicial (empezamos en 1,1 para dejar los bordes del mapa como pared)
            Nodo nodoInicio = MatrizNodos[1, 1];
            nodoInicio.EsPared = false;
            nodoInicio.Visitado = true;

            // 3. Apilar el inicio
            pilaPendientes.Push(nodoInicio);

            // 4. Mientras la pila no esté vacía...
            while (pilaPendientes.Count > 0)
            {
                // Desapilar el nodo actual
                Nodo actual = pilaPendientes.Pop();

                // Identificar vecinos válidos (saltando de 2 en 2 para que queden paredes de por medio)
                List<Nodo> vecinos = ObtenerVecinosValidos(actual);

                if (vecinos.Count > 0)
                {
                    // Volver a apilar el actual para poder retroceder a él luego (Backtracking)
                    pilaPendientes.Push(actual);

                    // Seleccionar un vecino al azar
                    int indiceAleatorio = aleatorio.Next(vecinos.Count);
                    Nodo vecinoSeleccionado = vecinos[indiceAleatorio];

                    // Romper la pared que está en medio del nodo actual y el vecino seleccionado
                    int paredX = actual.X + (vecinoSeleccionado.X - actual.X) / 2;
                    int paredY = actual.Y + (vecinoSeleccionado.Y - actual.Y) / 2;
                    MatrizNodos[paredX, paredY].EsPared = false;

                    // Marcar el vecino como transitable y visitado
                    vecinoSeleccionado.EsPared = false;
                    vecinoSeleccionado.Visitado = true;

                    // Apilar el vecino para continuar el camino desde ahí
                    pilaPendientes.Push(vecinoSeleccionado);
                }
            }

            // Limpiamos el rastro de visitas para que la Búsqueda de Ruta (BFS) lo encuentre limpio
            ReiniciarVisitados();
        }

        // --- MÉTODOS AUXILIARES ---

        // Busca vecinos que aún son pared y no han sido visitados
        private List<Nodo> ObtenerVecinosValidos(Nodo nodo)
        {
            List<Nodo> vecinos = new List<Nodo>();
            int x = nodo.X;
            int y = nodo.Y;

            // Revisamos en las 4 direcciones ortogonales (Regla de negocio #2)
            if (x - 2 > 0 && !MatrizNodos[x - 2, y].Visitado) vecinos.Add(MatrizNodos[x - 2, y]); // Arriba
            if (x + 2 < Filas - 1 && !MatrizNodos[x + 2, y].Visitado) vecinos.Add(MatrizNodos[x + 2, y]); // Abajo
            if (y - 2 > 0 && !MatrizNodos[x, y - 2].Visitado) vecinos.Add(MatrizNodos[x, y - 2]); // Izquierda
            if (y + 2 < Columnas - 1 && !MatrizNodos[x, y + 2].Visitado) vecinos.Add(MatrizNodos[x, y + 2]); // Derecha

            return vecinos;
        }

        private void ReiniciarVisitados()
        {
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    MatrizNodos[i, j].Visitado = false;
                }
            }
        }
    }
}