# Simulador de Enrutamiento Urbano (Mini GPS)

## Descripción
Este proyecto es un simulador de navegación autónoma basado en estructuras de datos. Genera laberintos o mapas cuadriculados que representan entornos urbanos con obstáculos estáticos, y calcula la ruta más eficiente entre un punto de origen y un destino.

## Estructuras de Datos Implementadas
* **Grafos:** Representación topológica del mapa (Nodos y adyacencias en `Nodo.cs` y `GrafoMapa.cs`).
* **Pilas (Stacks):** Utilizadas para el algoritmo de generación de mapas mediante Búsqueda en Profundidad (DFS) y backtracking.
* **Colas (Queues):** Utilizadas en el motor de resolución de rutas mediante Búsqueda en Anchura (BFS) para garantizar el camino más corto.
* **Tablas Hash (Dictionaries):** Utilizadas para registrar nodos visitados y trazar la ruta de regreso en tiempo $O(1)$.

## Requisitos de Ejecución
* .NET SDK 10.0 (o superior).
* Visual Studio 2022 / Visual Studio Code / Terminal de comandos.

## Instrucciones de Ejecución

### 1. Ejecutar el Simulador Interactivo (Consola)
Desde la terminal, en la carpeta raíz del proyecto:
```bash
dotnet run --project SimuladorEnrutamientoGPS\SimuladorEnrutamientoGPS.csproj
```
El menú permite:
* Generar mapas aleatorios de dimensiones personalizadas.
* Seleccionar coordenadas de inicio `(I)` y meta `(M)` con validación de colisiones contra paredes.
* Resolver la ruta óptima mediante BFS y ver el trazado del camino en consola.

### 2. Ejecutar Pruebas Automatizadas de QA
```bash
dotnet run --project SimuladorEnrutamientoGPS\QA_Automatizada\SimuladorEnrutamientoGPS.QA.csproj
```
Evalúa 5 casos de prueba funcionales y bordes del núcleo de búsqueda (TC-01 a TC-05).

### 3. Ejecutar Pruebas de Interfaz y Validaciones
```bash
dotnet run --project SimuladorEnrutamientoGPS\Tests_Interfaz\SimuladorEnrutamientoGPS.TestsInterfaz.csproj
```
Evalúa 5 casos de prueba de resiliencia ante errores de teclado, rechazo de paredes y flujo integrado (TC-INT-01 a TC-INT-05).

## Documentación del Proyecto
* `roles_del_proyecto.md`: Asignación de roles y responsabilidades según PED Fase 2.
* `SimuladorEnrutamientoGPS/Documentacion_QA/`:
  * `Informe_QA_Bitacora_Coherencia.docx`: Informe de aseguramiento de calidad, bitácora y coherencia con Fase 1.
  * `Matriz_Pruebas_y_Bitacora_QA.xlsx`: Matriz de pruebas y registro de incidencias técnicas.
* `SimuladorEnrutamientoGPS/Documentacion_Entrega/`:
  * `Guion_Video_Demostracion.md`: Guion técnico con minutaje para el video demostrativo.
  * `Documento_Autocritica.md`: Informe de autoevaluación, aspectos por pulir y plan de acción hacia la Fase 3.
