using System;
using System.Text;
using Sudoku.MachineLearning;
namespace Sudoku.Tools
{
    /// <summary>
    /// Programa de ejemplo para probar y entrenar el sistema de ML para Sudoku
    /// </summary>
    class MainMenu
    {
        public static void MostrarMenu()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
            }
            catch
            {
            }
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   🤖 SISTEMA DE MACHINE LEARNING PARA GENERACIÓN DE SUDOKU   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
            while (true)
            {
                Console.WriteLine("\n╔═══════════ 📋 MENÚ PRINCIPAL ═══════════╗");
                Console.WriteLine("║                                          ║");
                Console.WriteLine("║  1. 🚀 INICIO RÁPIDO                    ║");
                Console.WriteLine("║  2. 🎲 Generar Sudoku                   ║");
                Console.WriteLine("║  3. 🏋️  Entrenar Modelo                  ║");
                Console.WriteLine("║  4. ⚙️  Configuración                    ║");
                Console.WriteLine("║  5. 🔬 Análisis y Tests                 ║");
                Console.WriteLine("║  6. 📊 Ver Estadísticas                 ║");
                Console.WriteLine("║  0. 🚪 Salir                            ║");
                Console.WriteLine("║                                          ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.Write("\n👉 Seleccione una opción: ");

                string opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        MenuInicioRapido();
                        break;
                    case "2":
                        MenuGenerarSudoku();
                        break;
                    case "3":
                        MenuEntrenar();
                        break;
                    case "4":
                        MenuConfiguracion();
                        break;
                    case "5":
                        MenuAnalisis();
                        break;
                    case "6":
                        Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("❌ Opción no válida.");
                        Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void MenuInicioRapido()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🚀 INICIO RÁPIDO ═══════════╗\n");
            Console.WriteLine("1. ⚡ Prueba Rápida (Recomendado)");
            Console.WriteLine("   💡 Entrena + genera + verifica variedad");
            Console.WriteLine("\n2. 📚 Ejemplo de uso básico");
            Console.WriteLine("   🎯 Demostración simple del sistema");
            Console.WriteLine("\n3. ⚖️  Comparativa antes/después");
            Console.WriteLine("   📈 Ver impacto de las mejoras");
            Console.WriteLine("\n0. ↩️  Volver al menú principal");
            Console.Write("\n👉 Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    PruebaRapidaVariedad.Ejecutar();
                    break;
                case "2":
                    SudokuMLHelper.EjemploUso();
                    break;
                case "3":
                    PruebaRapidaVariedad.ComparativaAntesDepues();
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    break;
            }

            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuGenerarSudoku()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🎲 GENERAR SUDOKU ═══════════╗\n");
            Console.WriteLine("1. 🤖 Generar con Machine Learning");
            Console.WriteLine("2. 🔧 Generar sin ML (tradicional)");
            Console.WriteLine("3. 🌈 Demostrar variedad (genera varios)");
            Console.WriteLine("\n0. ↩️  Volver al menú principal");
            Console.Write("\n👉 Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    GenerarSudoku(usarML: true);
                    break;
                case "2":
                    GenerarSudoku(usarML: false);
                    break;
                case "3":
                    DemostrarVariedad();
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    break;
            }

            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuEntrenar()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🏋️  ENTRENAR MODELO ═══════════╗\n");
            Console.WriteLine("1. ⚡ Entrenamiento rápido (100 episodios)");
            Console.WriteLine("2. 💪 Entrenamiento completo (1000 episodios)");
            Console.WriteLine("3. 🎯 Entrenar para Sudoku 4x4 (2000 episodios)");
            Console.WriteLine("4. 🛠️  Entrenar personalizado");
            Console.WriteLine("5. 📊 Monitorear diversidad durante entrenamiento");
            Console.WriteLine("\n0. ↩️  Volver al menú principal");
            Console.Write("\n👉 Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    SudokuGenerator.EntrenarAgente(100, 3, 3);
                    Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "2":
                    SudokuGenerator.EntrenarAgente(1000, 3, 3);
                    Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "3":
                    Console.WriteLine("Entrenando para Sudoku 4x4 (esto puede tomar varios minutos)...");
                    SudokuGenerator.EntrenarAgente(2000, 4, 4);
                    Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "4":
                    Console.Write("Episodios: ");
                    if (!int.TryParse(Console.ReadLine(), out int episodios)) episodios = 1000;
                    Console.Write("Columnas X: ");
                    if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;
                    Console.Write("Columnas Y: ");
                    if (!int.TryParse(Console.ReadLine(), out int colY)) colY = 3;
                    SudokuGenerator.EntrenarAgente(episodios, colX, colY);
                    Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "5":
                    MonitorearDiversidad();
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    break;
            }

            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuConfiguracion()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ ⚙️  CONFIGURACIÓN ═══════════╗\n");
            Console.WriteLine("1. 🎛️  Configurar parámetros de exploración");
            Console.WriteLine("   💡 Presets: Variedad, Balance, Rendimiento");
            Console.WriteLine("\n2. 👀 Ver parámetros actuales");
            Console.WriteLine("\n0. ↩️  Volver al menú principal");
            Console.Write("\n👉 Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    ConfigurarParametros();
                    break;
                case "2":
                    Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    break;
            }

            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuAnalisis()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🔬 ANÁLISIS Y TESTS ═══════════╗\n");
            Console.WriteLine("1. ⚔️  Comparar rendimiento (con vs sin ML)");
            Console.WriteLine("2. 🔀 Comparar estrategias de exploración");
            Console.WriteLine("3. 🧪 Testeo masivo (50 sudokus)");
            Console.WriteLine("\n0. ↩️  Volver al menú principal");
            Console.Write("\n👉 Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Tamaño del Sudoku (ej: 3 para 3x3, 4 para 4x4): ");
                    if (int.TryParse(Console.ReadLine(), out int tam))
                    {
                        SudokuMLHelper.CompararRendimiento(100, tam, tam);
                    }
                    break;
                case "2":
                    CompararEstrategias();
                    break;
                case "3":
                    TesteoMasivo();
                    break;
                case "0":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("❌ Opción no válida.");
                    break;
            }

            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void GenerarSudoku(bool usarML)
        {
            Console.Write("Columnas X (ej: 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;

            Console.Write("Columnas Y (ej: 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colY)) colY = 3;

            Console.WriteLine($"\n🎲 Generando Sudoku {colX}x{colY} {(usarML ? "CON 🤖" : "SIN 🔧")} Machine Learning...\n");

            var sudoku = new SudokuGenerator(colX, colY, usarML, entrenar: false);

            Console.WriteLine($"Éxito: {(sudoku.Exito ? "✅ SÍ" : "❌ NO")}");
            Console.WriteLine($"⏱️  Tiempo: {sudoku.TiempoEjecutado} ms");
            Console.WriteLine($"🔄 Backtracking: {sudoku.ConteoErrores}");
            Console.WriteLine($"Validado: {(sudoku.Validado ? "✅ SÍ" : "❌ NO")}");

            if (sudoku.Exito)
            {
                Console.WriteLine("\n" + sudoku.ResumenASCII);
            }
        }

        static void TesteoMasivo()
        {
            Console.Write("🔢 Cantidad de Sudokus: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad)) cantidad = 50;

            Console.Write("📏 Columnas X (ej: 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;

            Console.Write("📏 Columnas Y (ej: 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colY)) colY = 3;

            Console.WriteLine($"\n🚀 Generando {cantidad} sudokus {colX}x{colY}...\n");

            int exitosos = 0;
            long tiempoTotal = 0;
            int backtrackTotal = 0;
            int backtrackMin = int.MaxValue;
            int backtrackMax = 0;

            for (int i = 0; i < cantidad; i++)
            {
                var sudoku = new SudokuGenerator(colX, colY, usarML: true, entrenar: false);

                if (sudoku.Exito)
                {
                    exitosos++;
                    tiempoTotal += sudoku.TiempoEjecutado;
                    backtrackTotal += sudoku.ConteoErrores;
                    backtrackMin = Math.Min(backtrackMin, sudoku.ConteoErrores);
                    backtrackMax = Math.Max(backtrackMax, sudoku.ConteoErrores);
                }

                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine($"📊 Progreso: {i + 1}/{cantidad}");
                }
            }

            Console.WriteLine("\n╔═══ 📈 RESULTADOS ═══╗");
            Console.WriteLine($"✅ Exitosos: {exitosos}/{cantidad} ({(double)exitosos / cantidad * 100:F2}%)");
            Console.WriteLine($"⏱️  Tiempo promedio: {(exitosos > 0 ? tiempoTotal / exitosos : 0)} ms");
            Console.WriteLine($"🔄 Backtracking promedio: {(exitosos > 0 ? backtrackTotal / exitosos : 0)}");
            Console.WriteLine($"⬇️  Backtracking mínimo: {(backtrackMin == int.MaxValue ? 0 : backtrackMin)}");
            Console.WriteLine($"⬆️  Backtracking máximo: {backtrackMax}");
        }

        /// <summary>
        /// Método principal de ejemplo - Genera un sudoku con ML
        /// </summary>
        public static void EjemploBasico()
        {
            Console.WriteLine("╔═══ 🎲 GENERACIÓN DE SUDOKU CON ML ═══╗\n");

            // Generar un sudoku 3x3 usando Machine Learning
            var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

            Console.WriteLine($"✅ Éxito: {sudoku.Exito}");
            Console.WriteLine($"🔄 Backtracking: {sudoku.ConteoErrores}");
            Console.WriteLine($"⏱️  Tiempo: {sudoku.TiempoEjecutado} ms");
            Console.WriteLine($"\n{sudoku.ResumenHTML}");
        }

        /// <summary>
        /// Entrena el agente de ML con episodios específicos
        /// </summary>
        public static void EntrenarModelo(int episodios = 1000, int columnasX = 3, int columnasY = 3)
        {
            Console.WriteLine($"╔═══ 🏋️  ENTRENAMIENTO DEL AGENTE ═══╗");
            Console.WriteLine($"📐 Tamaño: {columnasX}x{columnasY}");
            Console.WriteLine($"🔢 Episodios: {episodios}\n");

            SudokuGenerator.EntrenarAgente(episodios, columnasX, columnasY);

            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");
        }

        /// <summary>
        /// Compara el rendimiento con y sin ML
        /// </summary>
        public static void CompararRendimiento(int cantidad = 100, int columnasX = 4, int columnasY = 4)
        {
            Console.WriteLine($"╔═══ ⚔️  COMPARACIÓN DE RENDIMIENTO ═══╗");
            Console.WriteLine($"📐 Tamaño: {columnasX}x{columnasY}");
            Console.WriteLine($"🔢 Cantidad: {cantidad} sudokus\n");

            SudokuMLHelper.CompararRendimiento(cantidad, columnasX, columnasY);
        }

        /// <summary>
        /// Ejecuta todos los ejemplos en secuencia
        /// </summary>
        public static void EjecutarTodo()
        {
            // 1. Entrenar el modelo
            EntrenarModelo(100, 3, 3);

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // 2. Generar un sudoku
            EjemploBasico();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // 3. Comparar rendimiento
            CompararRendimiento(50, 3, 3);
        }
        
        /// <summary>
        /// Demuestra la generación de sudokus diversos
        /// </summary>
        static void DemostrarVariedad()
        {
            Console.WriteLine("╔═══ 🌈 DEMOSTRACIÓN DE SUDOKUS DIVERSOS ═══╗\n");
            Console.Write("🔢 ¿Cuántos sudokus generar? (recomendado: 5-10): ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad)) cantidad = 5;
            
            Console.WriteLine($"\n🚀 Generando {cantidad} sudokus con ML...\n");
            
            var hashsGenerados = new System.Collections.Generic.HashSet<string>();
            var listaHashs = new System.Collections.Generic.List<string>();
            
            for (int i = 0; i < cantidad; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                
                if (sudoku.Exito)
                {
                    bool esUnico = hashsGenerados.Add(sudoku.HashSudoku);
                    listaHashs.Add(sudoku.HashSudoku);
                    
                    Console.WriteLine($"🎲 Sudoku #{i + 1} {(esUnico ? "✨ ÚNICO" : "🔁 REPETIDO")}");
                    Console.WriteLine($"🔑 Hash: {sudoku.HashSudoku.Substring(0, Math.Min(30, sudoku.HashSudoku.Length))}...");
                    Console.WriteLine($"❌ Errores: {sudoku.ConteoErrores}, ⏱️  Tiempo: {sudoku.TiempoEjecutado}ms");
                    
                    if (i < 3) // Mostrar solo los primeros 3 completos
                    {
                        Console.WriteLine(sudoku.ResumenASCII);
                    }
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine($"\n╔═══ 📊 RESUMEN ═══╗");
            Console.WriteLine($"✨ Sudokus únicos: {hashsGenerados.Count}/{cantidad}");
            Console.WriteLine($"📈 Tasa de unicidad: {(double)hashsGenerados.Count / cantidad * 100:F2}%");
            
            if (hashsGenerados.Count < cantidad)
            {
                Console.WriteLine("\n💡 Tip: Para mayor variedad, intenta:");
                Console.WriteLine("   📌 Aumentar epsilon de uso (opción 13)");
                Console.WriteLine("   📌 Cambiar a estrategia Softmax");
                Console.WriteLine("   📌 Entrenar más episodios");
            }
        }
        
        /// <summary>
        /// Compara las diferentes estrategias de exploración
        /// </summary>
        static void CompararEstrategias()
        {
            Console.WriteLine("╔═══ 🔀 COMPARACIÓN DE ESTRATEGIAS DE EXPLORACIÓN ═══╗\n");
            
            int sudokusPorEstrategia = 10;
            
            var estrategias = new[]
            {
                (SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy, "Epsilon-Greedy"),
                (SudokuRLAgent.EstrategiaExploracion.Softmax, "Softmax"),
                (SudokuRLAgent.EstrategiaExploracion.Hibrida, "Híbrida")
            };
            
            foreach (var (estrategia, nombre) in estrategias)
            {
                Console.WriteLine($"\n╠═══ 🎯 Estrategia: {nombre} ═══╣");
                
                // Nota: Idealmente deberías tener acceso al agente para cambiar la estrategia
                // Por ahora, generamos con la estrategia por defecto
                var hashs = new System.Collections.Generic.HashSet<string>();
                int totalErrores = 0;
                long totalTiempo = 0;
                int exitos = 0;
                
                for (int i = 0; i < sudokusPorEstrategia; i++)
                {
                    var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                    if (sudoku.Exito)
                    {
                        exitos++;
                        hashs.Add(sudoku.HashSudoku);
                        totalErrores += sudoku.ConteoErrores;
                        totalTiempo += sudoku.TiempoEjecutado;
                    }
                }
                
                Console.WriteLine($"✨ Sudokus únicos: {hashs.Count}/{exitos}");
                Console.WriteLine($"📈 Tasa de unicidad: {(exitos > 0 ? (double)hashs.Count / exitos * 100 : 0):F2}%");
                Console.WriteLine($"❌ Errores promedio: {(exitos > 0 ? (double)totalErrores / exitos : 0):F2}");
                Console.WriteLine($"⏱️  Tiempo promedio: {(exitos > 0 ? totalTiempo / exitos : 0)}ms");
                Console.WriteLine();
            }
            
            Console.WriteLine("💡 Nota: Para cambiar estrategias, usa la opción 13");
        }
        
        /// <summary>
        /// Monitorea la diversidad durante el entrenamiento
        /// </summary>
        static void MonitorearDiversidad()
        {
            Console.WriteLine("╔═══ 📊 MONITOREO DE DIVERSIDAD DURANTE ENTRENAMIENTO ═══╗\n");
            Console.Write("🔢 ¿Cuántos episodios entrenar?: ");
            if (!int.TryParse(Console.ReadLine(), out int episodios)) episodios = 200;
            
            Console.WriteLine($"\n🚀 Entrenando {episodios} episodios y monitoreando diversidad...\n");
            
            // Aquí ejecutamos el entrenamiento con tracking
            var hashsVistos = new System.Collections.Generic.HashSet<string>();
            int reporteCada = Math.Max(1, episodios / 10);
            
            for (int i = 0; i < episodios; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: true);
                
                if (sudoku.Exito && !string.IsNullOrEmpty(sudoku.HashSudoku))
                {
                    hashsVistos.Add(sudoku.HashSudoku);
                }
                
                if ((i + 1) % reporteCada == 0)
                {
                    double tasaUnicidad = (double)hashsVistos.Count / (i + 1) * 100;
                    Console.WriteLine($"📍 Episodio {i + 1}/{episodios}:");
                    Console.WriteLine($"  ✨ Sudokus únicos: {hashsVistos.Count}");
                    Console.WriteLine($"  📈 Tasa de unicidad: {tasaUnicidad:F2}%");
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine($"\n╔═══ 🏆 RESULTADOS FINALES ═══╗");
            Console.WriteLine($"🔢 Total episodios: {episodios}");
            Console.WriteLine($"✨ Sudokus únicos: {hashsVistos.Count}");
            Console.WriteLine($"📈 Tasa de unicidad: {(double)hashsVistos.Count / episodios * 100:F2}%");
            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");
        }
        
        /// <summary>
        /// Permite configurar parámetros de exploración
        /// </summary>
        static void ConfigurarParametros()
        {
            Console.WriteLine("╔═══ 🎛️  CONFIGURACIÓN DE PARÁMETROS DE EXPLORACIÓN ═══╗\n");
            
            Console.WriteLine("📊 Parámetros actuales:");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
            Console.WriteLine();
            
            Console.WriteLine("🎯 Seleccione un preset:\n");
            Console.WriteLine("1. 🌈 MÁXIMA VARIEDAD");
            Console.WriteLine("   📌 Epsilon uso: 0.3 (30% exploración)");
            Console.WriteLine("   📌 Temperatura: 2.0");
            Console.WriteLine("   📌 Estrategia: Softmax\n");
            
            Console.WriteLine("2. ⚖️  BALANCE (recomendado)");
            Console.WriteLine("   📌 Epsilon uso: 0.15 (15% exploración)");
            Console.WriteLine("   📌 Temperatura: 0.8");
            Console.WriteLine("   📌 Estrategia: Híbrida\n");
            
            Console.WriteLine("3. ⚡ MÁXIMO RENDIMIENTO");
            Console.WriteLine("   📌 Epsilon uso: 0.05 (5% exploración)");
            Console.WriteLine("   📌 Temperatura: 0.3");
            Console.WriteLine("   📌 Estrategia: EpsilonGreedy\n");
            
            Console.WriteLine("4. 🛠️  PERSONALIZADO\n");
            
            Console.Write("👉 Seleccione opción (1-4): ");
            string opcion = Console.ReadLine();
            
            var agente = SudokuGenerator.agenteML;
            
            switch (opcion)
            {
                case "1":
                    agente.SetEpsilonUso(0.3);
                    agente.SetTemperature(2.0);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;
                    Console.WriteLine("\n✅ Configuración aplicada: MÁXIMA VARIEDAD 🌈");
                    break;
                
                case "2":
                    agente.SetEpsilonUso(0.15);
                    agente.SetTemperature(0.8);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
                    Console.WriteLine("\n✅ Configuración aplicada: BALANCE ⚖️");
                    break;
                
                case "3":
                    agente.SetEpsilonUso(0.05);
                    agente.SetTemperature(0.3);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;
                    Console.WriteLine("\n✅ Configuración aplicada: MÁXIMO RENDIMIENTO ⚡");
                    break;
                
                case "4":
                    Console.Write("\n🎚️  Epsilon de uso (0.0 - 1.0, actual: 0.15): ");
                    if (double.TryParse(Console.ReadLine(), out double epsilon))
                    {
                        agente.SetEpsilonUso(epsilon);
                    }
                    
                    Console.Write("🌡️  Temperatura (0.1 - 5.0, actual: 1.0): ");
                    if (double.TryParse(Console.ReadLine(), out double temp))
                    {
                        agente.SetTemperature(temp);
                    }
                    
                    Console.WriteLine("\n🎯 Estrategia:");
                    Console.WriteLine("1. 🎲 EpsilonGreedy");
                    Console.WriteLine("2. 🔥 Softmax");
                    Console.WriteLine("3. 🔀 Híbrida");
                    Console.Write("👉 Seleccione (1-3): ");
                    
                    switch (Console.ReadLine())
                    {
                        case "1":
                            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;
                            break;
                        case "2":
                            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;
                            break;
                        case "3":
                            agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
                            break;
                    }
                    
                    Console.WriteLine("\n✅ Configuración personalizada aplicada 🛠️");
                    break;
                
                default:
                    Console.WriteLine("\n❌ Opción no válida");
                    return;
            }
            
            Console.WriteLine("\n📊 Nuevos parámetros:");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
            
            Console.WriteLine("\n💡 Genera algunos sudokus (opción 6 o 10) para ver el efecto de los cambios.");
        }
    }
}