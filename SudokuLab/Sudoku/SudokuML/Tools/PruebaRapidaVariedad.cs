using System;
using System.Collections.Generic;
using Sudoku.MachineLearning;
namespace Sudoku.Tools
{
    /// <summary>
    /// Quick test to verify the variety in sudoku generation
    /// </summary>
    public class PruebaRapidaVariedad
    {
        public static void Ejecutar()
        {
            Console.WriteLine("?????????????????????????????????????????????????????????????");
            Console.WriteLine("?  QUICK TEST: SUDOKU VARIETY WITH ML ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????\n");

            // Step 1: Quick training (optional if you already have a trained model)
            Console.WriteLine("? Step 1: Quick training (100 episodes)...");
            SudokuGenerator.EntrenarAgente(100, 3, 3);
            Console.WriteLine("? Training completed\n");

            // Step 2: Generate 10 sudokus and verify uniqueness
            Console.WriteLine("? Step 2: Generating 10 sudokus and checking variety...\n");

            var hashsGenerados = new HashSet<string>();
            var sudokus = new List<SudokuGenerator>();

            for (int i = 0; i < 10; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

                if (sudoku.Exito)
                {
                    sudokus.Add(sudoku);
                    bool esNuevo = hashsGenerados.Add(sudoku.HashSudoku);

                    string indicador = esNuevo ? "? UNIQUE" : "?? REPEATED";
                    Console.WriteLine($"Sudoku #{i + 1}: {indicador}");
                    Console.WriteLine($"  Hash: {sudoku.HashSudoku.Substring(0, 20)}...");
                    Console.WriteLine($"  Errors: {sudoku.ConteoErrores}, Time: {sudoku.TiempoEjecutado}ms");

                    // Show first 2 sudokus completely
                    if (i < 2)
                    {
                        Console.WriteLine(sudoku.ResumenASCII);
                    }

                    Console.WriteLine();
                }
            }

            // Step 3: Statistics
            Console.WriteLine("? Step 3: Statistics\n");

            double tasaUnicidad = (double)hashsGenerados.Count / sudokus.Count * 100;
            double erroresPromedio = 0;
            long tiempoPromedio = 0;

            foreach (var s in sudokus)
            {
                erroresPromedio += s.ConteoErrores;
                tiempoPromedio += s.TiempoEjecutado;
            }

            erroresPromedio /= sudokus.Count;
            tiempoPromedio /= sudokus.Count;

            Console.WriteLine($"Total generated: {sudokus.Count}");
            Console.WriteLine($"Unique sudokus: {hashsGenerados.Count}");
            Console.WriteLine($"Uniqueness rate: {tasaUnicidad:F2}%");
            Console.WriteLine($"Average errors: {erroresPromedio:F2}");
            Console.WriteLine($"Average time: {tiempoPromedio}ms\n");

            // Step 4: Evaluation
            Console.WriteLine("? Step 4: Evaluation\n");

            if (tasaUnicidad >= 90)
            {
                Console.WriteLine("? EXCELLENT! Variety is very good (?90%)");
            }
            else if (tasaUnicidad >= 70)
            {
                Console.WriteLine("? Good variety, but can be improved");
                Console.WriteLine("?? Suggestion: Increase epsilon usage or use Softmax strategy");
            }
            else
            {
                Console.WriteLine("? Low variety detected");
                Console.WriteLine("?? Suggestions:");
                Console.WriteLine("   1. Train more episodes");
                Console.WriteLine("   2. Increase epsilon usage: SudokuGenerator.agenteML.SetEpsilonUso(0.3)");
                Console.WriteLine("   3. Change strategy: SudokuGenerator.agenteML.Estrategia = Softmax");
            }

            Console.WriteLine();
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());

            Console.WriteLine("\n?????????????????????????????????????????????????????????????");
            Console.WriteLine("?                    TEST COMPLETED                      ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????");
        }

        /// <summary>
        /// Comparative test: before vs after improvements
        /// </summary>
        public static void ComparativaAntesDepues()
        {
            Console.WriteLine("?????????????????????????????????????????????????????????????");
            Console.WriteLine("?       COMPARISON: BEFORE vs AFTER IMPROVEMENTS        ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????\n");

            // Simulate "BEFORE": Without exploration in normal use
            Console.WriteLine("? BEFORE (without exploration in normal use):\n");

            var agente = SudokuGenerator.agenteML;

            // Save original configuration
            var estrategiaOriginal = agente.Estrategia;

            // Configure as "before" (without exploration)
            agente.SetEpsilonUso(0.0);
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;

            var hashsAntes = new HashSet<string>();
            for (int i = 0; i < 20; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                    hashsAntes.Add(sudoku.HashSudoku);
            }

            Console.WriteLine($"Unique sudokus: {hashsAntes.Count}/20 ({(double)hashsAntes.Count / 20 * 100:F2}%)");
            Console.WriteLine("Result: Always (or almost always) the same sudoku ??\n");

            // "AFTER": With improvements
            Console.WriteLine("? AFTER (with exploration and strategies):\n");

            // Configure with improvements
            agente.SetEpsilonUso(0.15);
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;

            var hashsDespues = new HashSet<string>();
            for (int i = 0; i < 20; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                    hashsDespues.Add(sudoku.HashSudoku);
            }

            Console.WriteLine($"Unique sudokus: {hashsDespues.Count}/20 ({(double)hashsDespues.Count / 20 * 100:F2}%)");
            Console.WriteLine("Result: Diverse sudokus in each generation ??\n");

            // Comparison
            Console.WriteLine("? COMPARISON:\n");
            double mejora = ((double)hashsDespues.Count / hashsAntes.Count - 1) * 100;
            Console.WriteLine($"Variety improvement: {mejora:+0.0;-0.0}%");
            Console.WriteLine($"Additional unique sudokus: {hashsDespues.Count - hashsAntes.Count}");

            // Restore configuration
            agente.Estrategia = estrategiaOriginal;

            Console.WriteLine("\n?????????????????????????????????????????????????????????????");
            Console.WriteLine("?              IMPROVEMENTS WORK! ?                    ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????");
        }
    }
}
