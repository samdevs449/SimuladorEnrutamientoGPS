using System;
using System.Collections.Generic;

namespace SimuladorEnrutamientoGPS
{
    public class SolucionadorRutas
    {
        // Agregamos "?" para decirle a C# que acepta nulos al inicio
        public List<Nodo>? RutaEncontrada { get; private set; }

        public bool BuscarRutaBFS(GrafoMapa mapa, Nodo inicio, Nodo meta)
        {
            Queue<Nodo> colaFrontera = new Queue<Nodo>();

            // Agregamos "?" al valor del diccionario
            Dictionary<Nodo, Nodo?> tablaPadres = new Dictionary<Nodo, Nodo?>();

            RutaEncontrada = new List<Nodo>();

            colaFrontera.Enqueue(inicio);
            inicio.Visitado = true;
            tablaPadres[inicio] = null;

            while (colaFrontera.Count > 0)
            {
                Nodo actual = colaFrontera.Dequeue();

                if (actual == meta)
                {
                    RutaEncontrada = ReconstruirCamino(tablaPadres, meta);
                    return true;
                }

                List<Nodo> vecinos = ObtenerVecinosAccesibles(mapa, actual);

                foreach (Nodo vecino in vecinos)
                {
                    if (!vecino.Visitado)
                    {
                        vecino.Visitado = true;
                        tablaPadres[vecino] = actual;
                        colaFrontera.Enqueue(vecino);
                    }
                }
            }

            return false;
        }

        private List<Nodo> ObtenerVecinosAccesibles(GrafoMapa mapa, Nodo nodo)
        {
            List<Nodo> vecinos = new List<Nodo>();
            int x = nodo.X;
            int y = nodo.Y;

            if (x - 1 >= 0 && !mapa.MatrizNodos[x - 1, y].EsPared) vecinos.Add(mapa.MatrizNodos[x - 1, y]);
            if (x + 1 < mapa.Filas && !mapa.MatrizNodos[x + 1, y].EsPared) vecinos.Add(mapa.MatrizNodos[x + 1, y]);
            if (y - 1 >= 0 && !mapa.MatrizNodos[x, y - 1].EsPared) vecinos.Add(mapa.MatrizNodos[x, y - 1]);
            if (y + 1 < mapa.Columnas && !mapa.MatrizNodos[x, y + 1].EsPared) vecinos.Add(mapa.MatrizNodos[x, y + 1]);

            return vecinos;
        }

        // Agregamos "?" en el diccionario y en la variable actual
        private List<Nodo> ReconstruirCamino(Dictionary<Nodo, Nodo?> padres, Nodo meta)
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