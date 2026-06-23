# Guía del Sistema de Menús

## ?? Ubicación
`SudokuML/Tools/MainMenu.cs`

## ?? Estructura General

El sistema de menús proporciona una interfaz interactiva para todas las funcionalidades del proyecto. Se organiza en **6 categorías principales** con submenús especializados.

```
???????????????????????????????????????
?       MAIN MENU (Menú Principal)    ?
???????????????????????????????????????
? 1. ?? Quick Start                   ?
? 2. ?? Generate Sudoku               ?
? 3. ???  Train Model                  ?
? 4. ??  Configuration                ?
? 5. ?? Analysis & Tests              ?
? 6. ?? View Statistics               ?
? 0. ?? Exit                          ?
???????????????????????????????????????
```

---

## 1?? QUICK START (Inicio Rápido)

### Propósito
Realizar pruebas rápidas sin configuración compleja.

### Opciones

#### 1.1 ? Quick Test (Recomendado)
**Qué hace**: Prueba completa y automatizada del sistema

```
Flujo:
?? Entrena el agente con 500 episodios
?? Genera 10 sudokus con ML
?? Verifica que sean únicos
?? Compara con método tradicional
?? Muestra estadísticas completas
?? Calcula mejoras en rendimiento
```

**Tiempo**: ~30-60 segundos

**Salida**: 
```
Quick Test Results:
??????????????????
With ML Training (500 episodes):
?? Average time: 280ms
?? Average backtracking: 95
?? Unique sudokus: 10/10 (100%)
?? Success rate: 100%
?? Avg reward: 85.32

Traditional Method:
?? Average time: 450ms
?? Average backtracking: 340
?? Unique sudokus: 4/10 (40%)
?? Success rate: 100%

Improvement:
?? Speed: +37.7%
?? Backtracking: -72%
?? Uniqueness: +150%
```

**Uso**: Ideal para verificar que todo funciona correctamente.

#### 1.2 ?? Basic Usage Example
**Qué hace**: Demostración simple de uso en código

```csharp
// Sin ML
Console.WriteLine("Traditional method:");
var sudoku1 = new SudokuGenerator(3, 3, usarML: false);
Console.WriteLine(sudoku1.ResumenASCII);

// Con ML
Console.WriteLine("With Machine Learning:");
var sudoku2 = new SudokuGenerator(3, 3, usarML: true);
Console.WriteLine(sudoku2.ResumenASCII);
```

**Uso**: Aprender la sintaxis básica.

#### 1.3 ?? Before/After Comparison
**Qué hace**: Comparación visual del impacto de las mejoras

```
Muestra lado a lado:
?? Método tradicional (columna izquierda)
?? Método con ML (columna derecha)
?? Diferencia de tiempo
?? Diferencia de backtracking
?? Mejora porcentual
```

**Uso**: Para presentaciones o documentación.

---

## 2?? GENERATE SUDOKU (Generar Sudoku)

### Propósito
Generar sudokus individuales con diferentes configuraciones.

### Opciones

#### 2.1 ?? Generate with Machine Learning
**Qué hace**: Genera un sudoku usando el modelo entrenado

```
Pasos:
1. Carga el agente ML entrenado
2. Genera un sudoku seleccionando celdas inteligentemente
3. Muestra el resultado en ASCII
4. Reporta estadísticas
```

**Código interno**:
```csharp
var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);
Console.WriteLine($"Tiempo: {sudoku.TiempoEjecutado}ms");
Console.WriteLine(sudoku.ResumenASCII);
```

**Opciones interactivas**:
- Elegir tamaño: 3x3 o 4x4
- Ver detalles adicionales

#### 2.2 ?? Generate without ML (Traditional)
**Qué hace**: Genera un sudoku con el método clásico

```
Características:
- Selección completamente aleatoria
- Sin aprendizaje previo
- Más lento pero determinista
```

**Uso**: Comparación o si prefieres método tradicional.

#### 2.3 ?? Demonstrate Variety (Demonstrar Variedad)
**Qué hace**: Genera múltiples sudokus y verifica unicidad

```
Flujo:
1. Solicita cantidad (default: 10)
2. Genera N sudokus con ML
3. Compara hashes para detectar duplicados
4. Muestra porcentaje de únicos
5. Reporta estadísticas

Salida:
Generating 10 Sudokus with ML...
1. ? Unique - Time: 285ms
2. ? Unique - Time: 312ms
3. ? Unique - Time: 298ms
...
10. ? Unique - Time: 305ms

Results: 10/10 unique (100%)
Average time: 299.5ms
```

**Uso**: Verificar que el sistema genera diversidad.

---

## 3?? TRAIN MODEL (Entrenar Modelo)

### Propósito
Mejorar el agente de ML mediante entrenamiento.

### Opciones

#### 3.1 ? Fast Training
**Episodios**: 100

```
Ideal para: Pruebas rápidas
Tiempo: ~1-2 minutos
Resultado: Mejora notoria
```

#### 3.2 ? Complete Training (Recomendado)
**Episodios**: 1000

```
Ideal para: Uso en producción
Tiempo: ~10-15 minutos
Resultado: Excelente rendimiento
```

#### 3.3 ?? For 4x4 Sudokus
**Episodios**: 2000

```
Ideal para: Sudokus complejos 4x4
Tiempo: ~20-30 minutos
Resultado: Muy optimizado para 4x4
```

#### 3.4 ?? Custom Training
**Qué hace**: Entrenar con número personalizado de episodios

```
Entrada del usuario:
Ingrese número de episodios: 500

Proceso:
?? Cada 10 episodios: muestra progreso
?? Monitorea recompensa promedio
?? Registra sudokus únicos
?? Guarda modelo cada 100 episodios

Salida:
Training progress:
?? Episode 10: Avg reward 45.23, Unique: 8
?? Episode 20: Avg reward 62.15, Unique: 18
?? Episode 30: Avg reward 71.89, Unique: 29
...
```

#### 3.5 ?? Monitor Variety in Real-time
**Qué hace**: Entrena mientras muestra gráficos de diversidad

```
Monitorea:
?? Recompensa promedio (debe ?)
?? Sudokus únicos generados (debe ?)
?? Episodios completados (debe ?)
?? Velocidad promedio (debe ?)

Actualización cada 10 episodios
Útil para ver convergencia en tiempo real
```

---

## 4?? CONFIGURATION (Configuración)

### Propósito
Ajustar parámetros del sistema ML.

### Opciones

#### 4.1 ?? Available Presets (Presets Disponibles)

**Maximum Variety** (Máxima Variedad)
```
Para obtener más sudokus diferentes
?? Epsilon Training: 0.5 (50% exploración)
?? Epsilon Usage: 0.3 (30% exploración)
?? Temperature: 2.0 (muy exploratoria)
?? Strategy: Softmax

Resultado: 95-100% únicos, pero más lento
```

**Balance** (Equilibrio - Recomendado) ?
```
Balance perfecto entre rendimiento y variedad
?? Epsilon Training: 0.3 (30% exploración)
?? Epsilon Usage: 0.15 (15% exploración)
?? Temperature: 0.8 (moderada)
?? Strategy: Hybrid

Resultado: 85-95% únicos, rápido y estable
```

**Maximum Performance** (Máximo Rendimiento)
```
Para máxima velocidad
?? Epsilon Training: 0.1 (10% exploración)
?? Epsilon Usage: 0.05 (5% exploración)
?? Temperature: 0.3 (muy explotadora)
?? Strategy: Epsilon-Greedy

Resultado: 40-60% únicos, muy rápido
```

#### 4.2 ?? Exploration Strategies (Estrategias de Exploración)

**Epsilon-Greedy**
```
Acceso rápido a: agente.Estrategia = EstrategiaExploracion.EpsilonGreedy
?? Con probabilidad ?: selecciona aleatoria
?? Con probabilidad 1-?: selecciona mejor Q-Value
?? Simple pero efectivo
```

**Softmax**
```
Acceso rápido a: agente.Estrategia = EstrategiaExploracion.Softmax
?? Probabilidad basada en Q-Values
?? Mayor variabilidad que Epsilon-Greedy
?? Mejor para diversidad
```

**Hybrid** (Recomendado)
```
Acceso rápido a: agente.Estrategia = EstrategiaExploracion.Hibrida
?? Combina ambas estrategias
?? Más exploración en entrenamiento
?? Mejor balance en uso
```

#### 4.3 ?? Custom Configuration (Configuración Personalizada)

**Entrada interactiva**:
```
?? Learning Rate: 0.1 (0.001-1.0) - Velocidad de aprendizaje
?? Discount Factor: 0.95 (0.0-1.0) - Importancia del futuro
?? Epsilon Training: 0.3 (0.0-1.0) - Exploración en entrenamiento
?? Epsilon Usage: 0.15 (0.0-1.0) - Exploración en uso
?? Temperature: 0.8 (0.1-5.0) - Temperatura Softmax
?? Strategy: Hybrid - Estrategia de exploración
```

**Aplicación**:
```csharp
var agente = SudokuGenerator.agenteML;
agente.SetLearningRate(0.15);
agente.SetEpsilonEntrenamiento(0.25);
agente.SetTemperature(1.2);
agente.Estrategia = EstrategiaExploracion.Softmax;
```

#### 4.4 ?? View Current Parameters (Ver Parámetros Actuales)

**Muestra**:
```
Current Configuration:
?? Learning Rate: 0.1
?? Discount Factor: 0.95
?? Epsilon Training: 0.3
?? Epsilon Usage: 0.15
?? Temperature: 1.0
?? Active Strategy: Hybrid
?? Episodes Trained: 1000
?? Unique Sudokus: 987
?? Average Reward: 85.32
```

---

## 5?? ANALYSIS & TESTS (Análisis y Tests)

### Propósito
Realizar análisis comparativos detallados.

### Opciones

#### 5.1 ?? Compare Performance (Comparar Rendimiento)

**Compara**:
```
?? Velocidad: Con ML vs Sin ML
?? Backtracking: Con ML vs Sin ML
?? Tasa de éxito: Con ML vs Sin ML
?? Promedio tiempo: Con ML vs Sin ML
?? Promedio errores: Con ML vs Sin ML

Ejecuta: 30 sudokus de cada tipo

Salida:
?????????????????????????????????????
              Sin ML    Con ML    Mejora
?????????????????????????????????????
Tiempo (ms)    450      280       +37.7%
Backtrack      340      95        -72.0%
Éxito (%)      100      100       0.0%
Errores Prom.  12.5     3.2       -74.4%
?????????????????????????????????????
```

#### 5.2 ?? Compare Strategies (Comparar Estrategias)

**Compara las 3 estrategias**:
```
?? Epsilon-Greedy vs Softmax vs Hybrid
?? Mide: Velocidad, Variedad, Éxito
?? Ejecuta: 20 sudokus cada una

Salida:
Strategy         Speed(ms)  Unique   Success
?????????????????????????????????????????
Epsilon-Greedy     280      8/20     100%
Softmax            310      18/20    100%
Hybrid             295      19/20    100%
?????????????????????????????????????
```

#### 5.3 ?? Massive Testing (Testing Masivo)

**Test completo**:
```
?? Genera 50 sudokus con ML
?? Calcula todas las estadísticas
?? Reporta distribución de tiempos
?? Identifica outliers
?? Guarda resultados

Salida:
Massive Testing (50 sudokus):
?? Total exitosos: 50/50 (100%)
?? Tiempo promedio: 293ms
?? Desviación estándar: 28ms
?? Mínimo: 245ms
?? Máximo: 362ms
?? Sudokus únicos: 49/50 (98%)
?? Recompensa promedio: 85.67
?? Backtracking promedio: 87
```

---

## 6?? VIEW STATISTICS (Ver Estadísticas)

### Propósito
Mostrar estado actual del modelo

### Información Mostrada

```
ML Agent Statistics:
???????????????????????????????????????

Training Progress:
?? Episodes trained: 1000
?? Average reward: 85.32
?? Unique sudokus: 987
?? Model saved: Yes

Current Configuration:
?? Active strategy: Hybrid
?? Epsilon (training): 0.25
?? Epsilon (usage): 0.14
?? Temperature: 0.95
?? Q-Table size: 4567

Performance Metrics:
?? Average generation time: 289ms
?? Average backtracking: 95
?? Success rate: 100%
?? Unique rate: 98.7%

Memory:
?? Model file size: 2.3 MB
?? Q-Table entries: 4567
```

---

## ?? Flujo de Uso Recomendado

### Primer uso (Setup inicial)
```
1. Main Menu ? 1 (Quick Start) ? 1 (Quick Test)
   ?? Valida que todo funcione

2. Main Menu ? 3 (Train) ? 2 (Complete Training)
   ?? Entrena el agente (10-15 min)

3. Main Menu ? 1 (Quick Start) ? 1 (Quick Test)
   ?? Verifica mejoras
```

### Uso normal (Generación)
```
Main Menu ? 2 (Generate Sudoku) ? 1 (With ML)
?? Genera sudoku optimizado
```

### Exploración avanzada
```
1. Main Menu ? 5 (Analysis) ? 1 (Compare Performance)
   ?? Valida rendimiento

2. Main Menu ? 4 (Configuration) ? Ajustar parámetros
   ?? Optimiza para tu caso

3. Main Menu ? 5 (Analysis) ? 3 (Massive Testing)
   ?? Test de estabilidad
```

---

## ?? Tips y Mejores Prácticas

### Para Máxima Variedad
```
1. Configuration ? Maximum Variety preset
2. Train ? Custom ? 2000 episodes
3. Generate ? Demonstrate Variety
```

### Para Máximo Rendimiento
```
1. Configuration ? Maximum Performance preset
2. Train ? For 4x4 Sudokus ? 2000 episodes
3. Generate ? Generate with ML
```

### Para Producción
```
1. Configuration ? Balance preset (default)
2. Train ? Complete Training ? 1000 episodes
3. Salvar modelo: automático cada 100 episodios
```

---

## ?? Troubleshooting

| Problema | Solución |
|----------|----------|
| Generación lenta | ?? ? Maximum Performance preset |
| Pocos sudokus únicos | ?? ? Maximum Variety preset |
| Muchas fallos | 3?? Train ? Complete Training |
| Modelo corrupto | Eliminar `SudokuRLModel.json` + reentrenar |
