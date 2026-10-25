using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorEnrutamientoGPS;

// Pruebas automatizadas de la capa de interfaz y validaciones.
// Verifica que la captura de entradas del usuario sea tolerante a fallos,
// que las reglas de colisión se cumplan y que la visualización del mapa sea consistente.

var resultados = new List<(string Id, string Nombre, bool Paso, string Detalle)>();

// Prueba 1: Filtrado de dimensiones válidas para el generador DFS (impares >= 5)
Ejecutar("TC-INT-01", "Validación de formato y límites en dimensiones del mapa", () =>
{
    bool v1 = ValidadorInterfaz.ValidarDimension("11", out int d1, out string _);
    bool v2 = ValidadorInterfaz.ValidarDimension("10", out int d2, out string err2); // Número par, debe ser rechazado
    bool v3 = ValidadorInterfaz.ValidarDimension("3", out int d3, out string err3);  // Menor a 5, debe ser rechazado
    bool v4 = ValidadorInterfaz.ValidarDimension("25", out int d4, out string _);

    bool paso = (v1 && d1 == 11) &&
                (!v2 && err2.ToLower().Contains("impar")) &&
                (!v3 && err3.Contains("5")) &&
                (v4 && d4 == 25);

    return (paso, $"11 válido={v1}, 10 rechazado={!v2}, 3 rechazado={!v3}, 25 válido={v4}");
});

// Prueba 2: Protección contra entradas no numéricas, cadenas vacías o valores desbordados
Ejecutar("TC-INT-02", "Control de excepciones por entradas de texto corruptas", () =>
{
    string[] casosEntradaInvalida = ["", "   ", "abc", "!@#$", "999999999999999999999", "-15", "12.5"];
    bool todasRechazadasCorrectamente = true;

    foreach (var entrada in casosEntradaInvalida)
    {
        bool valido = ValidadorInterfaz.ValidarDimension(entrada, out _, out string error);
        if (valido || string.IsNullOrWhiteSpace(error))
        {
            todasRechazadasCorrectamente = false;
            break;
        }
    }

    return (todasRechazadasCorrectamente, $"Se evaluaron {casosEntradaInvalida.Length} entradas anómalas sin interrupciones del programa.");
});

// Prueba 3: Comprobación de límites y rechazo de casillas con pared
Ejecutar("TC-INT-03", "Detección de obstáculos y control de límites de la matriz", () =>
{
    var mapa = new GrafoMapa(7, 7);
    for (int i = 0; i < 7; i++)
        for (int j = 0; j < 7; j++)
            mapa.MatrizNodos[i, j].EsPared = true;

    // Habilitar casillas transitables para la prueba
    mapa.MatrizNodos[1, 1].EsPared = false;
    mapa.MatrizNodos[1, 2].EsPared = false;

    // Casilla libre (1,1) debe ser aceptada
    bool libreAceptada = ValidadorInterfaz.ValidarPuntoTransitable(mapa, "1", "1", out Nodo? n1, out _);

    // Casilla con obstáculo (0,0) debe ser denegada
    bool obstaculoRechazado = !ValidadorInterfaz.ValidarPuntoTransitable(mapa, "0", "0", out _, out string errPared) &&
                              errPared.ToLower().Contains("pared");

    // Coordenada fuera de límites (10,10) debe ser denegada
    bool fueraDeRangoRechazado = !ValidadorInterfaz.ValidarPuntoTransitable(mapa, "10", "10", out _, out string errRango) &&
                                 errRango.ToLower().Contains("fuera del mapa");

    bool paso = libreAceptada && (n1 != null && n1.X == 1 && n1.Y == 1) && obstaculoRechazado && fueraDeRangoRechazado;

    return (paso, $"Libre={libreAceptada}, ObstáculoRechazado={obstaculoRechazado}, FueraDeRangoRechazado={fueraDeRangoRechazado}");
});

// Prueba 4: Verificación visual de los símbolos del mapa generado
Ejecutar("TC-INT-04", "Generación correcta de caracteres visuales en consola", () =>
{
    var mapa = new GrafoMapa(5, 5);
    for (int i = 0; i < 5; i++)
        for (int j = 0; j < 5; j++)
            mapa.MatrizNodos[i, j].EsPared = true;

    mapa.MatrizNodos[1, 1].EsPared = false;
    mapa.MatrizNodos[1, 2].EsPared = false;
    mapa.MatrizNodos[1, 3].EsPared = false;

    var inicio = mapa.MatrizNodos[1, 1];
    var meta = mapa.MatrizNodos[1, 3];
    var ruta = new List<Nodo> { inicio, mapa.MatrizNodos[1, 2], meta };

    string salidaVisual = mapa.ObtenerMapaTexto(inicio, meta, ruta);

    bool tieneInicio = salidaVisual.Contains('I');
    bool tieneMeta = salidaVisual.Contains('M');
    bool tieneRuta = salidaVisual.Contains('*');
    bool tienePared = salidaVisual.Contains('#');

    bool paso = tieneInicio && tieneMeta && tieneRuta && tienePared;

    return (paso, $"Símbolos presentes: Inicio (I)={tieneInicio}, Meta (M)={tieneMeta}, Ruta (*)={tieneRuta}, Pared (#)={tienePared}");
});

// Prueba 5: Ejecución integrada de un ciclo completo de uso
Ejecutar("TC-INT-05", "Flujo completo de simulación (Dimensiones -> DFS -> Puntos -> BFS -> Visualización)", () =>
{
    // Simulación de entrada válida
    bool dimOk = ValidadorInterfaz.ValidarDimension("9", out int tam, out _);
    var mapa = new GrafoMapa(tam, tam);
    mapa.GenerarLaberintoDFS();

    // Localizar casillas libres para probar el recorrido
    Nodo? inicio = null;
    Nodo? meta = null;

    for (int i = 1; i < tam - 1 && inicio == null; i++)
        for (int j = 1; j < tam - 1 && inicio == null; j++)
            if (!mapa.MatrizNodos[i, j].EsPared) inicio = mapa.MatrizNodos[i, j];

    for (int i = tam - 2; i >= 1 && meta == null; i--)
        for (int j = tam - 2; j >= 1 && meta == null; j--)
            if (!mapa.MatrizNodos[i, j].EsPared && !ReferenceEquals(mapa.MatrizNodos[i, j], inicio))
                meta = mapa.MatrizNodos[i, j];

    bool puntosOk = ValidadorInterfaz.ValidarParInicioDestino(inicio, meta, out _);

    var solver = new SolucionadorRutas();
    bool rutaOk = solver.BuscarRutaBFS(mapa, inicio!, meta!);

    string visual = mapa.ObtenerMapaTexto(inicio, meta, solver.RutaEncontrada);
    bool visualOk = !string.IsNullOrEmpty(visual) && visual.Contains('I');

    bool paso = dimOk && puntosOk && rutaOk && visualOk && solver.RutaEncontrada?.Count > 0;

    return (paso, $"Generación={dimOk}, SelecciónPuntos={puntosOk}, SoluciónBFS={rutaOk} ({solver.RutaEncontrada?.Count ?? 0} pasos), Dibujado={visualOk}");
});

// Imprimir reporte de resultados
Console.WriteLine();
Console.WriteLine("==================== REPORTE DE PRUEBAS DE INTERFAZ ====================");
foreach (var r in resultados)
{
    Console.WriteLine($"{r.Id} | {(r.Paso ? "PASS" : "FAIL")} | {r.Nombre}\n   {r.Detalle}");
}
Console.WriteLine("------------------------------------------------------------------------");
Console.WriteLine($"Total evaluadas: {resultados.Count} | Aprobadas: {resultados.Count(r => r.Paso)} | Fallidas: {resultados.Count(r => !r.Paso)}");
Console.WriteLine("========================================================================");

void Ejecutar(string id, string nombre, Func<(bool Paso, string Detalle)> prueba)
{
    try
    {
        var res = prueba();
        resultados.Add((id, nombre, res.Paso, res.Detalle));
    }
    catch (Exception ex)
    {
        resultados.Add((id, nombre, false, $"Excepción no esperada: {ex.GetType().Name}: {ex.Message}"));
    }
}
