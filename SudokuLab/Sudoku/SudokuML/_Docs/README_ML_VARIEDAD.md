# Generación de Sudokus con Variedad usando Machine Learning

## Cambios Implementados

Se han implementado mejoras significativas en el agente de Reinforcement Learning para generar sudokus diversos y evitar repeticiones:

### 1. **Doble Epsilon (Entrenamiento vs Uso)**
- **`epsilonEntrenamiento`**: 0.3 (30% de exploración durante entrenamiento)
- **`epsilonUso`**: 0.15 (15% de exploración en uso normal)
- Esto mantiene variabilidad incluso después del entrenamiento

### 2. **Múltiples Estrategias de Exploración**

#### Epsilon-Greedy
- Exploración aleatoria vs explotación del mejor Q-Value
- Simple pero efectivo

#### Softmax/Boltzmann
- Selección probabilística basada en Q-Values
- Proporciona más variedad que epsilon-greedy puro
- Controlada por parámetro de temperatura

#### Estrategia Híbrida (Recomendada)
- Combina ambas estrategias
- En entrenamiento: más Softmax para mayor exploración
- En uso: balance entre ambas para variedad controlada

### 3. **Tracking de Sudokus Únicos**
- El agente mantiene un `HashSet` de sudokus generados
- Permite monitorear la diversidad durante el entrenamiento
- Propiedad `SudokusUnicos` disponible

### 4. **Semilla Aleatoria Dinámica**
- Cada instancia de `SudokuGenerator` usa una semilla única basada en `Guid`
- Elimina la predictibilidad del generador de números aleatorios

### 5. **Temperatura Adaptativa**
- La temperatura de Softmax se ajusta durante el entrenamiento
- Inicia en 1.0 y decae gradualmente
- Controla el balance entre exploración y explotación

## Ejemplos de Uso

### Entrenar el Agente con Diversidad

```csharp
// Entrenar con 1000 episodios (genera múltiples sudokus diferentes)
SudokuGenerator.EntrenarAgente(episodios: 1000, columnasX: 3, columnasY: 3);

// Ver estadísticas
Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
// Output: "Agente ML - Episodios: 1000, Recompensa Promedio: 85.32, Sudokus Únicos: 987, Estrategia: Hibrida"
```

### Generar Sudokus Diversos con ML

```csharp
// Generar múltiples sudokus con ML activado
for (int i = 0; i < 10; i++)
{
    var sudoku = new SudokuGenerator(
        ColumnasX: 3, 
        ColumnasY: 3, 
        usarML: true,      // Usar Machine Learning
        entrenar: false    // Modo uso (no entrenamiento)
    );
    
    if (sudoku.Exito)
    {
        Console.WriteLine($"Sudoku {i+1}: {sudoku.HashSudoku}");
        Console.WriteLine(sudoku.ResumenASCII);
    }
}
```

### Configurar Estrategia de Exploración

```csharp
// Obtener acceso al agente (necesitarás hacer público el campo agenteML)
var agente = SudokuGenerator.agenteML;

// Usar solo Epsilon-Greedy
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;

// Usar solo Softmax
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;

// Usar estrategia híbrida (recomendado)
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
```

### Ajustar Parámetros de Exploración

```csharp
var agente = new SudokuRLAgent();

// Configurar epsilon de entrenamiento y uso
agente.SetEpsilonEntrenamiento(0.4);  // 40% exploración en entrenamiento
agente.SetEpsilonUso(0.2);            // 20% exploración en uso

// Ajustar temperatura de Softmax
agente.SetTemperature(1.5);  // Mayor temperatura = más exploración

// Restaurar valores por defecto
agente.ResetearModelo();
```

### Generar sin ML (Método Tradicional con Variedad)

```csharp
// Aunque sin ML, ahora también tiene semilla aleatoria única
var sudoku = new SudokuGenerator(
    ColumnasX: 3, 
    ColumnasY: 3, 
    usarML: false,  // Desactivar ML
    entrenar: false
);

Console.WriteLine(sudoku.ResumenASCII);
```

## Parámetros Recomendados

### Para Máxima Variedad Durante Entrenamiento
```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonEntrenamiento(0.5);  // 50% exploración
agente.SetTemperature(2.0);           // Alta temperatura
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Softmax;
```

### Para Balance entre Rendimiento y Variedad en Uso
```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonUso(0.15);           // 15% exploración
agente.SetTemperature(0.8);           // Temperatura moderada
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.Hibrida;
```

### Para Máximo Rendimiento (Menos Variedad)
```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonUso(0.05);           // 5% exploración
agente.SetTemperature(0.3);           // Baja temperatura
agente.Estrategia = SudokuRLAgent.EstrategiaExploracion.EpsilonGreedy;
```

## Monitoreo de Diversidad

```csharp
// Entrenar y verificar diversidad
SudokuGenerator.EntrenarAgente(1000);

var agente = SudokuGenerator.agenteML;
Console.WriteLine($"Episodios: {agente.EpisodiosEntrenados}");
Console.WriteLine($"Sudokus únicos generados: {agente.SudokusUnicos}");
Console.WriteLine($"Tasa de unicidad: {(double)agente.SudokusUnicos / agente.EpisodiosEntrenados * 100:F2}%");
```

## Cómo Funciona

### Softmax/Boltzmann
En lugar de elegir siempre la mejor celda, Softmax asigna probabilidades a cada celda basándose en sus Q-Values:

```
P(celda_i) = exp(Q(celda_i) / temperature) / ? exp(Q(celda_j) / temperature)
```

- **Alta temperatura**: Selección más uniforme (más exploración)
- **Baja temperatura**: Favorece celdas con alto Q-Value (más explotación)

### Decaimiento de Parámetros
Durante el entrenamiento, los parámetros se ajustan automáticamente:

```csharp
epsilonEntrenamiento *= 0.9995;  // Decay cada episodio
temperature *= 0.9998;            // Decay cada episodio
```

Esto permite:
- **Inicio**: Máxima exploración para descubrir estrategias
- **Medio**: Balance progresivo
- **Final**: Más explotación de estrategias aprendidas

## Ventajas de las Mejoras

1. ? **Genera sudokus diferentes en cada ejecución**
2. ? **Mantiene la calidad del ML** (reduce backtracking)
3. ? **Configurable según necesidades** (variedad vs rendimiento)
4. ? **Tracking de diversidad** durante entrenamiento
5. ? **No es determinista** incluso después del entrenamiento
6. ? **Compatible con modelos pre-entrenados**

## Notas Importantes

- El epsilon de uso (`epsilonUso`) **siempre está activo**, incluso fuera del entrenamiento
- La estrategia híbrida es la más recomendada para uso general
- Entrenar con más episodios generalmente produce más variedad
- Los parámetros se guardan automáticamente cada 100 episodios
- El hash del sudoku se calcula basándose en la secuencia ordenada de valores

## Resolución de Problemas

### Si los sudokus siguen siendo similares:
1. Aumentar `epsilonUso`: `agente.SetEpsilonUso(0.25)`
2. Aumentar temperatura: `agente.SetTemperature(1.5)`
3. Cambiar a estrategia Softmax: `agente.Estrategia = Softmax`

### Si hay demasiado backtracking:
1. Reducir `epsilonUso`: `agente.SetEpsilonUso(0.1)`
2. Reducir temperatura: `agente.SetTemperature(0.5)`
3. Entrenar más episodios

### Para resetear completamente:
```csharp
agente.ResetearModelo();
// Eliminar archivo SudokuRLModel.json para empezar desde cero
```
