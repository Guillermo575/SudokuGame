using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
namespace Sudoku.MachineLearning
{
    /// <summary>
    /// Agente de Reinforcement Learning para optimizar la selección de celdas en Sudoku
    /// Usa Q-Learning con aproximación de funciones para reducir backtracking
    /// </summary>
    [Serializable]
    public class SudokuRLAgent
    {
        #region Variables
        private Dictionary<string, double> QTable = new Dictionary<string, double>();
        private double learningRate = 0.1; // Alpha: tasa de aprendizaje
        private double discountFactor = 0.95; // Gamma: factor de descuento para recompensas futuras
        private double epsilonEntrenamiento = 0.3; // Epsilon para entrenamiento (mayor exploración)
        private double epsilonUso = 0.15; // Epsilon para uso normal (mantiene variabilidad)
        private double epsilonMin = 0.05; // Epsilon mínimo
        private double temperature = 1.0; // Temperatura para Softmax/Boltzmann
        private Random rnd;
        
        // Parámetros de normalización
        private const int MAX_FEATURES = 10;
        
        // Estadísticas de entrenamiento
        public int EpisodiosEntrenados { get; private set; } = 0;
        public double RecompensaPromedio { get; private set; } = 0;
        private List<double> historicoRecompensas = new List<double>();
        
        // Tracking de diversidad
        private HashSet<string> sudokusGenerados = new HashSet<string>();
        public int SudokusUnicos => sudokusGenerados.Count;
        
        // Estrategia de exploración
        public enum EstrategiaExploracion { EpsilonGreedy, Softmax, Hibrida }
        public EstrategiaExploracion Estrategia { get; set; } = EstrategiaExploracion.Hibrida;
        #endregion

        #region Constructor
        public SudokuRLAgent()
        {
            // Semilla aleatoria basada en tiempo para variabilidad
            rnd = new Random(Guid.NewGuid().GetHashCode());
            // Intentar cargar modelo pre-entrenado
            CargarModelo();
        }

        public SudokuRLAgent(double learningRate, double discountFactor, double epsilonEntrenamiento, double epsilonUso)
        {
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.epsilonEntrenamiento = epsilonEntrenamiento;
            this.epsilonUso = epsilonUso;
            rnd = new Random(Guid.NewGuid().GetHashCode());
            CargarModelo();
        }
        #endregion

        #region Métodos Principales
        /// <summary>
        /// Selecciona la mejor celda de la lista usando Q-Learning con múltiples estrategias
        /// </summary>
        public SudokuGenerator.Celda SeleccionarCelda(List<SudokuGenerator.Celda> lstCeldas, SudokuState estado, bool entrenamiento = false)
        {
            if (lstCeldas == null || lstCeldas.Count == 0)
                return null;

            if (lstCeldas.Count == 1)
                return lstCeldas[0];

            // Determinar epsilon según el modo
            double epsilonActual = entrenamiento ? epsilonEntrenamiento : epsilonUso;

            // Seleccionar estrategia
            switch (Estrategia)
            {
                case EstrategiaExploracion.EpsilonGreedy:
                    return SeleccionarEpsilonGreedy(lstCeldas, estado, epsilonActual);
                
                case EstrategiaExploracion.Softmax:
                    return SeleccionarSoftmax(lstCeldas, estado);
                
                case EstrategiaExploracion.Hibrida:
                default:
                    // En entrenamiento: más exploración con Softmax
                    // En uso: balance entre epsilon-greedy y softmax
                    if (entrenamiento || rnd.NextDouble() < 0.3)
                        return SeleccionarSoftmax(lstCeldas, estado);
                    else
                        return SeleccionarEpsilonGreedy(lstCeldas, estado, epsilonActual);
            }
        }
        
        /// <summary>
        /// Selección usando Epsilon-Greedy
        /// </summary>
        private SudokuGenerator.Celda SeleccionarEpsilonGreedy(List<SudokuGenerator.Celda> lstCeldas, SudokuState estado, double epsilon)
        {
            bool explorar = rnd.NextDouble() < epsilon;

            if (explorar)
            {
                // Exploración: selección aleatoria
                return lstCeldas[rnd.Next(lstCeldas.Count)];
            }
            else
            {
                // Explotación: seleccionar la mejor acción según Q-Table
                return ObtenerMejorCelda(lstCeldas, estado);
            }
        }
        
        /// <summary>
        /// Selección usando Softmax/Boltzmann (probabilística basada en Q-Values)
        /// Proporciona más variedad que epsilon-greedy puro
        /// </summary>
        private SudokuGenerator.Celda SeleccionarSoftmax(List<SudokuGenerator.Celda> lstCeldas, SudokuState estado)
        {
            // Calcular Q-Values para todas las celdas
            var qValues = lstCeldas.Select(c => ObtenerQValue(estado, c)).ToList();
            
            // Aplicar Softmax con temperatura
            var probabilidades = CalcularProbabilidadesSoftmax(qValues, temperature);
            
            // Selección probabilística
            double random = rnd.NextDouble();
            double acumulado = 0;
            
            for (int i = 0; i < lstCeldas.Count; i++)
            {
                acumulado += probabilidades[i];
                if (random <= acumulado)
                    return lstCeldas[i];
            }
            
            return lstCeldas[lstCeldas.Count - 1];
        }
        
        /// <summary>
        /// Obtiene la celda con el mayor Q-Value
        /// </summary>
        private SudokuGenerator.Celda ObtenerMejorCelda(List<SudokuGenerator.Celda> lstCeldas, SudokuState estado)
        {
            SudokuGenerator.Celda mejorCelda = null;
            double mejorQValue = double.MinValue;

            foreach (var celda in lstCeldas)
            {
                double qValue = ObtenerQValue(estado, celda);
                if (qValue > mejorQValue)
                {
                    mejorQValue = qValue;
                    mejorCelda = celda;
                }
            }

            return mejorCelda ?? lstCeldas[0];
        }
        
        /// <summary>
        /// Calcula probabilidades usando Softmax
        /// </summary>
        private List<double> CalcularProbabilidadesSoftmax(List<double> qValues, double temp)
        {
            // Encontrar el máximo para estabilidad numérica
            double maxQ = qValues.Max();
            
            // Calcular exponenciales
            var exp = qValues.Select(q => Math.Exp((q - maxQ) / temp)).ToList();
            double sumaExp = exp.Sum();
            
            // Normalizar
            return exp.Select(e => e / sumaExp).ToList();
        }

        /// <summary>
        /// Actualiza el Q-Value después de una acción
        /// </summary>
        public void ActualizarQValue(SudokuState estadoAnterior, SudokuGenerator.Celda accionTomada, 
                                      double recompensa, SudokuState nuevoEstado, bool episodioTerminado)
        {
            string claveEstadoAccion = GenerarClave(estadoAnterior, accionTomada);
            double qActual = ObtenerQValue(estadoAnterior, accionTomada);

            double maxQFuturo = 0;
            if (!episodioTerminado && nuevoEstado != null)
            {
                // Obtener el máximo Q-Value del siguiente estado
                maxQFuturo = ObtenerMaxQValueFuturo(nuevoEstado);
            }

            // Fórmula de Q-Learning: Q(s,a) = Q(s,a) + ? * (r + ? * max(Q(s',a')) - Q(s,a))
            double nuevoQValue = qActual + learningRate * (recompensa + discountFactor * maxQFuturo - qActual);

            if (QTable.ContainsKey(claveEstadoAccion))
                QTable[claveEstadoAccion] = nuevoQValue;
            else
                QTable.Add(claveEstadoAccion, nuevoQValue);
        }

        /// <summary>
        /// Registra el final de un episodio de entrenamiento
        /// </summary>
        public void RegistrarEpisodio(double recompensaTotal, string hashSudoku = null)
        {
            EpisodiosEntrenados++;
            historicoRecompensas.Add(recompensaTotal);
            
            // Registrar sudoku único
            if (!string.IsNullOrEmpty(hashSudoku))
                sudokusGenerados.Add(hashSudoku);
            
            // Mantener solo los últimos 1000 episodios para el promedio
            if (historicoRecompensas.Count > 1000)
                historicoRecompensas.RemoveAt(0);

            RecompensaPromedio = historicoRecompensas.Average();

            // Decay del epsilon de entrenamiento (reducir exploración gradualmente)
            if (epsilonEntrenamiento > epsilonMin)
                epsilonEntrenamiento *= 0.9995;
            
            // Ajustar temperatura de Softmax
            if (temperature > 0.5)
                temperature *= 0.9998;

            // Guardar modelo cada 100 episodios
            if (EpisodiosEntrenados % 100 == 0)
                GuardarModelo();
        }
        #endregion

        #region Métodos Privados
        private double ObtenerQValue(SudokuState estado, SudokuGenerator.Celda celda)
        {
            string clave = GenerarClave(estado, celda);
            return QTable.ContainsKey(clave) ? QTable[clave] : 0.0;
        }

        private double ObtenerMaxQValueFuturo(SudokuState estado)
        {
            // Para simplificar, asumimos que el Q-Value futuro es un promedio de características
            // En una implementación completa, evaluaríamos todas las posibles acciones
            return estado.CalcularPotencial() * 0.1;
        }

        private string GenerarClave(SudokuState estado, SudokuGenerator.Celda celda)
        {
            // Generar una clave única basada en características del estado y la acción
            var features = estado.ExtraerCaracteristicas(celda);
            return string.Join("_", features.Select(f => Math.Round(f, 2).ToString()));
        }

        private void GuardarModelo()
        {
            try
            {
                string rutaModelo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SudokuRLModel.json");
                var modeloData = new
                {
                    QTable = QTable,
                    EpisodiosEntrenados = EpisodiosEntrenados,
                    RecompensaPromedio = RecompensaPromedio,
                    EpsilonEntrenamiento = epsilonEntrenamiento,
                    EpsilonUso = epsilonUso,
                    Temperature = temperature,
                    SudokusUnicos = sudokusGenerados.Count,
                    Estrategia = Estrategia.ToString()
                };
                string json = JsonConvert.SerializeObject(modeloData, Formatting.Indented);
                File.WriteAllText(rutaModelo, json);
            }
            catch (Exception)
            {
                // No detener ejecución si falla el guardado
            }
        }

        private void CargarModelo()
        {
            try
            {
                string rutaModelo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SudokuRLModel.json");
                if (File.Exists(rutaModelo))
                {
                    string json = File.ReadAllText(rutaModelo);
                    var modeloData = JsonConvert.DeserializeObject<dynamic>(json);
                    
                    QTable = JsonConvert.DeserializeObject<Dictionary<string, double>>(modeloData.QTable.ToString());
                    EpisodiosEntrenados = modeloData.EpisodiosEntrenados;
                    RecompensaPromedio = modeloData.RecompensaPromedio;
                    
                    // Cargar nuevos parámetros con valores por defecto si no existen
                    if (modeloData.EpsilonEntrenamiento != null)
                        epsilonEntrenamiento = modeloData.EpsilonEntrenamiento;
                    if (modeloData.EpsilonUso != null)
                        epsilonUso = modeloData.EpsilonUso;
                    if (modeloData.Temperature != null)
                        temperature = modeloData.Temperature;
                    if (modeloData.Estrategia != null)
                        Estrategia = Enum.Parse<EstrategiaExploracion>(modeloData.Estrategia.ToString());
                }
            }
            catch (Exception)
            {
                // Si falla la carga, comenzar con Q-Table vacía
                QTable = new Dictionary<string, double>();
            }
        }
        #endregion

        #region Métodos de Configuración
        public void SetLearningRate(double rate)
        {
            learningRate = Math.Max(0.001, Math.Min(1.0, rate));
        }

        public void SetEpsilon(double eps)
        {
            epsilonEntrenamiento = Math.Max(0.0, Math.Min(1.0, eps));
            epsilonUso = Math.Max(0.0, Math.Min(1.0, eps * 0.5));
        }
        
        public void SetEpsilonEntrenamiento(double eps)
        {
            epsilonEntrenamiento = Math.Max(0.0, Math.Min(1.0, eps));
        }
        
        public void SetEpsilonUso(double eps)
        {
            epsilonUso = Math.Max(0.0, Math.Min(1.0, eps));
        }
        
        public void SetTemperature(double temp)
        {
            temperature = Math.Max(0.1, temp);
        }

        public void ResetearModelo()
        {
            QTable.Clear();
            EpisodiosEntrenados = 0;
            historicoRecompensas.Clear();
            RecompensaPromedio = 0;
            epsilonEntrenamiento = 0.3;
            epsilonUso = 0.15;
            temperature = 1.0;
            sudokusGenerados.Clear();
        }
        #endregion
    }
}