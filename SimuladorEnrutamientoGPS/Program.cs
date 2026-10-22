using System;
using System.Collections.Generic;

namespace SimuladorEnrutamientoGPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SIMULADOR DE ENRUTAMIENTO URBANO (MINI GPS) ===");
            Console.WriteLine("Inicializando motor de estructuras de datos...\n");

            // 1. Crear el mapa de 15x15
            int filas = 15;
            int columnas = 15;
            GrafoMapa mapa = new GrafoMapa(filas, columnas);

            // 2. Generar el laberinto utilizando DFS (Pilas)
            mapa.GenerarLaberintoDFS();
            Console.WriteLine("Laberinto generado con éxito (DFS - Pilas).");

            // 3. Definir las coordenadas de Inicio y Meta
            Nodo inicio = mapa.MatrizNodos[1, 1];
            Nodo meta = mapa.MatrizNodos[filas - 2, columnas - 2];

            // Garantizar por regla de negocio que inicio y meta sean transitables
            inicio.EsPared = false;
            meta.EsPared = false;

            // 4. Resolver utilizando BFS (Colas y Tablas Hash)
            SolucionadorRutas gps = new SolucionadorRutas();
            bool rutaEncontrada = gps.BuscarRutaBFS(mapa, inicio, meta);

            // 5. Mostrar los resultados visuales
            if (rutaEncontrada)
            {
                Console.WriteLine("¡Ruta óptima encontrada con éxito (BFS - Colas)!\n");
                DibujarMapa(mapa, gps.RutaEncontrada);
            }
            else
            {
                Console.WriteLine("Error: No se encontró una ruta de salida.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        // Método auxiliar para imprimir la matriz en la consola negra con colores
        static void DibujarMapa(GrafoMapa mapa, List<Nodo> ruta)
        {
            for (int i = 0; i < mapa.Filas; i++)
            {
                for (int j = 0; j < mapa.Columnas; j++)
                {
                    Nodo actual = mapa.MatrizNodos[i, j];

                    if (ruta.Contains(actual))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("██"); // Imprime la ruta en Verde
                        Console.ResetColor();
                    }
                    else if (actual.EsPared)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("██"); // Imprime las paredes en Gris oscuro
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("  "); // Imprime espacio en blanco para caminos libres
                    }
                }
                Console.WriteLine(); // Salto de línea al terminar la fila
            }
        }
    }
}