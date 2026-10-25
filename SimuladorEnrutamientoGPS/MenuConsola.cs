using System;

namespace SimuladorEnrutamientoGPS
{
    // Menú principal interactivo por consola.
    // Gestiona el flujo paso a paso: pide dimensiones, genera el mapa, solicita puntos al usuario,
    // ejecuta la búsqueda en anchura (BFS) y muestra el resultado gráfico y la secuencia de pasos.
    public static class MenuConsola
    {
        public static void Iniciar()
        {
            GrafoMapa? mapaActual = null;
            Nodo? inicio = null;
            Nodo? meta = null;
            var solver = new SolucionadorRutas();

            bool salir = false;

            while (!salir)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n=======================================================");
                Console.WriteLine("    SIMULADOR DE ENRUTAMIENTO AUTÓNOMO (MINI GPS)     ");
                Console.WriteLine("=======================================================");
                Console.ResetColor();

                Console.WriteLine("1. Generar nuevo mapa / laberinto");
                Console.WriteLine("2. Seleccionar puntos de Inicio y Destino");
                Console.WriteLine("3. Calcular y visualizar la ruta más corta (BFS)");
                Console.WriteLine("4. Visualizar estado actual del mapa");
                Console.WriteLine("5. Salir");
                Console.Write("\nSeleccione una opción (1-5): ");

                string? opcion = Console.ReadLine();

                switch (opcion?.Trim())
                {
                    case "1":
                        // Reiniciar puntos si se genera un mapa nuevo
                        mapaActual = SolicitarDimensionesYGenerar();
                        inicio = null;
                        meta = null;
                        break;

                    case "2":
                        if (mapaActual == null)
                        {
                            MostrarAviso("Primero debe generar un mapa utilizando la opción 1.");
                        }
                        else
                        {
                            ConfigurarPuntos(mapaActual, ref inicio, ref meta);
                        }
                        break;

                    case "3":
                        if (mapaActual == null || inicio == null || meta == null)
                        {
                            MostrarAviso("Debe generar el mapa y configurar los puntos de partida y llegada antes de buscar una ruta.");
                        }
                        else
                        {
                            CalcularYMostrarRuta(mapaActual, inicio, meta, solver);
                        }
                        break;

                    case "4":
                        if (mapaActual == null)
                        {
                            MostrarAviso("Todavía no se ha generado ningún mapa.");
                        }
                        else
                        {
                            Console.WriteLine("\n--- MAPA ACTUAL ---");
                            mapaActual.MostrarMapa(inicio, meta, solver.RutaEncontrada);
                        }
                        break;

                    case "5":
                        salir = true;
                        Console.WriteLine("\nPrograma finalizado correctamente.");
                        break;

                    default:
                        MostrarAviso("Opción incorrecta. Escriba un número del 1 al 5.");
                        break;
                }
            }
        }

        // Solicita filas y columnas al usuario, repitiendo la petición si ingresa datos incorrectos
        private static GrafoMapa SolicitarDimensionesYGenerar()
        {
            Console.WriteLine("\n--- CONFIGURACIÓN DE DIMENSIONES DEL MAPA ---");
            int filas;
            int columnas;

            while (true)
            {
                Console.Write("Ingrese número de filas (número impar >= 5): ");
                string? input = Console.ReadLine();
                if (ValidadorInterfaz.ValidarDimension(input, out filas, out string err))
                    break;
                MostrarAviso(err);
            }

            while (true)
            {
                Console.Write("Ingrese número de columnas (número impar >= 5): ");
                string? input = Console.ReadLine();
                if (ValidadorInterfaz.ValidarDimension(input, out columnas, out string err))
                    break;
                MostrarAviso(err);
            }

            Console.WriteLine($"\nGenerando laberinto ({filas}x{columnas}) con algoritmo DFS...");
            var mapa = new GrafoMapa(filas, columnas);
            mapa.GenerarLaberintoDFS();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("¡Mapa generado correctamente!");
            Console.ResetColor();

            mapa.MostrarMapa();
            return mapa;
        }

        // Pide las coordenadas para el punto de origen y la meta comprobando que no caigan en paredes
        private static void ConfigurarPuntos(GrafoMapa mapa, ref Nodo? inicio, ref Nodo? meta)
        {
            Console.WriteLine("\n--- SELECCIÓN DE PUNTOS ---");
            Console.WriteLine("Guíese por las coordenadas numéricas en los bordes (# = pared, espacio = calle):");
            mapa.MostrarMapa(inicio, meta);

            // Captura segura de la coordenada de inicio
            while (true)
            {
                Console.WriteLine("\n--- Punto de Inicio (I) ---");
                Console.Write("Fila: ");
                string? x = Console.ReadLine();
                Console.Write("Columna: ");
                string? y = Console.ReadLine();

                if (ValidadorInterfaz.ValidarPuntoTransitable(mapa, x, y, out Nodo? nodoValido, out string err))
                {
                    inicio = nodoValido;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Punto de partida fijado en ({inicio!.X}, {inicio.Y}).");
                    Console.ResetColor();
                    break;
                }
                MostrarAviso(err);
            }

            // Captura segura de la coordenada de meta
            while (true)
            {
                Console.WriteLine("\n--- Punto de Destino (M) ---");
                Console.Write("Fila: ");
                string? x = Console.ReadLine();
                Console.Write("Columna: ");
                string? y = Console.ReadLine();

                if (ValidadorInterfaz.ValidarPuntoTransitable(mapa, x, y, out Nodo? nodoValido, out string err))
                {
                    meta = nodoValido;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Punto de llegada fijado en ({meta!.X}, {meta.Y}).");
                    Console.ResetColor();
                    break;
                }
                MostrarAviso(err);
            }

            Console.WriteLine("\n--- VISTA CON PUNTOS ASIGNADOS ---");
            mapa.MostrarMapa(inicio, meta);
        }

        // Ejecuta el algoritmo BFS y muestra el camino trazado junto a las coordenadas
        private static void CalcularYMostrarRuta(GrafoMapa mapa, Nodo inicio, Nodo meta, SolucionadorRutas solver)
        {
            Console.WriteLine("\nBuscando el camino óptimo mediante Búsqueda en Anchura (BFS)...");
            bool encontrada = solver.BuscarRutaBFS(mapa, inicio, meta);

            if (encontrada && solver.RutaEncontrada != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n¡Ruta encontrada! Longitud del trayecto: {solver.RutaEncontrada.Count} casillas.");
                Console.ResetColor();

                Console.WriteLine("\nMapa con el trayecto marcado (*):");
                mapa.MostrarMapa(inicio, meta, solver.RutaEncontrada);

                Console.Write("Secuencia de pasos: ");
                for (int i = 0; i < solver.RutaEncontrada.Count; i++)
                {
                    var n = solver.RutaEncontrada[i];
                    Console.Write($"({n.X},{n.Y})");
                    if (i < solver.RutaEncontrada.Count - 1) Console.Write(" -> ");
                }
                Console.WriteLine();
            }
            else
            {
                MostrarAviso("No existe un camino accesible entre las dos coordenadas seleccionadas.");
            }
        }

        private static void MostrarAviso(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[AVISO] {mensaje}");
            Console.ResetColor();
        }
    }
}
