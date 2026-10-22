# Simulador de Enrutamiento Urbano (Mini GPS)

## Descripción
Este proyecto es un simulador de navegación autónoma basado en estructuras de datos. Genera laberintos o mapas cuadriculados que representan entornos urbanos con obstáculos estáticos, y calcula la ruta más eficiente entre un punto de origen y un destino.

## Estructuras de Datos Implementadas
* **Grafos:** Representación topológica del mapa (Nodos y adyacencias).
* **Pilas (Stacks):** Utilizadas para el algoritmo de generación de mapas mediante Búsqueda en Profundidad (DFS) y backtracking.
* **Colas (Queues):** Utilizadas en el motor de resolución de rutas mediante Búsqueda en Anchura (BFS) para garantizar el camino más corto.
* **Tablas Hash (Dictionaries):** Utilizadas para registrar nodos visitados y trazar la ruta de regreso en tiempo O(1).

## Requisitos de Ejecución
* .NET SDK 6.0 o superior.
* Visual Studio 2022 / Visual Studio Code.

## Fase Actual: Etapa 2 (Avance Funcional)
El sistema actual es capaz de inicializar las clases base (Nodos y Grafos) y ejecutar la lógica de generación y resolución por consola.