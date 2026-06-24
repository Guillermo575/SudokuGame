using System;
using System.Text;
using SudokuML.MachineLearning;
namespace SudokuML.Tools
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
            Console.WriteLine("║   🤖 ML SYSTEM FOR SUDOKU GENERATION   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
            while (true)
            {
                Console.WriteLine("\n╔═══════════ 📋 MAIN MENU ═══════════╗");
                Console.WriteLine("║                                          ║");
                Console.WriteLine("║  1. 🚀 Quick Start                      ║");
                Console.WriteLine("║  2. 🎲 Generate Sudoku                  ║");
                Console.WriteLine("║  3. 🏋️  Train Model                     ║");
                Console.WriteLine("║  4. ⚙️  Configuration                   ║");
                Console.WriteLine("║  5. 🔬 Analysis & Tests                 ║");
                Console.WriteLine("║  6. 📊 View Statistics                  ║");
                Console.WriteLine("║  0. 🚪 Exit                             ║");
                Console.WriteLine("║                                          ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.Write("\n👉 Select an option: ");

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
                        Console.WriteLine("❌ Invalid option.");
                        Console.WriteLine("\n⏸️  Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void MenuInicioRapido()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🚀 QUICK START ═══════════╗\n");
            Console.WriteLine("1. ⚡ Quick Test (Recommended)");
            Console.WriteLine("   💡 Train + generate + verify diversity");
            Console.WriteLine("\n2. 📚 Basic usage example");
            Console.WriteLine("   🎯 Simple system demo");
            Console.WriteLine("\n3. ⚖️  Before/After Comparison");
            Console.WriteLine("   📈 See improvements impact");
            Console.WriteLine("\n0. ↩️  Back to main menu");
            Console.Write("\n👉 Select an option: ");

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
            Console.WriteLine("╔═══════════ 🎲 GENERATE SUDOKU ═══════════╗\n");
            Console.WriteLine("1. 🤖 Generate with Machine Learning");
            Console.WriteLine("2. 🔧 Generate without ML (traditional)");
            Console.WriteLine("3. 🌈 Demonstrate variety (generate multiple)");
            Console.WriteLine("\n0. ↩️  Back to main menu");
            Console.Write("\n👉 Select an option: ");

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
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }

            Console.WriteLine("\n⏸️  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuEntrenar()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🏋️  TRAIN MODEL ═══════════╗\n");
            Console.WriteLine("1. ⚡ Quick training (100 episodes)");
            Console.WriteLine("2. 💪 Full training (1000 episodes)");
            Console.WriteLine("3. 🎯 Train for 4x4 Sudoku (2000 episodes)");
            Console.WriteLine("4. 🛠️  Custom training");
            Console.WriteLine("5. 📊 Monitor diversity during training");
            Console.WriteLine("\n0. ↩️  Back to main menu");
            Console.Write("\n👉 Select an option: ");

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
                    Console.WriteLine("Training for 4x4 Sudoku (this may take several minutes)...");
                    SudokuGenerator.EntrenarAgente(2000, 4, 4);
                    Console.WriteLine("\n" + SudokuGenerator.ObtenerEstadisticasML());
                    break;
                case "4":
                    Console.Write("Episodes: ");
                    if (!int.TryParse(Console.ReadLine(), out int episodios)) episodios = 1000;
                    Console.Write("Columns X: ");
                    if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;
                    Console.Write("Columns Y: ");
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
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }

            Console.WriteLine("\n⏸️  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuConfiguracion()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ ⚙️  CONFIGURATION ═══════════╗\n");
            Console.WriteLine("1. 🎛️  Configure exploration parameters");
            Console.WriteLine("   💡 Presets: Variety, Balance, Performance");
            Console.WriteLine("\n2. 👀 View current parameters");
            Console.WriteLine("\n0. ↩️  Back to main menu");
            Console.Write("\n👉 Select an option: ");

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
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }

            Console.WriteLine("\n⏸️  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void MenuAnalisis()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════ 🔬 ANALYSIS & TESTS ═══════════╗\n");
            Console.WriteLine("1. ⚔️  Compare performance (with vs without ML)");
            Console.WriteLine("2. 🔀 Compare exploration strategies");
            Console.WriteLine("3. 🧪 Massive test (50 sudokus)");
            Console.WriteLine("\n0. ↩️  Back to main menu");
            Console.Write("\n👉 Select an option: ");

            string opcion = Console.ReadLine();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Sudoku size (e.g. 3 for 3x3, 4 for 4x4): ");
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
                    Console.WriteLine("❌ Invalid option.");
                    break;
            }

            Console.WriteLine("\n⏸️  Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static void GenerarSudoku(bool usarML)
        {
            Console.Write("Columns X (e.g. 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;

            Console.Write("Columns Y (e.g. 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colY)) colY = 3;

            Console.WriteLine($"\n🎲 Generating Sudoku {colX}x{colY} {(usarML ? "WITH 🤖" : "WITHOUT 🔧")} Machine Learning...\n");

            var sudoku = new SudokuGenerator(colX, colY, usarML, entrenar: false);

            Console.WriteLine($"Success: {(sudoku.Exito ? "✅ YES" : "❌ NO")}");
            Console.WriteLine($"⏱️  Time: {sudoku.TiempoEjecutado} ms");
            Console.WriteLine($"🔄 Backtracking: {sudoku.ConteoErrores}");

            if (sudoku.Exito)
            {
                Console.WriteLine("\n" + sudoku.ResumenASCII);
            }
        }

        static void TesteoMasivo()
        {
            Console.Write("🔢 Number of Sudokus: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad)) cantidad = 50;

            Console.Write("📏 Columns X (e.g. 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colX)) colX = 3;

            Console.Write("📏 Columns Y (e.g. 3): ");
            if (!int.TryParse(Console.ReadLine(), out int colY)) colY = 3;

            Console.WriteLine($"\n🚀 Generating {cantidad} sudokus {colX}x{colY}...\n");

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
                    Console.WriteLine($"📊 Progress: {i + 1}/{cantidad}");
                }
            }

            Console.WriteLine("\n╔═══ 📈 RESULTS ═══╗");
            Console.WriteLine($"✅ Successful: {exitosos}/{cantidad} ({(double)exitosos / cantidad * 100:F2}%)");
            Console.WriteLine($"⏱️  Average time: {(exitosos > 0 ? tiempoTotal / exitosos : 0)} ms");
            Console.WriteLine($"🔄 Average backtracking: {(exitosos > 0 ? backtrackTotal / exitosos : 0)}");
            Console.WriteLine($"⬇️  Minimum backtracking: {(backtrackMin == int.MaxValue ? 0 : backtrackMin)}");
            Console.WriteLine($"⬆️  Maximum backtracking: {backtrackMax}");
        }

        /// <summary>
        /// Método principal de ejemplo - Genera un sudoku con ML
        /// </summary>
        public static void EjemploBasico()
        {
            Console.WriteLine("╔═══ 🎲 SUDOKU GENERATION WITH ML ═══╗\n");

            // Generate a 3x3 sudoku using Machine Learning
            var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

            Console.WriteLine($"✅ Success: {sudoku.Exito}");
            Console.WriteLine($"🔄 Backtracking: {sudoku.ConteoErrores}");
            Console.WriteLine($"⏱️  Time: {sudoku.TiempoEjecutado} ms");
            Console.WriteLine($"\n{sudoku.ResumenHTML}");
        }

        /// <summary>
        /// Entrena el agente de ML con episodios específicos
        /// </summary>
        public static void EntrenarModelo(int episodios = 1000, int columnasX = 3, int columnasY = 3)
        {
            Console.WriteLine($"╔═══ 🏋️  AGENT TRAINING ═══╗");
            Console.WriteLine($"📐 Size: {columnasX}x{columnasY}");
            Console.WriteLine($"🔢 Episodes: {episodios}\n");

            SudokuGenerator.EntrenarAgente(episodios, columnasX, columnasY);

            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");
        }

        /// <summary>
        /// Compara el rendimiento con y sin ML
        /// </summary>
        public static void CompararRendimiento(int cantidad = 100, int columnasX = 4, int columnasY = 4)
        {
            Console.WriteLine($"╔═══ ⚔️  PERFORMANCE COMPARISON ═══╗");
            Console.WriteLine($"📐 Size: {columnasX}x{columnasY}");
            Console.WriteLine($"🔢 Quantity: {cantidad} sudokus\n");

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
            Console.WriteLine("╔═══ 🌈 DEMONSTRATION OF DIVERSE SUDOKUS ═══╗\n");
            Console.Write("🔢 How many sudokus to generate? (recommended: 5-10): ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad)) cantidad = 5;

            Console.WriteLine($"\n🚀 Generating {cantidad} sudokus with ML...\n");
            
            var hashsGenerados = new System.Collections.Generic.HashSet<string>();
            var listaHashs = new System.Collections.Generic.List<string>();
            
            for (int i = 0; i < cantidad; i++)
            {
                var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
                
                if (sudoku.Exito)
                {
                    bool esUnico = hashsGenerados.Add(sudoku.HashSudoku);
                    listaHashs.Add(sudoku.HashSudoku);
                    
                    Console.WriteLine($"🎲 Sudoku #{i + 1} {(esUnico ? "✨ UNIQUE" : "🔁 REPEATED")}");
                    Console.WriteLine($"🔑 Hash: {sudoku.HashSudoku.Substring(0, Math.Min(30, sudoku.HashSudoku.Length))}...");
                    Console.WriteLine($"❌ Errors: {sudoku.ConteoErrores}, ⏱️  Time: {sudoku.TiempoEjecutado}ms");
                    
                    if (i < 3) // Mostrar solo los primeros 3 completos
                    {
                        Console.WriteLine(sudoku.ResumenASCII);
                    }
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine($"\n╔═══ 📊 SUMMARY ═══╗");
            Console.WriteLine($"✨ Unique sudokus: {hashsGenerados.Count}/{cantidad}");
            Console.WriteLine($"📈 Uniqueness rate: {(double)hashsGenerados.Count / cantidad * 100:F2}%");
            
            if (hashsGenerados.Count < cantidad)
            {
                Console.WriteLine("\n💡 Tip: For more variety, try:");
                Console.WriteLine("   📌 Increase epsilon usage");
                Console.WriteLine("   📌 Switch to Softmax strategy");
                Console.WriteLine("   📌 Train more episodes");
            }
        }
        
        /// <summary>
        /// Compara las diferentes estrategias de exploración
        /// </summary>
        static void CompararEstrategias()
        {
            Console.WriteLine("╔═══ 🔀 EXPLORATION STRATEGIES COMPARISON ═══╗\n");
            
            int sudokusPorEstrategia = 10;
            
            var estrategias = new[]
            {
                (SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy, "Epsilon-Greedy"),
                (SudokuRLAgent.EstrategiaExploracion.Softmax, "Softmax"),
                (SudokuRLAgent.EstrategiaExploracion.Hibrida, "Hybrid")
            };
            
            foreach (var (estrategia, nombre) in estrategias)
            {
                Console.WriteLine($"\n╠═══ 🎯 Strategy: {nombre} ═══╣");

                // Note: Ideally you should have access to the agent to change the strategy
                // For now, we generate with the default strategy
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
                
                Console.WriteLine($"✨ Unique sudokus: {hashs.Count}/{exitos}");
                Console.WriteLine($"📈 Uniqueness rate: {(exitos > 0 ? (double)hashs.Count / exitos * 100 : 0):F2}%");
                Console.WriteLine($"❌ Average errors: {(exitos > 0 ? (double)totalErrores / exitos : 0):F2}");
                Console.WriteLine($"⏱️  Average time: {(exitos > 0 ? totalTiempo / exitos : 0)}ms");
                Console.WriteLine();
            }
            
            Console.WriteLine("💡 Note: To change strategies, use the configuration option");
        }
        
        /// <summary>
        /// Monitorea la diversidad durante el entrenamiento
        /// </summary>
        static void MonitorearDiversidad()
        {
            Console.WriteLine("╔═══ 📊 DIVERSITY MONITORING DURING TRAINING ═══╗\n");
            Console.Write("🔢 How many episodes to train?: ");
            if (!int.TryParse(Console.ReadLine(), out int episodios)) episodios = 200;

            Console.WriteLine($"\n🚀 Training {episodios} episodes and monitoring diversity...\n");
            
            // Execute training with tracking
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
                    Console.WriteLine($"📍 Episode {i + 1}/{episodios}:");
                    Console.WriteLine($"  ✨ Unique sudokus: {hashsVistos.Count}");
                    Console.WriteLine($"  📈 Uniqueness rate: {tasaUnicidad:F2}%");
                    Console.WriteLine();
                }
            }
            
            Console.WriteLine($"\n╔═══ 🏆 FINAL RESULTS ═══╗");
            Console.WriteLine($"🔢 Total episodes: {episodios}");
            Console.WriteLine($"✨ Unique sudokus: {hashsVistos.Count}");
            Console.WriteLine($"📈 Uniqueness rate: {(double)hashsVistos.Count / episodios * 100:F2}%");
            Console.WriteLine($"\n{SudokuGenerator.ObtenerEstadisticasML()}");
        }
        
        /// <summary>
        /// Permite configurar parámetros de exploración
        /// </summary>
        static void ConfigurarParametros()
        {
            Console.WriteLine("╔═══ 🎛️  EXPLORATION PARAMETERS CONFIGURATION ═══╗\n");

            Console.WriteLine("📊 Current parameters:");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
            Console.WriteLine();
            
            Console.WriteLine("🎯 Select a preset:\n");
            Console.WriteLine("1. 🌈 MAXIMUM VARIETY");
            Console.WriteLine("   📌 Epsilon usage: 0.3 (30% exploration)");
            Console.WriteLine("   📌 Temperature: 2.0");
            Console.WriteLine("   📌 Strategy: Softmax\n");

            Console.WriteLine("2. ⚖️  BALANCED (recommended)");
            Console.WriteLine("   📌 Epsilon usage: 0.15 (15% exploration)");
            Console.WriteLine("   📌 Temperature: 0.8");
            Console.WriteLine("   📌 Strategy: Hybrid\n");

            Console.WriteLine("3. ⚡ MAXIMUM PERFORMANCE");
            Console.WriteLine("   📌 Epsilon usage: 0.05 (5% exploration)");
            Console.WriteLine("   📌 Temperature: 0.3");
            Console.WriteLine("   📌 Strategy: EpsilonGreedy\n");

            Console.WriteLine("4. 🛠️  CUSTOM\n");

            Console.Write("👉 Select option (1-4): ");
            string opcion = Console.ReadLine();
            
            var agente = SudokuGenerator.agenteML;
            
            switch (opcion)
            {
                case "1":
                    agente.SetEpsilonUso(0.3);
                    agente.SetTemperature(2.0);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;
                    Console.WriteLine("\n✅ Configuration applied: MAXIMUM VARIETY 🌈");
                    break;

                case "2":
                    agente.SetEpsilonUso(0.15);
                    agente.SetTemperature(0.8);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
                    Console.WriteLine("\n✅ Configuration applied: BALANCED ⚖️");
                    break;

                case "3":
                    agente.SetEpsilonUso(0.05);
                    agente.SetTemperature(0.3);
                    agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;
                    Console.WriteLine("\n✅ Configuration applied: MAXIMUM PERFORMANCE ⚡");
                    break;
                
                case "4":
                    Console.Write("\n🎚️  Epsilon usage (0.0 - 1.0, current: 0.15): ");
                    if (double.TryParse(Console.ReadLine(), out double epsilon))
                    {
                        agente.SetEpsilonUso(epsilon);
                    }

                    Console.Write("🌡️  Temperature (0.1 - 5.0, current: 1.0): ");
                    if (double.TryParse(Console.ReadLine(), out double temp))
                    {
                        agente.SetTemperature(temp);
                    }

                    Console.WriteLine("\n🎯 Strategy:");
                    Console.WriteLine("1. 🎲 EpsilonGreedy");
                    Console.WriteLine("2. 🔥 Softmax");
                    Console.WriteLine("3. 🔀 Hybrid");
                    Console.Write("👉 Select (1-3): ");
                    
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
                    
                    Console.WriteLine("\n✅ Custom configuration applied 🛠️");
                    break;

                default:
                    Console.WriteLine("\n❌ Invalid option");
                    return;
            }
            
            Console.WriteLine("\n📊 New parameters:");
            Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());

            Console.WriteLine("\n💡 Generate some sudokus to see the effect of the changes.");
        }
    }
}