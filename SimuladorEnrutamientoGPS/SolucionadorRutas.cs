
using System;
using System.Collections.Generic;

namespace SimuladorEnrutamientoGPS
{
    public class SolucionadorRutas
    {
        public List<Nodo>? RutaEncontrada { get; private set; }

        public bool BuscarRutaBFS(GrafoMapa mapa, Nodo inicio, Nodo meta)
        {
            // Reiniciar el estado de todos los nodos antes de cada búsqueda.
            for (int i = 0; i < mapa.Filas; i++)
            {
                for (int j = 0; j < mapa.Columnas; j++)
                {
                    mapa.MatrizNodos[i, j].Visitado = false;
                }
            }

            // Preparar las estructuras de la búsqueda BFS.
            Queue<Nodo> colaFrontera = new Queue<Nodo>();
            Dictionary<Nodo, Nodo?> tablaPadres =
                new Dictionary<Nodo, Nodo?>();

            RutaEncontrada = new List<Nodo>();

            // Validar que el inicio y la meta sean transitables.
            if (inicio.EsPared || meta.EsPared)
            {
                return false;
            }

            colaFrontera.Enqueue(inicio);
            inicio.Visitado = true;
            tablaPadres[inicio] = null;

            while (colaFrontera.Count > 0)
            {
                Nodo actual = colaFrontera.Dequeue();

                // Si llegamos a la meta, reconstruimos el camino.
                if (ReferenceEquals(actual, meta))
                {
                    RutaEncontrada = ReconstruirCamino(tablaPadres, meta);
                    return true;
                }

                List<Nodo> vecinos = ObtenerVecinosAccesibles(mapa, actual);

                foreach (Nodo vecino in vecinos)
                {
                    if (!vecino.EsPared && !vecino.Visitado)
                    {
                        vecino.Visitado = true;
                        tablaPadres[vecino] = actual;
                        colaFrontera.Enqueue(vecino);
                    }
                }
            }

            // No se encontró una ruta.
            RutaEncontrada.Clear();
            return false;
        }

        private List<Nodo> ObtenerVecinosAccesibles(
            GrafoMapa mapa,
            Nodo nodo)
        {
            List<Nodo> vecinos = new List<Nodo>();

            int x = nodo.X;
            int y = nodo.Y;

            if (x - 1 >= 0 &&
                !mapa.MatrizNodos[x - 1, y].EsPared)
            {
                vecinos.Add(mapa.MatrizNodos[x - 1, y]);
            }

            if (x + 1 < mapa.Filas &&
                !mapa.MatrizNodos[x + 1, y].EsPared)
            {
                vecinos.Add(mapa.MatrizNodos[x + 1, y]);
            }

            if (y - 1 >= 0 &&
                !mapa.MatrizNodos[x, y - 1].EsPared)
            {
                vecinos.Add(mapa.MatrizNodos[x, y - 1]);
            }

            if (y + 1 < mapa.Columnas &&
                !mapa.MatrizNodos[x, y + 1].EsPared)
            {
                vecinos.Add(mapa.MatrizNodos[x, y + 1]);
            }

            return vecinos;
        }

        private List<Nodo> ReconstruirCamino(
            Dictionary<Nodo, Nodo?> padres,
            Nodo meta)
        {
            List<Nodo> camino = new List<Nodo>();
            Nodo? actual = meta;

            while (actual != null)
            {
                camino.Add(actual);
                actual = padres[actual];
            }

            camino.Reverse();
            return camino;
        }
    }
}
