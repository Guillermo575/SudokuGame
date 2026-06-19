using System;
namespace Sudoku.Tools
{
    /// <summary>
    /// Clase de utilidad para entrenar y usar el generador de Sudoku con Machine Learning
    /// </summary>
    public class SudokuMLHelper
    {
        /// <summary>
        /// Entrena el agente de ML con diferentes tamaños de Sudoku
        /// Recomendado ejecutar antes del primer uso
        /// </summary>
        /// <param name="episodiosPorTamanio">Cantidad de sudokus a generar por cada tamaño</param>
        public static void EntrenamientoCompleto(int episodiosPorTamanio = 500)
        {
            Console.WriteLine("=== ENTRENAMIENTO DEL AGENTE DE MACHINE LEARNING ===\n");

            // Entrenar con diferentes tamaños
            Console.WriteLine("Fase 1: Entrenamiento con Sudoku 3x3 (9x9)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio, 3, 3);

            Console.WriteLine("\nFase 2: Entrenamiento con Sudoku 2x3 (6x6)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio / 2, 2, 3);

            Console.WriteLine("\nFase 3: Entrenamiento con Sudoku 4x4 (16x16)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio * 2, 4, 4);

            Console.WriteLine("\n=== ENTRENAMIENTO COMPLETADO ===");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
        }

        /// <summary>
        /// Compara el rendimiento con y sin Machine Learning
        /// </summary>
        public static void CompararRendimiento(int cantidadPruebas = 100, int columnasX = 4, int columnasY = 4)
        {
            Console.WriteLine($"\n=== COMPARACIÓN DE RENDIMIENTO ({columnasX}x{columnasY}) ===\n");

            // Prueba SIN Machine Learning
            Console.WriteLine($"Generando {cantidadPruebas} Sudokus SIN Machine Learning...");
            var estadisticasSinML = EjecutarPruebas(cantidadPruebas, columnasX, columnasY, usarML: false);

            // Prueba CON Machine Learning
            Console.WriteLine($"\nGenerando {cantidadPruebas} Sudokus CON Machine Learning...");
            var estadisticasConML = EjecutarPruebas(cantidadPruebas, columnasX, columnasY, usarML: true);

            // Mostrar resultados
            Console.WriteLine("\n--- RESULTADOS ---");
            Console.WriteLine("\nSIN Machine Learning:");
            MostrarEstadisticas(estadisticasSinML);

            Console.WriteLine("\nCON Machine Learning:");
            MostrarEstadisticas(estadisticasConML);

            // Calcular mejora
            double mejoraTiempo = estadisticasSinML.TiempoPromedio == 0 ? 0 :((estadisticasSinML.TiempoPromedio - estadisticasConML.TiempoPromedio) 
                                   / estadisticasSinML.TiempoPromedio) * 100;
            double mejoraBacktrack = estadisticasSinML.BacktrackingPromedio == 0 ? 0 :((estadisticasSinML.BacktrackingPromedio - estadisticasConML.BacktrackingPromedio) 
                                      / estadisticasSinML.BacktrackingPromedio) * 100;

            Console.WriteLine("\n--- MEJORAS CON MACHINE LEARNING ---");
            Console.WriteLine($"Reducción en tiempo: {mejoraTiempo:F2}%");
            Console.WriteLine($"Reducción en backtracking: {mejoraBacktrack:F2}%");
            Console.WriteLine($"Mejora en tasa de éxito: {estadisticasConML.TasaExito - estadisticasSinML.TasaExito:F2}%");
        }

        private static Estadisticas EjecutarPruebas(int cantidad, int columnasX, int columnasY, bool usarML)
        {
            var stats = new Estadisticas();
            int exitosos = 0;

            for (int i = 0; i < cantidad; i++)
            {
                var sudoku = new SudokuGenerator(columnasX, columnasY, usarML, entrenar: false);

                if (sudoku.Exito)
                {
                    exitosos++;
                    stats.TiempoTotal += sudoku.TiempoEjecutado;
                    stats.BacktrackingTotal += sudoku.ConteoErrores;
                    stats.BacktrackingMin = Math.Min(stats.BacktrackingMin, sudoku.ConteoErrores);
                    stats.BacktrackingMax = Math.Max(stats.BacktrackingMax, sudoku.ConteoErrores);
                }
            }

            stats.CantidadExitosos = exitosos;
            stats.TasaExito = (double)exitosos / cantidad * 100;
            stats.TiempoPromedio = exitosos > 0 ? stats.TiempoTotal / exitosos : 0;
            stats.BacktrackingPromedio = exitosos > 0 ? stats.BacktrackingTotal / exitosos : 0;

            return stats;
        }

        private static void MostrarEstadisticas(Estadisticas stats)
        {
            Console.WriteLine($"  Tasa de éxito: {stats.TasaExito:F2}% ({stats.CantidadExitosos} exitosos)");
            Console.WriteLine($"  Tiempo promedio: {stats.TiempoPromedio:F2} ms");
            Console.WriteLine($"  Backtracking promedio: {stats.BacktrackingPromedio:F2}");
            Console.WriteLine($"  Backtracking mínimo: {stats.BacktrackingMin}");
            Console.WriteLine($"  Backtracking máximo: {stats.BacktrackingMax}");
        }

        private class Estadisticas
        {
            public int CantidadExitosos = 0;
            public double TasaExito = 0;
            public long TiempoTotal = 0;
            public long TiempoPromedio = 0;
            public int BacktrackingTotal = 0;
            public int BacktrackingPromedio = 0;
            public int BacktrackingMin = int.MaxValue;
            public int BacktrackingMax = 0;
        }

        /// <summary>
        /// Ejemplo de uso básico
        /// </summary>
        public static void EjemploUso()
        {
            Console.WriteLine("=== EJEMPLO DE USO ===\n");

            // 1. Generar un Sudoku tradicional (sin ML)
            Console.WriteLine("1. Generando Sudoku 3x3 sin Machine Learning...");
            var sudokuTradicional = new SudokuGenerator(3, 3, usarML: false);
            Console.WriteLine($"   Backtracking: {sudokuTradicional.ConteoErrores}");
            Console.WriteLine($"   Tiempo: {sudokuTradicional.TiempoEjecutado} ms");
            Console.WriteLine($"   Éxito: {sudokuTradicional.Exito}");

            // 2. Generar un Sudoku con ML (sin entrenar)
            Console.WriteLine("\n2. Generando Sudoku 3x3 con Machine Learning (modelo inicial)...");
            var sudokuML = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudokuML.ConteoErrores}");
            Console.WriteLine($"   Tiempo: {sudokuML.TiempoEjecutado} ms");
            Console.WriteLine($"   Éxito: {sudokuML.Exito}");

            // 3. Entrenar el agente
            Console.WriteLine("\n3. Entrenando el agente con 100 episodios...");
            SudokuGenerator.EntrenarAgente(100, 3, 3);

            // 4. Generar un Sudoku con ML entrenado
            Console.WriteLine("\n4. Generando Sudoku 3x3 con Machine Learning (modelo entrenado)...");
            var sudokuMLEntrenado = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudokuMLEntrenado.ConteoErrores}");
            Console.WriteLine($"   Tiempo: {sudokuMLEntrenado.TiempoEjecutado} ms");
            Console.WriteLine($"   Éxito: {sudokuMLEntrenado.Exito}");
            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");

            // 5. Generar Sudoku 4x4 (el más problemático)
            Console.WriteLine("\n5. Generando Sudoku 4x4 con Machine Learning...");
            var sudoku4x4 = new SudokuGenerator(4, 4, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudoku4x4.ConteoErrores}");
            Console.WriteLine($"   Tiempo: {sudoku4x4.TiempoEjecutado} ms");
            Console.WriteLine($"   Éxito: {sudoku4x4.Exito}");
        }
    }
}