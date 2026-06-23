using System;
using System.Collections.Generic;
using System.Linq;
namespace SudokuML.MachineLearning
{
    /// <summary>
    /// Representa el estado actual del Sudoku con características para el agente de RL
    /// </summary>
    [Serializable]
    public class SudokuState
    {
        #region Propiedades
        public int CeldasLlenadas { get; set; }
        public int TotalCeldas { get; set; }
        public int ValorActual { get; set; }
        public int CuadranteActual { get; set; }
        public int BacktrackingCount { get; set; }
        public int UltimoBacktrackingCount { get; set; }
        
        // Métricas de conflicto
        public int ConflictosFilas { get; set; }
        public int ConflictosColumnas { get; set; }
        public int ConflictosCuadrantes { get; set; }
        
        // Información contextual
        public Dictionary<int, int> CeldasPorCuadrante { get; set; }
        public Dictionary<int, int> ValoresPorFila { get; set; }
        public Dictionary<int, int> ValoresPorColumna { get; set; }
        
        public List<SudokuGenerator.Celda> ListaCeldas { get; set; }
        #endregion

        #region Constructor
        public SudokuState()
        {
            CeldasPorCuadrante = new Dictionary<int, int>();
            ValoresPorFila = new Dictionary<int, int>();
            ValoresPorColumna = new Dictionary<int, int>();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Extrae características numéricas del estado y la celda candidata
        /// Estas características son usadas por el agente para tomar decisiones
        /// </summary>
        public List<double> ExtraerCaracteristicas(SudokuGenerator.Celda celda)
        {
            var features = new List<double>();

            // 1. Progreso general (0-1)
            features.Add(TotalCeldas > 0 ? (double)CeldasLlenadas / TotalCeldas : 0);

            // 2. Posición de la celda normalizada
            features.Add((double)celda.IdCuadrante / 9.0);
            features.Add((double)celda.EjeX / 3.0);
            features.Add((double)celda.EjeY / 3.0);

            // 3. Densidad del cuadrante (cuántas celdas llenas tiene)
            int celdasEnCuadrante = CeldasPorCuadrante.ContainsKey(celda.IdCuadrante) 
                ? CeldasPorCuadrante[celda.IdCuadrante] : 0;
            features.Add(celdasEnCuadrante / 9.0);

            // 4. Conflictos en la fila/columna
            int filaKey = celda.CuadranteEjeY * 10 + celda.EjeY;
            int columnaKey = celda.CuadranteEjeX * 10 + celda.EjeX;
            
            int valoresEnFila = ValoresPorFila.ContainsKey(filaKey) ? ValoresPorFila[filaKey] : 0;
            int valoresEnColumna = ValoresPorColumna.ContainsKey(columnaKey) ? ValoresPorColumna[columnaKey] : 0;
            
            features.Add(valoresEnFila / 9.0);
            features.Add(valoresEnColumna / 9.0);

            // 5. Tasa de backtracking normalizada
            features.Add(Math.Min(1.0, BacktrackingCount / 100.0));

            // 6. Peso de la celda (preferencia actual basada en warnings)
            features.Add(Math.Min(1.0, celda.Peso / 10.0));

            // 7. Progreso del valor actual
            features.Add(ValorActual / 9.0);

            return features;
        }

        /// <summary>
        /// Calcula el potencial del estado actual (qué tan prometedor es)
        /// </summary>
        public double CalcularPotencial()
        {
            if (TotalCeldas == 0) return 0;

            double progresoPositivo = (double)CeldasLlenadas / TotalCeldas;
            double penalizacionBacktracking = 1.0 / (1.0 + BacktrackingCount * 0.01);
            double equilibrioCuadrantes = CalcularEquilibrioCuadrantes();

            return progresoPositivo * penalizacionBacktracking * equilibrioCuadrantes;
        }

        /// <summary>
        /// Calcula qué tan equilibrado está el llenado entre cuadrantes
        /// </summary>
        private double CalcularEquilibrioCuadrantes()
        {
            if (CeldasPorCuadrante.Count == 0) return 1.0;

            var valores = CeldasPorCuadrante.Values.ToList();
            double promedio = valores.Average();
            double varianza = valores.Sum(v => Math.Pow(v - promedio, 2)) / valores.Count;
            
            // Menor varianza = mejor equilibrio
            return 1.0 / (1.0 + varianza * 0.1);
        }

        /// <summary>
        /// Crea una copia del estado actual
        /// </summary>
        public SudokuState Clonar()
        {
            return new SudokuState
            {
                CeldasLlenadas = this.CeldasLlenadas,
                TotalCeldas = this.TotalCeldas,
                ValorActual = this.ValorActual,
                CuadranteActual = this.CuadranteActual,
                BacktrackingCount = this.BacktrackingCount,
                UltimoBacktrackingCount = this.BacktrackingCount,
                ConflictosFilas = this.ConflictosFilas,
                ConflictosColumnas = this.ConflictosColumnas,
                ConflictosCuadrantes = this.ConflictosCuadrantes,
                CeldasPorCuadrante = new Dictionary<int, int>(this.CeldasPorCuadrante),
                ValoresPorFila = new Dictionary<int, int>(this.ValoresPorFila),
                ValoresPorColumna = new Dictionary<int, int>(this.ValoresPorColumna),
                ListaCeldas = this.ListaCeldas
            };
        }
        #endregion
    }
}