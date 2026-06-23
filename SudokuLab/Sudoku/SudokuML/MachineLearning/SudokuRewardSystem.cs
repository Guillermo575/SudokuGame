using System;
namespace SudokuML.MachineLearning
{
    /// <summary>
    /// Sistema de recompensas para el agente de Reinforcement Learning
    /// Define cómo se premia o castiga al agente según sus acciones
    /// </summary>
    public static class SudokuRewardSystem
    {
        #region Constantes de Recompensa
        // Recompensas positivas
        private const double RECOMPENSA_PROGRESO = 1.0;              // Por cada celda colocada exitosamente
        private const double RECOMPENSA_SIN_BACKTRACK = 5.0;         // Bonus por completar un valor sin backtracking
        private const double RECOMPENSA_CUADRANTE_COMPLETO = 3.0;    // Bonus por completar un cuadrante
        private const double RECOMPENSA_SUDOKU_COMPLETO = 100.0;     // Bonus por completar el sudoku
        
        // Penalizaciones
        private const double PENALIZACION_BACKTRACK = -2.0;          // Por cada backtracking
        private const double PENALIZACION_BACKTRACK_EXCESIVO = -5.0; // Si hay muchos backtracks consecutivos
        private const double PENALIZACION_FALLO = -50.0;             // Si el sudoku no se puede completar
        
        // Umbrales
        private const int UMBRAL_BACKTRACK_EXCESIVO = 5;
        #endregion

        #region Métodos de Cálculo de Recompensa
        /// <summary>
        /// Calcula la recompensa para una acción de colocar una celda exitosamente
        /// </summary>
        public static double CalcularRecompensaExito(SudokuState estadoAnterior, SudokuState nuevoEstado, 
                                                       bool cuadranteCompleto, bool valorCompleto)
        {
            double recompensa = RECOMPENSA_PROGRESO;

            // Bonus si no hubo backtracking reciente
            if (nuevoEstado.BacktrackingCount == estadoAnterior.BacktrackingCount)
            {
                recompensa += 0.5; // Pequeño bonus por mantener la racha
            }

            // Gran bonus si completamos un valor sin backtracking
            if (valorCompleto && nuevoEstado.BacktrackingCount == estadoAnterior.BacktrackingCount)
            {
                recompensa += RECOMPENSA_SIN_BACKTRACK;
            }

            // Bonus adicional por completar cuadrante
            if (cuadranteCompleto)
            {
                recompensa += RECOMPENSA_CUADRANTE_COMPLETO;
            }

            // Factor multiplicador basado en progreso (más avanzado = más valioso)
            double factorProgreso = 1.0 + ((double)nuevoEstado.CeldasLlenadas / nuevoEstado.TotalCeldas) * 0.5;
            recompensa *= factorProgreso;

            return recompensa;
        }

        /// <summary>
        /// Calcula la penalización por backtracking
        /// </summary>
        public static double CalcularPenalizacionBacktrack(SudokuState estadoAnterior, SudokuState nuevoEstado)
        {
            double penalizacion = PENALIZACION_BACKTRACK;

            // Penalización aumentada si hay múltiples backtracks consecutivos
            int backtracksDelta = nuevoEstado.BacktrackingCount - estadoAnterior.UltimoBacktrackingCount;
            if (backtracksDelta >= UMBRAL_BACKTRACK_EXCESIVO)
            {
                penalizacion = PENALIZACION_BACKTRACK_EXCESIVO;
            }

            // Penalización mayor en etapas avanzadas (más costoso retroceder cuando estás cerca del final)
            double factorProgreso = (double)estadoAnterior.CeldasLlenadas / estadoAnterior.TotalCeldas;
            if (factorProgreso > 0.5)
            {
                penalizacion *= (1.0 + factorProgreso);
            }

            return penalizacion;
        }

        /// <summary>
        /// Calcula la recompensa por completar el sudoku exitosamente
        /// </summary>
        public static double CalcularRecompensaCompletado(int totalBacktracks, int totalCeldas)
        {
            double recompensa = RECOMPENSA_SUDOKU_COMPLETO;

            // Bonus adicional si se completó con poco backtracking
            double eficiencia = 1.0 - Math.Min(1.0, (double)totalBacktracks / totalCeldas);
            recompensa += eficiencia * 50.0;

            return recompensa;
        }

        /// <summary>
        /// Calcula la penalización por fallo total
        /// </summary>
        public static double CalcularPenalizacionFallo(int celdasCompletadas, int totalCeldas)
        {
            double penalizacion = PENALIZACION_FALLO;

            // Menor penalización si al menos se logró avanzar algo
            double progresoLogrado = (double)celdasCompletadas / totalCeldas;
            penalizacion *= (1.0 - progresoLogrado * 0.5);

            return penalizacion;
        }

        /// <summary>
        /// Calcula una recompensa intermedia basada en el potencial del estado
        /// </summary>
        public static double CalcularRecompensaPotencial(SudokuState estado)
        {
            // Recompensa pequeña basada en qué tan prometedor es el estado actual
            double potencial = estado.CalcularPotencial();
            return potencial * 0.1; // Recompensa muy pequeña para guiar suavemente
        }

        /// <summary>
        /// Calcula la recompensa total considerando todos los factores
        /// </summary>
        public static double CalcularRecompensaTotal(SudokuState estadoAnterior, SudokuState nuevoEstado,
                                                      bool exitoso, bool completado, bool fallo,
                                                      bool cuadranteCompleto, bool valorCompleto)
        {
            double recompensaTotal = 0;

            if (completado)
            {
                // Sudoku completado exitosamente
                recompensaTotal = CalcularRecompensaCompletado(
                    nuevoEstado.BacktrackingCount, 
                    nuevoEstado.TotalCeldas);
            }
            else if (fallo)
            {
                // Fallo total - no se pudo completar
                recompensaTotal = CalcularPenalizacionFallo(
                    estadoAnterior.CeldasLlenadas, 
                    estadoAnterior.TotalCeldas);
            }
            else if (exitoso)
            {
                // Colocación exitosa de una celda
                recompensaTotal = CalcularRecompensaExito(
                    estadoAnterior, 
                    nuevoEstado, 
                    cuadranteCompleto, 
                    valorCompleto);
            }
            else
            {
                // Backtracking
                recompensaTotal = CalcularPenalizacionBacktrack(estadoAnterior, nuevoEstado);
            }

            // Agregar recompensa de potencial (muy pequeña, solo para guiar)
            recompensaTotal += CalcularRecompensaPotencial(nuevoEstado);

            return recompensaTotal;
        }
        #endregion
    }
}