using System;
namespace SudokuML.Tools
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
            Console.WriteLine("=== ML AGENT TRAINING ===\n");

            // Train with different sizes
            Console.WriteLine("Phase 1: Training with 3x3 Sudoku (9x9)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio, 3, 3);

            Console.WriteLine("\nPhase 2: Training with 2x3 Sudoku (6x6)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio / 2, 2, 3);

            Console.WriteLine("\nPhase 3: Training with 4x4 Sudoku (16x16)");
            SudokuGenerator.EntrenarAgente(episodiosPorTamanio * 2, 4, 4);

            Console.WriteLine("\n=== TRAINING COMPLETED ===");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
        }

        /// <summary>
        /// Compara el rendimiento con y sin Machine Learning
        /// </summary>
        public static void CompararRendimiento(int cantidadPruebas = 100, int columnasX = 4, int columnasY = 4)
        {
            Console.WriteLine($"\n=== PERFORMANCE COMPARISON ({columnasX}x{columnasY}) ===\n");

            // Test WITHOUT Machine Learning
            Console.WriteLine($"Generating {cantidadPruebas} Sudokus WITHOUT Machine Learning...");
            var estadisticasSinML = EjecutarPruebas(cantidadPruebas, columnasX, columnasY, usarML: false);

            // Test WITH Machine Learning
            Console.WriteLine($"\nGenerating {cantidadPruebas} Sudokus WITH Machine Learning...");
            var estadisticasConML = EjecutarPruebas(cantidadPruebas, columnasX, columnasY, usarML: true);

            // Show results
            Console.WriteLine("\n--- RESULTS ---");
            Console.WriteLine("\nWITHOUT Machine Learning:");
            MostrarEstadisticas(estadisticasSinML);

            Console.WriteLine("\nWITH Machine Learning:");
            MostrarEstadisticas(estadisticasConML);

            // Calculate improvement
            double mejoraTiempo = estadisticasSinML.TiempoPromedio == 0 ? 0 :((estadisticasSinML.TiempoPromedio - estadisticasConML.TiempoPromedio) 
                                   / estadisticasSinML.TiempoPromedio) * 100;
            double mejoraBacktrack = estadisticasSinML.BacktrackingPromedio == 0 ? 0 :((estadisticasSinML.BacktrackingPromedio - estadisticasConML.BacktrackingPromedio) 
                                      / estadisticasSinML.BacktrackingPromedio) * 100;

            Console.WriteLine("\n--- IMPROVEMENTS WITH MACHINE LEARNING ---");
            Console.WriteLine($"Time reduction: {mejoraTiempo:F2}%");
            Console.WriteLine($"Backtracking reduction: {mejoraBacktrack:F2}%");
            Console.WriteLine($"Success rate improvement: {estadisticasConML.TasaExito - estadisticasSinML.TasaExito:F2}%");
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
            Console.WriteLine($"  Success rate: {stats.TasaExito:F2}% ({stats.CantidadExitosos} successful)");
            Console.WriteLine($"  Average time: {stats.TiempoPromedio:F2} ms");
            Console.WriteLine($"  Average backtracking: {stats.BacktrackingPromedio:F2}");
            Console.WriteLine($"  Minimum backtracking: {stats.BacktrackingMin}");
            Console.WriteLine($"  Maximum backtracking: {stats.BacktrackingMax}");
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
            Console.WriteLine("=== BASIC USAGE EXAMPLE ===\n");

            // 1. Generate a traditional Sudoku (without ML)
            Console.WriteLine("1. Generating 3x3 Sudoku without Machine Learning...");
            var sudokuTradicional = new SudokuGenerator(3, 3, usarML: false);
            Console.WriteLine($"   Backtracking: {sudokuTradicional.ConteoErrores}");
            Console.WriteLine($"   Time: {sudokuTradicional.TiempoEjecutado} ms");
            Console.WriteLine($"   Success: {sudokuTradicional.Exito}");

            // 2. Generate a Sudoku with ML (without training)
            Console.WriteLine("\n2. Generating 3x3 Sudoku with Machine Learning (initial model)...");
            var sudokuML = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudokuML.ConteoErrores}");
            Console.WriteLine($"   Time: {sudokuML.TiempoEjecutado} ms");
            Console.WriteLine($"   Success: {sudokuML.Exito}");

            // 3. Train the agent
            Console.WriteLine("\n3. Training the agent with 100 episodes...");
            SudokuGenerator.EntrenarAgente(100, 3, 3);

            // 4. Generate a Sudoku with trained ML
            Console.WriteLine("\n4. Generating 3x3 Sudoku with Machine Learning (trained model)...");
            var sudokuMLEntrenado = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudokuMLEntrenado.ConteoErrores}");
            Console.WriteLine($"   Time: {sudokuMLEntrenado.TiempoEjecutado} ms");
            Console.WriteLine($"   Success: {sudokuMLEntrenado.Exito}");
            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");

            // 5. Generate 4x4 Sudoku (the most problematic)
            Console.WriteLine("\n5. Generating 4x4 Sudoku with Machine Learning...");
            var sudoku4x4 = new SudokuGenerator(4, 4, usarML: true, entrenar: false);
            Console.WriteLine($"   Backtracking: {sudoku4x4.ConteoErrores}");
            Console.WriteLine($"   Time: {sudoku4x4.TiempoEjecutado} ms");
            Console.WriteLine($"   Success: {sudoku4x4.Exito}");
        }
    }
}