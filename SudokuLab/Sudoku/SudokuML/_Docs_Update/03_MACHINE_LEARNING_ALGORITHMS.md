# Algoritmos de Machine Learning

## ?? Ubicación
`SudokuML/MachineLearning/`

## ?? Componentes del Sistema ML

El sistema de ML implementa **Reinforcement Learning con Q-Learning** para optimizar la generación de Sudokus:

```
???????????????????????????????????????
?   SudokuRLAgent (Motor de RL)       ?
?  - Q-Learning                       ?
?  - 3 Estrategias de exploración     ?
?  - Persistencia de modelo           ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?   SudokuState (Estado del juego)    ?
?  - Características del tablero      ?
?  - Cálculo de potencial             ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?  SudokuRewardSystem (Recompensas)   ?
?  - Cálculo de rewards               ?
?  - Penalización de bloqueos         ?
???????????????????????????????????????
```

---

## ?? Q-Learning: Fundamentos

### Concepto Básico

**Q-Learning** enseña al agente a aprender qué acciones son mejores en cada situación mediante recompensas:

```
Estado Actual (S) ? Agente ? Acción (A)
     ?                        ?
    ¿Qué celdas    Selecciona la celda
    puedo llenar?  con mejor Q-Value
```

### Ecuación de Q-Learning

```
Q(s,a) = Q(s,a) + ? × [r + ? × max(Q(s',a')) - Q(s,a)]
         ?         ?   ?   ?                      ?
         ?         ?   ?   ?                      Q-Value actual
         ?         ?   ?   ?? Factor de descuento (0.95)
         ?         ?   ??????? Recompensa recibida
         ?         ??????????? Tasa de aprendizaje (0.1)
         ????????????????????? Q-Value nueva
```

### Parámetros

```csharp
private double learningRate = 0.1;          // ?: qué tan rápido aprende
private double discountFactor = 0.95;       // ?: importancia del futuro
private double epsilonEntrenamiento = 0.3;  // ?: exploración (entrenamiento)
private double epsilonUso = 0.15;           // ?: exploración (uso)
private double temperature = 1.0;           // ?: temperatura Softmax
```

---

## ?? Estrategias de Exploración

El agente usa **3 estrategias** para decidir qué celda seleccionar:

### 1. Epsilon-Greedy (?-Greedy)

**Concepto**: Con probabilidad ?, explorar aleatoriamente; de lo contrario, explotar el mejor Q-Value.

```
Si random() < epsilon:
    ? (exploración)
    Seleccionar celda aleatoria
Sino:
    ? (explotación)
    Seleccionar celda con mayor Q-Value
```

**Ventajas**:
- Simple de implementar
- Garantiza exploración
- Rápida convergencia

**Desventajas**:
- Cambio abrupto entre exploración y explotación
- Puede perder buenas acciones si epsilon es muy alto

**Código**:
```csharp
private SudokuGenerator.Celda SeleccionarEpsilonGreedy(
    List<SudokuGenerator.Celda> lstCeldas, 
    SudokuState estado, 
    double epsilon)
{
    if (rnd.NextDouble() < epsilon)
        return lstCeldas[rnd.Next(lstCeldas.Count)];  // Explorar
    else
        return ObtenerMejorCelda(lstCeldas, estado);  // Explotar
}
```

### 2. Softmax / Boltzmann

**Concepto**: Seleccionar celdas con probabilidad basada en sus Q-Values. Celdas con mejor Q-Value tienen mayor probabilidad, pero todas tienen oportunidad.

```
Probabilidad(celda) = e^(Q(celda)/?) / ?(e^(Q(otros)/?))
                      ?                    ?
                      Q-Value dividido     Suma de todas
                      por temperatura      probabilidades
```

**Ventajas**:
- Transición suave entre exploración y explotación
- Mayor variabilidad que ?-Greedy
- Mejor para generación diversa

**Desventajas**:
- Más costoso computacionalmente
- Sensible al parámetro de temperatura

**Código**:
```csharp
private SudokuGenerator.Celda SeleccionarSoftmax(
    List<SudokuGenerator.Celda> lstCeldas, 
    SudokuState estado)
{
    var qValues = lstCeldas.Select(c => ObtenerQValue(estado, c)).ToList();
    var probabilidades = CalcularProbabilidadesSoftmax(qValues, temperature);

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
```

### 3. Estrategia Híbrida (Recomendada)

**Concepto**: Combina ambas estrategias inteligentemente.

```
Durante entrenamiento:
    30% ?-Greedy + 70% Softmax ? Mayor exploración

Durante uso:
    50% ?-Greedy + 50% Softmax ? Balance variedad/rendimiento
```

**Ventajas**:
- Mejor exploración durante entrenamiento
- Mejor variabilidad durante uso normal
- Más estable que usar una sola estrategia

**Código**:
```csharp
if (entrenamiento || rnd.NextDouble() < 0.3)
    return SeleccionarSoftmax(lstCeldas, estado);
else
    return SeleccionarEpsilonGreedy(lstCeldas, estado, epsilonActual);
```

---

## ?? Ciclo de Aprendizaje (Entrenamiento)

### Por cada Sudoku Generado

```
1. INICIO DE EPISODIO
   ?? Crear estado inicial vacío
   ?? Inicializar recompensa en 0

2. GENERAR SUDOKU (por cada celda)
   ?? Obtener estado actual
   ?? Seleccionar celda con estrategia
   ?? Calcular recompensa
   ?? Actualizar Q-Value: Q(s,a) = Q(s,a) + ? × [r + ? × max(Q(s',a')) - Q(s,a)]
   ?? Guardar estado anterior
   ?? Avanzar al siguiente estado

3. FIN DE EPISODIO
   ?? Registrar recompensa total
   ?? Guardar hash del sudoku único
   ?? Decrementar epsilon (menos exploración)
   ?? Decrementar temperatura
   ?? Guardar modelo cada 100 episodios
```

### Ejemplo de Entrenamiento

```csharp
// Entrenar 1000 episodios
for (int episodio = 0; episodio < 1000; episodio++)
{
    // Crear sudoku en modo entrenamiento
    var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: true);

    if (sudoku.Exito)
    {
        // El agente registra automáticamente
        var agente = SudokuGenerator.agenteML;
        Console.WriteLine($"Episodio {episodio}: " +
                         $"Recompensa promedio: {agente.RecompensaPromedio:F2}, " +
                         $"Únicos: {agente.SudokusUnicos}");
    }
}

Console.WriteLine($"Entrenamiento completado:");
Console.WriteLine($"Total episodios: {SudokuGenerator.agenteML.EpisodiosEntrenados}");
Console.WriteLine($"Sudokus únicos generados: {SudokuGenerator.agenteML.SudokusUnicos}");
```

---

## ?? SudokuState: Gestión del Estado

### Propósito
Captura las características del tablero en un momento específico para Q-Learning.

### Características Clave

```csharp
public class SudokuState
{
    // Características del tablero
    public int CeldasLlenas { get; set; }           // Cuántas celdas llenadas
    public int CeldasVacias { get; set; }           // Cuántas quedan
    public int NivelDificultad { get; set; }       // Dificultad actual
    public int OpcionesPromedioCelda { get; set; } // Promedio de opciones
    public int ConflictosActuales { get; set; }    // Conflictos detectados

    // Métodos
    public List<double> ExtraerCaracteristicas(Celda celda)
    public double CalcularPotencial()
}
```

### Extracción de Características

```csharp
public List<double> ExtraerCaracteristicas(Celda celda)
{
    return new List<double>
    {
        CeldasLlenas / (double)SumaCeldas,      // Progreso (0.0-1.0)
        NivelDificultad / 10.0,                  // Dificultad normalizada
        OpcionesPromedioCelda / 9.0,             // Opciones disponibles
        ConflictosActuales / 10.0,               // Conflictos detectados
        CalcularDistanciaDesdeBloqueo(celda)    // Proximidad a bloques
    };
}
```

---

## ?? SudokuRewardSystem: Sistema de Recompensas

### Lógica de Recompensas

```
Si la celda se llena sin conflictos:
    ? Recompensa positiva: +1.0 a +10.0
      (Más alta si reduce muchas opciones futuras)

Si la celda causa conflictos:
    ? Recompensa negativa: -5.0 a -20.0
      (Más baja si causa muchos conflictos)

Si llevar la celda completa el sudoku:
    ?? Bonificación: +50.0
```

### Implementación

```csharp
public double CalcularRecompensa(
    SudokuState estadoAnterior,
    SudokuGenerator.Celda celdaLlenada,
    SudokuState estadoNuevo)
{
    double recompensa = 0;

    // Bonificación por llenar celda sin conflicto
    if (estadoNuevo.ConflictosActuales == 0)
        recompensa += 1.0;
    else
        recompensa -= 5.0 * estadoNuevo.ConflictosActuales;

    // Bonificación por reducir opciones (mejor estrategia)
    int reduccionOpciones = estadoAnterior.OpcionesPromedioCelda - 
                            estadoNuevo.OpcionesPromedioCelda;
    recompensa += reduccionOpciones * 0.1;

    // Bonificación si completa sudoku
    if (estadoNuevo.CeldasVacias == 0)
        recompensa += 50.0;

    return recompensa;
}
```

---

## ?? Parámetros Recomendados

### Para Máxima Variedad

```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonEntrenamiento(0.5);    // 50% exploración
agente.SetEpsilonUso(0.3);              // 30% en uso
agente.SetTemperature(2.0);             // Temperatura alta
agente.Estrategia = EstrategiaExploracion.Softmax;
```

**Resultado**: 95-100% sudokus únicos, pero más lento

### Para Balance (Recomendado)

```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonEntrenamiento(0.3);    // 30% exploración
agente.SetEpsilonUso(0.15);             // 15% en uso
agente.SetTemperature(0.8);             // Temperatura moderada
agente.Estrategia = EstrategiaExploracion.Hibrida;
```

**Resultado**: 85-95% únicos, rápido y estable

### Para Máximo Rendimiento

```csharp
var agente = new SudokuRLAgent();
agente.SetEpsilonEntrenamiento(0.1);    // 10% exploración
agente.SetEpsilonUso(0.05);             // 5% en uso
agente.SetTemperature(0.3);             // Temperatura baja
agente.Estrategia = EstrategiaExploracion.EpsilonGreedy;
```

**Resultado**: 40-60% únicos, muy rápido

---

## ?? Persistencia del Modelo

### Guardado Automático

El modelo se guarda en `SudokuRLModel.json` cada 100 episodios:

```json
{
  "QTable": {
    "estado_0_celda_1": 5.432,
    "estado_0_celda_2": 3.221,
    ...
  },
  "EpisodiosEntrenados": 1000,
  "RecompensaPromedio": 85.32,
  "EpsilonEntrenamiento": 0.25,
  "EpsilonUso": 0.12,
  "Temperature": 0.95,
  "SudokusUnicos": 987,
  "Estrategia": "Hibrida"
}
```

### Carga Automática

Al crear `SudokuRLAgent()`, automáticamente:
1. Busca `SudokuRLModel.json`
2. Si existe, carga Q-Table y parámetros
3. Si no existe, comienza con Q-Table vacía

---

## ?? Flujo de Decisión en Selección de Celda

```
???????????????????????????????????
?  SeleccionarCelda() llamado     ?
???????????????????????????????????
             ?
???????????????????????????????????
? ¿Una sola opción disponible?    ?
???????????????????????????????????
     ? Sí                     ? No
     ?                        ?
  Retornar                ????????????????????
                          ? Seleccionar      ?
                          ? estrategia según ?
                          ? modo actual      ?
                          ????????????????????
                                  ?
                   ???????????????????????????????
                   ?              ?              ?
            Epsilon-Greedy   Softmax        Híbrida
                  ?              ?              ?
            [Exploración]  [Probabilística] [Combinada]
                  ?              ?              ?
                  ???????????????????????????????
                                 ?
                          ???????????????????
                          ? Retornar celda  ?
                          ? seleccionada    ?
                          ???????????????????
```

---

## ?? Métricas de Monitoreo

Durante el entrenamiento, puedes monitorear:

```csharp
var agente = SudokuGenerator.agenteML;

Console.WriteLine($"Episodios entrenados: {agente.EpisodiosEntrenados}");
Console.WriteLine($"Recompensa promedio: {agente.RecompensaPromedio:F2}");
Console.WriteLine($"Sudokus únicos: {agente.SudokusUnicos}");
Console.WriteLine($"Estrategia activa: {agente.Estrategia}");
```

**Interpretación**:
- **Recompensa promedio**: Aumenta con entrenamiento (objetivo: >80)
- **Sudokus únicos**: Debe ser alto (objetivo: >90% del total)
- **Episodios**: Más episodios = mejor rendimiento
