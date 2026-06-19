using System;
using Sudoku.MachineLearning;
namespace Sudoku.Tools
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
            Console.WriteLine("=== ENTRENAMIENTO DEL AGENTE ===\n");
            
            // Entrenar el agente con 500 episodios
            SudokuGenerator.EntrenarAgente(episodios: 500, columnasX: 3, columnasY: 3);
            
            // Ver estadísticas
            Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
            
            Console.WriteLine("\n=== GENERANDO SUDOKUS DIVERSOS ===\n");
            
            // Generar 5 sudokus diferentes
            var hashsGenerados = new System.Collections.Generic.HashSet<string>();
            
            for (int i = 0; i < 5; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                
                if (sudoku.Exito)
                {
                    hashsGenerados.Add(sudoku.HashSudoku);
                    Console.WriteLine($"Sudoku #{i + 1}");
                    Console.WriteLine($"Hash: {sudoku.HashSudoku.Substring(0, 20)}...");
                    Console.WriteLine($"Errores: {sudoku.ConteoErrores}");
                    Console.WriteLine(sudoku.ResumenASCII);
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine($"Sudokus únicos generados: {hashsGenerados.Count} de 5");
        }
        
        /// <summary>
        /// Ejemplo 2: Comparar estrategias de exploración
        /// </summary>
        public static void EjemploCompararEstrategias()
        {
            Console.WriteLine("=== COMPARACIÓN DE ESTRATEGIAS ===\n");
            
            var estrategias = new[]
            {
                SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy,
                SudokuRLAgent.EstrategiaExploracion.Softmax,
                SudokuRLAgent.EstrategiaExploracion.Hibrida
            };
            
            foreach (var estrategia in estrategias)
            {
                Console.WriteLine($"--- Estrategia: {estrategia} ---");
                
                // Crear un nuevo agente para cada estrategia
                var agente = new SudokuRLAgent();
                agente.Estrategia = estrategia;
                agente.SetEpsilonUso(0.2);
                
                // Generar 10 sudokus y medir diversidad
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
                
                Console.WriteLine($"Sudokus únicos: {hashs.Count}/10");
                Console.WriteLine($"Errores promedio: {totalErrores / 10.0:F2}");
                Console.WriteLine();
            }
        }
        
        /// <summary>
        /// Ejemplo 3: Ajustar parámetros para máxima variedad
        /// </summary>
        public static void EjemploMaximaVariedad()
        {
            Console.WriteLine("=== CONFIGURACIÓN PARA MÁXIMA VARIEDAD ===\n");
            
            // Configurar agente para máxima variedad
            var agente = new SudokuRLAgent();
            agente.SetEpsilonUso(0.3);        // 30% exploración
            agente.SetTemperature(2.0);        // Alta temperatura
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;
            
            Console.WriteLine("Configuración:");
            Console.WriteLine("- Epsilon uso: 0.3 (30% exploración)");
            Console.WriteLine("- Temperatura: 2.0");
            Console.WriteLine("- Estrategia: Softmax\n");
            
            // Generar 20 sudokus
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
            
            Console.WriteLine("Resultados:");
            Console.WriteLine($"- Éxitos: {exitos}/20");
            Console.WriteLine($"- Sudokus únicos: {hashs.Count}/{exitos}");
            Console.WriteLine($"- Tasa de unicidad: {(double)hashs.Count / exitos * 100:F2}%");
            Console.WriteLine($"- Errores promedio: {(double)totalErrores / exitos:F2}");
        }
        
        /// <summary>
        /// Ejemplo 4: Entrenar y monitorear diversidad en tiempo real
        /// </summary>
        public static void EjemploMonitoreoDiversidad()
        {
            Console.WriteLine("=== MONITOREO DE DIVERSIDAD DURANTE ENTRENAMIENTO ===\n");
            
            var agente = new SudokuRLAgent();
            agente.ResetearModelo(); // Empezar desde cero
            
            int episodios = 200;
            
            for (int i = 0; i < episodios; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: true);
                double recompensa = sudoku.Exito ? 100 : -50;
                agente.RegistrarEpisodio(recompensa, sudoku.HashSudoku);
                
                // Reportar cada 50 episodios
                if ((i + 1) % 50 == 0)
                {
                    double tasaUnicidad = (double)agente.SudokusUnicos / agente.EpisodiosEntrenados * 100;
                    Console.WriteLine($"Episodio {i + 1}:");
                    Console.WriteLine($"  - Sudokus únicos: {agente.SudokusUnicos}");
                    Console.WriteLine($"  - Tasa de unicidad: {tasaUnicidad:F2}%");
                    Console.WriteLine($"  - Recompensa promedio: {agente.RecompensaPromedio:F2}");
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine("Entrenamiento completado.");
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
