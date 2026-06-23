# SudokuGenerator - Núcleo del Sistema

## ?? Ubicación
`SudokuML/SudokuGenerator.cs`

## ?? Propósito

La clase `SudokuGenerator` es el núcleo del sistema de generación de Sudokus. Maneja:
- Creación de tableros válidos
- Integración con Machine Learning
- Generación de variabilidad
- Validación de soluciones

## ?? Parámetros de Constructor

```csharp
public SudokuGenerator(
    int ColumnasX = 3,      // Ancho del cuadrante
    int ColumnasY = 3,      // Alto del cuadrante
    bool usarML = true,     // Activar Machine Learning
    bool entrenar = false   // Modo entrenamiento
)
```

### Ejemplos

```csharp
// Sudoku 3x3 tradicional
var sudoku1 = new SudokuGenerator();

// Sudoku 3x3 con ML activado
var sudoku2 = new SudokuGenerator(3, 3, usarML: true);

// Sudoku 4x4 con ML en modo entrenamiento
var sudoku3 = new SudokuGenerator(4, 4, usarML: true, entrenar: true);

// Sudoku sin ML (método clásico)
var sudoku4 = new SudokuGenerator(3, 3, usarML: false);
```

## ?? Propiedades Principales

### Información del Tablero
- **`ColumnasX`, `ColumnasY`**: Dimensiones del cuadrante
- **`ValorFinal`**: Valor máximo (ColumnasX * ColumnasY)
- **`SumaCeldas`**: Total de celdas en el tablero
- **`lstCeldas`**: Lista de todas las celdas

### Estado de Generación
- **`Exito`**: ¿Se generó correctamente?
- **`Validado`**: ¿Pasó validación?
- **`TiempoEjecutado`**: Milisegundos empleados
- **`HashSudoku`**: Hash único del sudoku
- **`ConteoErrores`, `ConteoAciertos`**: Estadísticas de generación

### Configuración de ML
- **`UsarMachineLearning`**: ¿Usar ML?
- **`ModoEntrenamiento`**: ¿Entrenar modelo?
- **`agenteML`**: Agente RL estático compartido

## ?? Algoritmo de Generación

### Paso 1: Inicialización
```csharp
- Crear semilla aleatoria única (Guid.NewGuid().GetHashCode())
- Inicializar mapeo aleatorio de valores
- Crear lista de celdas vacías
- Si ML: inicializar estado
```

### Paso 2: Llenado de Tablero
```csharp
Para cada cuadrante (0 to ColumnasX*ColumnasY):
  Para cada valor a insertar (1 to ValorFinal):
    1. Obtener celdas válidas (respetan reglas Sudoku)
    2. Si hay celdas válidas:
       - Con ML: usar agente RL para seleccionar
       - Sin ML: seleccionar aleatoria
    3. Si no hay válidas: backtrack
    4. Actualizar estado y recompensas (si ML)
```

### Paso 3: Variabilidad Implementada

#### a) **Semilla Aleatoria Dinámica**
```csharp
rnd = new Random(Guid.NewGuid().GetHashCode());
```
Cada instancia obtiene una semilla única, eliminando predictibilidad.

#### b) **Mapeo Aleatorio de Valores**
```csharp
private Dictionary<int, int> mapeoValores;
void InicializarMapeoValores()
{
    mapeoValores = new Dictionary<int, int>();
    List<int> valores = new List<int>();
    for (int i = ValorInicial; i <= ValorFinal; i++)
        valores.Add(i);

    // Mezclar aleatoriamente
    for (int i = 0; i < ValorFinal; i++)
    {
        int idx = rnd.Next(i, ValorFinal);
        mapeoValores[i + 1] = valores[idx];
    }
}
```
Los valores se asignan en orden aleatorio, no secuencial.

#### c) **Selección Inteligente de Celdas (ML)**
El agente RL selecciona la celda con mayor probabilidad de éxito, reduciendo backtracking.

## ?? Métodos Principales

### Generación
```csharp
// Constructor (automático)
public SudokuGenerator(int ColumnasX, int ColumnasY, bool usarML, bool entrenar)

// Obtener representación
public String ResumenASCII { get; }      // Formato ASCII
public String ResumenHTML { get; }       // Formato HTML
```

### Validación
```csharp
// Validar celda individual
public bool ValidoSoloUno(Celda objCelda)

// Validar en eje X o Y
public bool ValidoEjeXY(Celda objCelda, int Valor)

// Validar todas las celdas
public static bool ValidarCeldas(List<Celda> lstCeldas)
```

### Métodos de Soporte
```csharp
// Obtener celdas válidas para un cuadrante
public List<Celda> GetCeldasValidas(int Cuadrante, int ValorSecuencial)

// Verificar si una celda está bloqueada
public bool GetBloqueo(Celda objCelda, int Valor)
```

## ?? Integración con Machine Learning

### Estado Inicial
```csharp
if (UsarMachineLearning)
{
    InicializarEstadoML();  // Crear estado inicial del juego
}
```

### Durante Generación
```csharp
// El agente RL selecciona la mejor celda
var celdaSeleccionada = agenteML.SeleccionarCelda(
    lstCeldas,           // Celdas disponibles
    estadoActual,        // Estado del juego
    ModoEntrenamiento    // ¿Entrenar?
);

// Actualizar Q-Learning después de la acción
agenteML.ActualizarQValue(
    estadoAnterior,
    celdaSeleccionada,
    recompensa,
    estadoNuevo,
    false
);
```

### Al Completar
```csharp
// Registrar episodio de entrenamiento
if (ModoEntrenamiento)
{
    agenteML.RegistrarEpisodio(recompensaTotal, HashSudoku);
}
```

## ?? Ejemplo Completo: Generar 10 Sudokus Únicos

```csharp
// Entrenar el modelo primero
SudokuGenerator.EntrenarAgente(episodios: 1000, columnasX: 3, columnasY: 3);

// Generar sudokus diversos
var sudokusUnicos = new HashSet<string>();
for (int i = 0; i < 10; i++)
{
    var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

    if (sudoku.Exito && !sudokusUnicos.Contains(sudoku.HashSudoku))
    {
        sudokusUnicos.Add(sudoku.HashSudoku);
        Console.WriteLine($"Sudoku {i+1}:");
        Console.WriteLine(sudoku.ResumenASCII);
        Console.WriteLine($"Tiempo: {sudoku.TiempoEjecutado}ms\n");
    }
}

Console.WriteLine($"Total únicos: {sudokusUnicos.Count}/10");
```

## ?? Flujo de Ejecución

```
???????????????????????????
?  Crear SudokuGenerator  ?
???????????????????????????
           ?
???????????????????????????
? Inicializar variables   ?
? - Semilla aleatoria     ?
? - Mapeo de valores      ?
? - Estado ML             ?
???????????????????????????
           ?
???????????????????????????
?  SetNewArray()          ?
?  (Crear estructura)     ?
???????????????????????????
           ?
???????????????????????????
?  SetDatos()             ?
?  (Llenar tablero)       ?
???????????????????????????
           ?
???????????????????????????
?  Validar Sudoku         ?
?  (Verificar reglas)     ?
???????????????????????????
           ?
???????????????????????????
?  Retornar resultado     ?
?  (Exito = true/false)   ?
???????????????????????????
```

## ? Validación

Un Sudoku es válido cuando:
1. **Cuadrantes**: Cada valor 1-N aparece exactamente una vez
2. **Filas**: Cada valor 1-N aparece exactamente una vez
3. **Columnas**: Cada valor 1-N aparece exactamente una vez
4. **Completitud**: Todas las celdas tienen valor

## ?? Notas Técnicas

- **Thread-safe**: Cada instancia mantiene su propio estado
- **Semilla única**: Garantiza variabilidad entre instancias
- **Soporte multi-tamaño**: Funciona con 3x3 y 4x4 (extensible)
- **Persistencia**: Los datos del agente se guardan automáticamente
