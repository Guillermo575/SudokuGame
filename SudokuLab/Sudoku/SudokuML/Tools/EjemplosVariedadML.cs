using System;
using SudokuML.MachineLearning;
namespace SudokuML.Tools
{
    /// <summary>
    /// Ejemplos de uso del generador de Sudoku con ML y variedad
    /// </summary>
    public class EjemplosVariedadML
    {
        /// <summary>
        /// Ejemplo 1: Entrenar el agente y generar sudokus diversos
        /// </summary>
        public static void EjemploEntrenamientoYGeneracion()
        {
            Console.WriteLine("=== AGENT TRAINING ===\n");

            // Train the agent with 500 episodes
            SudokuGenerator.EntrenarAgente(episodios: 500, columnasX: 3, columnasY: 3);

            // View statistics
            Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());

            Console.WriteLine("\n=== GENERATING DIVERSE SUDOKUS ===\n");

            // Generate 5 different sudokus
            var hashsGenerados = new System.Collections.Generic.HashSet<string>();

            for (int i = 0; i < 5; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

                if (sudoku.Exito)
                {
                    hashsGenerados.Add(sudoku.HashSudoku);
                    Console.WriteLine($"Sudoku #{i + 1}");
                    Console.WriteLine($"Hash: {sudoku.HashSudoku.Substring(0, 20)}...");
                    Console.WriteLine($"Errors: {sudoku.ConteoErrores}");
                    Console.WriteLine(sudoku.ResumenASCII);
                    Console.WriteLine();
                }
            }

            Console.WriteLine($"Unique sudokus generated: {hashsGenerados.Count} of 5");
        }
        
        /// <summary>
        /// Ejemplo 2: Comparar estrategias de exploración
        /// </summary>
        public static void EjemploCompararEstrategias()
        {
            Console.WriteLine("=== STRATEGY COMPARISON ===\n");

            var estrategias = new[]
            {
                SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy,
                SudokuRLAgent.EstrategiaExploracion.Softmax,
                SudokuRLAgent.EstrategiaExploracion.Hibrida
            };

            foreach (var estrategia in estrategias)
            {
                Console.WriteLine($"--- Strategy: {estrategia} ---");

                // Create a new agent for each strategy
                var agente = new SudokuRLAgent();
                agente.Estrategia = estrategia;
                agente.SetEpsilonUso(0.2);

                // Generate 10 sudokus and measure diversity
                var hashs = new System.Collections.Generic.HashSet<string>();
                int totalErrores = 0;

                for (int i = 0; i < 10; i++)
                {
                    var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                    if (sudoku.Exito)
                    {
                        hashs.Add(sudoku.HashSudoku);
                        totalErrores += sudoku.ConteoErrores;
                    }
                }

                Console.WriteLine($"Unique sudokus: {hashs.Count}/10");
                Console.WriteLine($"Average errors: {totalErrores / 10.0:F2}");
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// Ejemplo 3: Ajustar parámetros para máxima variedad
        /// </summary>
        public static void EjemploMaximaVariedad()
        {
            Console.WriteLine("=== CONFIGURATION FOR MAXIMUM VARIETY ===\n");

            // Configure agent for maximum variety
            var agente = new SudokuRLAgent();
            agente.SetEpsilonUso(0.3);        // 30% exploration
            agente.SetTemperature(2.0);        // High temperature
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;

            Console.WriteLine("Configuration:");
            Console.WriteLine("- Epsilon usage: 0.3 (30% exploration)");
            Console.WriteLine("- Temperature: 2.0");
            Console.WriteLine("- Strategy: Softmax\n");

            // Generate 20 sudokus
            var hashs = new System.Collections.Generic.HashSet<string>();
            int totalErrores = 0;
            int exitos = 0;

            for (int i = 0; i < 20; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                {
                    exitos++;
                    hashs.Add(sudoku.HashSudoku);
                    totalErrores += sudoku.ConteoErrores;
                }
            }

            Console.WriteLine("Results:");
            Console.WriteLine($"- Successful: {exitos}/20");
            Console.WriteLine($"- Unique sudokus: {hashs.Count}/{exitos}");
            Console.WriteLine($"- Uniqueness rate: {(double)hashs.Count / exitos * 100:F2}%");
            Console.WriteLine($"- Average errors: {(double)totalErrores / exitos:F2}");
        }
        
        /// <summary>
        /// Ejemplo 4: Entrenar y monitorear diversidad en tiempo real
        /// </summary>
        public static void EjemploMonitoreoDiversidad()
        {
            Console.WriteLine("=== DIVERSITY MONITORING DURING TRAINING ===\n");

            var agente = new SudokuRLAgent();
            agente.ResetearModelo(); // Start from scratch

            int episodios = 200;

            for (int i = 0; i < episodios; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: true);
                double recompensa = sudoku.Exito ? 100 : -50;
                agente.RegistrarEpisodio(recompensa, sudoku.HashSudoku);

                // Report every 50 episodes
                if ((i + 1) % 50 == 0)
                {
                    double tasaUnicidad = (double)agente.SudokusUnicos / agente.EpisodiosEntrenados * 100;
                    Console.WriteLine($"Episode {i + 1}:");
                    Console.WriteLine($"  - Unique sudokus: {agente.SudokusUnicos}");
                    Console.WriteLine($"  - Uniqueness rate: {tasaUnicidad:F2}%");
                    Console.WriteLine($"  - Average reward: {agente.RecompensaPromedio:F2}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("Training completed.");
        }
        
        /// <summary>
        /// Ejemplo 5: Comparar ML vs tradicional
        /// </summary>
        public static void EjemploComparacionMLvsTradicional()
        {
            Console.WriteLine("=== COMPARACIÓN: ML vs TRADICIONAL ===\n");
            
            int iteraciones = 10;
            
            // Con ML
            Console.WriteLine("--- CON MACHINE LEARNING ---");
            var hashsML = new System.Collections.Generic.HashSet<string>();
            long tiempoML = 0;
            int erroresML = 0;
            
            for (int i = 0; i < iteraciones; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                {
                    hashsML.Add(sudoku.HashSudoku);
                    tiempoML += sudoku.TiempoEjecutado;
                    erroresML += sudoku.ConteoErrores;
                }
            }
            
            Console.WriteLine($"Sudokus únicos: {hashsML.Count}/{iteraciones}");
            Console.WriteLine($"Tiempo promedio: {tiempoML / iteraciones}ms");
            Console.WriteLine($"Errores promedio: {erroresML / (double)iteraciones:F2}\n");
            
            // Sin ML (tradicional)
            Console.WriteLine("--- SIN MACHINE LEARNING (Tradicional) ---");
            var hashsTradicional = new System.Collections.Generic.HashSet<string>();
            long tiempoTradicional = 0;
            int erroresTradicional = 0;
            
            for (int i = 0; i < iteraciones; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: false, entrenar: false);
                if (sudoku.Exito)
                {
                    hashsTradicional.Add(sudoku.HashSudoku);
                    tiempoTradicional += sudoku.TiempoEjecutado;
                    erroresTradicional += sudoku.ConteoErrores;
                }
            }
            
            Console.WriteLine($"Sudokus únicos: {hashsTradicional.Count}/{iteraciones}");
            Console.WriteLine($"Tiempo promedio: {tiempoTradicional / iteraciones}ms");
            Console.WriteLine($"Errores promedio: {erroresTradicional / (double)iteraciones:F2}\n");
            
            // Comparación
            Console.WriteLine("--- ANÁLISIS ---");
            Console.WriteLine($"ML tiene {((double)hashsML.Count / hashsTradicional.Count - 1) * 100:+0.0;-0.0}% de variedad");
            Console.WriteLine($"ML es {((double)erroresTradicional / erroresML - 1) * 100:+0.0;-0.0}% más eficiente (menos errores)");
        }
        
        /// <summary>
        /// Ejecutar todos los ejemplos
        /// </summary>
        public static void EjecutarTodosLosEjemplos()
        {
            try
            {
                EjemploEntrenamientoYGeneracion();
                Console.WriteLine("\n" + new string('=', 60) + "\n");
                
                EjemploCompararEstrategias();
                Console.WriteLine("\n" + new string('=', 60) + "\n");
                
                EjemploMaximaVariedad();
                Console.WriteLine("\n" + new string('=', 60) + "\n");
                
                EjemploMonitoreoDiversidad();
                Console.WriteLine("\n" + new string('=', 60) + "\n");
                
                EjemploComparacionMLvsTradicional();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
