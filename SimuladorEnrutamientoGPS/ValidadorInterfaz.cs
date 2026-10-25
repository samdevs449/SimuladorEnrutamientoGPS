using System;

namespace SimuladorEnrutamientoGPS
{
    // Clase auxiliar para centralizar las comprobaciones de entrada del usuario en consola.
    // Evita excepciones durante la captura de datos (números inválidos, letras, valores fuera de rango)
    // y verifica que las coordenadas elegidas no choquen contra muros del laberinto.
    public static class ValidadorInterfaz
    {
        // Se define 5 como tamaño mínimo para tener al menos un camino interno rodeado de paredes perimetrales.
        public const int DimensionMinima = 5;
        public const int DimensionMaxima = 99;

        // Comprueba que el texto ingresado sea un número entero, positivo e impar.
        // El algoritmo de generación por DFS requiere dimensiones impares para poder alternar pasillos y paredes intermedias.
        public static bool ValidarDimension(string? entrada, out int dimension, out string mensajeError)
        {
            dimension = 0;
            if (string.IsNullOrWhiteSpace(entrada))
            {
                mensajeError = "No se ingresó ningún valor. Debe escribir un número entero.";
                return false;
            }

            // Usamos TryParse para que el programa no se detenga por FormatException si el usuario mete letras o símbolos
            if (!int.TryParse(entrada.Trim(), out dimension))
            {
                mensajeError = $"El valor '{entrada}' no es un número entero válido.";
                return false;
            }

            if (dimension < DimensionMinima)
            {
                mensajeError = $"El tamaño mínimo del mapa es de {DimensionMinima}x{DimensionMinima} para generar caminos transitables.";
                return false;
            }

            if (dimension > DimensionMaxima)
            {
                mensajeError = $"El tamaño no debe superar {DimensionMaxima} para que se pueda visualizar correctamente en la consola.";
                return false;
            }

            // Verificamos si es número par
            if (dimension % 2 == 0)
            {
                mensajeError = $"La medida debe ser un número impar (por ejemplo {dimension + 1}) para que el generador pueda dejar paredes intermedias.";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        // Verifica que las coordenadas (x, y) existan dentro de la matriz y que la casilla seleccionada esté despejada (no sea pared).
        public static bool ValidarPuntoTransitable(GrafoMapa mapa, string? entradaX, string? entradaY, out Nodo? nodoValido, out string mensajeError)
        {
            nodoValido = null;

            if (mapa == null)
            {
                mensajeError = "No se ha inicializado el mapa.";
                return false;
            }

            // Conversión segura de ambas coordenadas
            if (!int.TryParse(entradaX?.Trim(), out int x) || !int.TryParse(entradaY?.Trim(), out int y))
            {
                mensajeError = "Las coordenadas deben ser números enteros.";
                return false;
            }

            // Comprobación de límites dentro de la matriz
            if (x < 0 || x >= mapa.Filas || y < 0 || y >= mapa.Columnas)
            {
                mensajeError = $"La posición ({x}, {y}) queda fuera del mapa. Rango permitido: Filas de 0 a {mapa.Filas - 1}, Columnas de 0 a {mapa.Columnas - 1}.";
                return false;
            }

            // Validación de colisión contra obstáculos estáticos
            Nodo nodo = mapa.MatrizNodos[x, y];
            if (nodo.EsPared)
            {
                mensajeError = $"La casilla ({x}, {y}) es una pared (#). Por favor elija una casilla que esté libre (camino).";
                return false;
            }

            nodoValido = nodo;
            mensajeError = string.Empty;
            return true;
        }

        // Valida que ambos nodos existan y no sean obstáculos antes de lanzar el algoritmo de búsqueda
        public static bool ValidarParInicioDestino(Nodo? inicio, Nodo? destino, out string mensajeError)
        {
            if (inicio == null || destino == null)
            {
                mensajeError = "Debe definir tanto el punto de partida como el destino.";
                return false;
            }

            if (inicio.EsPared || destino.EsPared)
            {
                mensajeError = "El punto inicial y el punto de llegada deben ser casillas transitables (no pueden ser pared).";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }
    }
}
