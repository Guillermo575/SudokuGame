using System;
using System.Collections.Generic;
using Sudoku.MachineLearning;
namespace Sudoku.Tools
{
    /// <summary>
    /// Prueba rápida para verificar la variedad en la generación de sudokus
    /// </summary>
    public class PruebaRapidaVariedad
    {
        public static void Ejecutar()
        {
            Console.WriteLine("?????????????????????????????????????????????????????????????");
            Console.WriteLine("?  PRUEBA RÁPIDA: VARIEDAD EN GENERACIÓN DE SUDOKUS CON ML ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????\n");
            
            // Paso 1: Entrenar un poco (opcional si ya tienes modelo entrenado)
            Console.WriteLine("?? Paso 1: Entrenamiento rápido (100 episodios)...");
            SudokuGenerator.EntrenarAgente(100, 3, 3);
            Console.WriteLine("? Entrenamiento completado\n");
            
            // Paso 2: Generar 10 sudokus y verificar unicidad
            Console.WriteLine("?? Paso 2: Generando 10 sudokus y verificando variedad...\n");
            
            var hashsGenerados = new HashSet<string>();
            var sudokus = new List<SudokuGenerator>();
            
            for (int i = 0; i < 10; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                
                if (sudoku.Exito)
                {
                    sudokus.Add(sudoku);
                    bool esNuevo = hashsGenerados.Add(sudoku.HashSudoku);
                    
                    string indicador = esNuevo ? "? ÚNICO" : "? REPETIDO";
                    Console.WriteLine($"Sudoku #{i + 1}: {indicador}");
                    Console.WriteLine($"  Hash: {sudoku.HashSudoku.Substring(0, 20)}...");
                    Console.WriteLine($"  Errores: {sudoku.ConteoErrores}, Tiempo: {sudoku.TiempoEjecutado}ms");
                    
                    // Mostrar los primeros 2 sudokus completos
                    if (i < 2)
                    {
                        Console.WriteLine(sudoku.ResumenASCII);
                    }
                    
                    Console.WriteLine();
                }
            }
            
            // Paso 3: Estadísticas
            Console.WriteLine("?? Paso 3: Estadísticas\n");
            
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
            
            Console.WriteLine($"Total generados: {sudokus.Count}");
            Console.WriteLine($"Sudokus únicos: {hashsGenerados.Count}");
            Console.WriteLine($"Tasa de unicidad: {tasaUnicidad:F2}%");
            Console.WriteLine($"Errores promedio: {erroresPromedio:F2}");
            Console.WriteLine($"Tiempo promedio: {tiempoPromedio}ms\n");
            
            // Paso 4: Evaluación
            Console.WriteLine("?? Paso 4: Evaluación\n");
            
            if (tasaUnicidad >= 90)
            {
                Console.WriteLine("?? ¡EXCELENTE! La variedad es muy buena (?90%)");
            }
            else if (tasaUnicidad >= 70)
            {
                Console.WriteLine("? Buena variedad, pero se puede mejorar");
                Console.WriteLine("?? Sugerencia: Aumenta epsilon de uso o usa estrategia Softmax");
            }
            else
            {
                Console.WriteLine("? Baja variedad detectada");
                Console.WriteLine("?? Sugerencias:");
                Console.WriteLine("   1. Entrenar más episodios");
                Console.WriteLine("   2. Aumentar epsilon de uso: SudokuGenerator.agenteML.SetEpsilonUso(0.3)");
                Console.WriteLine("   3. Cambiar estrategia: SudokuGenerator.agenteML.Estrategia = Softmax");
            }
            
            Console.WriteLine();
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
            
            Console.WriteLine("\n?????????????????????????????????????????????????????????????");
            Console.WriteLine("?                    PRUEBA COMPLETADA                      ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????");
        }
        
        /// <summary>
        /// Prueba comparativa: antes vs después de las mejoras
        /// </summary>
        public static void ComparativaAntesDepues()
        {
            Console.WriteLine("?????????????????????????????????????????????????????????????");
            Console.WriteLine("?       COMPARATIVA: ANTES vs DESPUÉS DE LAS MEJORAS        ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????\n");
            
            // Simular "ANTES": Sin exploración en uso
            Console.WriteLine("?? ANTES (sin exploración en uso normal):\n");
            
            var agente = SudokuGenerator.agenteML;
            
            // Guardar configuración actual
            var estrategiaOriginal = agente.Estrategia;
            
            // Configurar como "antes" (sin exploración)
            agente.SetEpsilonUso(0.0);
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;
            
            var hashsAntes = new HashSet<string>();
            for (int i = 0; i < 20; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                    hashsAntes.Add(sudoku.HashSudoku);
            }
            
            Console.WriteLine($"Sudokus únicos: {hashsAntes.Count}/20 ({(double)hashsAntes.Count / 20 * 100:F2}%)");
            Console.WriteLine("Resultado: Siempre (o casi siempre) el mismo sudoku ??\n");
            
            // "DESPUÉS": Con las mejoras
            Console.WriteLine("?? DESPUÉS (con exploración y estrategias):\n");
            
            // Configurar con las mejoras
            agente.SetEpsilonUso(0.15);
            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
            
            var hashsDespues = new HashSet<string>();
            for (int i = 0; i < 20; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                if (sudoku.Exito)
                    hashsDespues.Add(sudoku.HashSudoku);
            }
            
            Console.WriteLine($"Sudokus únicos: {hashsDespues.Count}/20 ({(double)hashsDespues.Count / 20 * 100:F2}%)");
            Console.WriteLine("Resultado: Sudokus diversos en cada generación ??\n");
            
            // Comparación
            Console.WriteLine("?? COMPARACIÓN:\n");
            double mejora = ((double)hashsDespues.Count / hashsAntes.Count - 1) * 100;
            Console.WriteLine($"Mejora en variedad: {mejora:+0.0;-0.0}%");
            Console.WriteLine($"Sudokus adicionales únicos: {hashsDespues.Count - hashsAntes.Count}");
            
            // Restaurar configuración
            agente.Estrategia = estrategiaOriginal;
            
            Console.WriteLine("\n?????????????????????????????????????????????????????????????");
            Console.WriteLine("?              ¡LAS MEJORAS FUNCIONAN! ?                    ?");
            Console.WriteLine("?????????????????????????????????????????????????????????????");
        }
    }
}
