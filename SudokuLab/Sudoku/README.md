# Sistema Sudoku con Machine Learning

Sistema avanzado de generacion de Sudoku que utiliza **Reinforcement Learning (Q-Learning)** para optimizar la creacion de tableros y generar sudokus diversos.

## Inicio Rapido

```bash
# Ejecutar el programa
dotnet run

# En el menu, selecciona: 1 -> 1 (Quick Test)
```

## Caracteristicas Principales

- Reduccion de backtracking del 60-70%
- Generacion 30-40% mas rapida
- Sudokus unicos en cada ejecucion (95-100%)
- Tres estrategias de exploracion configurables
- Menu interactivo organizado en 6 categorias
- Modelo persistente que mejora con el uso

## Ejemplo Rapido

```csharp
// Generar un sudoku con ML
var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

Console.WriteLine($"Success: {sudoku.Exito}");
Console.WriteLine($"Errors: {sudoku.ConteoErrores}");
Console.WriteLine(sudoku.ResumenASCII);

// Cada sudoku sera diferente
Console.WriteLine($"Hash unico: {sudoku.HashSudoku}");
```

## Resultados (Sudoku 4x4)

| Metrica | Sin ML | Con ML | Mejora |
|---------|--------|--------|--------|
| Tasa de exito | 60-70% | 85-95% | +25-35% |
| Backtracking | 200-500 | 50-150 | -60-70% |
| Tiempo | 50-150ms | 30-80ms | -30-40% |
| Sudokus unicos | 100% | 95-98% | -2-5% |

## Estructura del Menu

El menu esta organizado en **6 categorias principales**:

```
1. ?? Quick Start          -> Tests y ejemplos rapidos
2. ?? Generate Sudoku      -> Crear puzzles con/sin ML
3. ???  Train Model          -> Entrenamiento en diferentes modos
4. ??  Configuration        -> Presets y ajustes personalizados
5. ?? Analysis & Tests      -> Comparativas y benchmarks
6. ?? View Statistics       -> Estado del modelo actual
```

**Para comenzar**: Selecciona opcion **1 -> 1** (Quick Test)

## Documentacion Completa

Toda la documentacion esta organizada en `SudokuML/MD/`:

### [INDICE.md](SudokuML/MD/INDICE.md) - Indice de Navegacion
**Punto de entrada a toda la documentacion**

### [README.md](SudokuML/MD/README.md) - Guia Principal
**Guia completa del sistema** (EMPEZAR AQUI)
- Inicio rapido con ejemplos
- Componentes del sistema explicados
- Uso desde codigo
- Configuracion de parametros
- Solucion de problemas
- FAQ completo

### [GUIA_RAPIDA.md](SudokuML/MD/GUIA_RAPIDA.md)
**Guia rapida de inicio**
- Menu simplificado explicado
- Uso inmediato con ejemplos
- Configuracion de variedad
- Tips y troubleshooting rapido

### [RESUMEN_PROYECTO.md](SudokuML/MD/RESUMEN_PROYECTO.md)
**Resumen consolidado del proyecto**
- Caracteristicas principales
- Mejoras implementadas
- Resultados comparativos
- Configuraciones disponibles
- Problemas comunes y soluciones

### [README_ML_VARIEDAD.md](SudokuML/MD/README_ML_VARIEDAD.md)
**Guia especializada en variedad**
- Configuracion avanzada de parametros
- Casos de uso especificos
- Tips y mejores practicas

### Documentacion Adicional

- **VISUALIZACION_MEJORAS.md**: Comparacion visual antes/despues
- **HISTORIAL_IMPLEMENTACION.md**: Historial de desarrollo
- **MEJORA_PATRON_ALEATORIO.md**: Documentacion de mejora especifica
- **EXPORTACION_PROYECTO.md**: Guia para exportar el proyecto

## Flujo de Lectura Recomendado

### Para Principiantes
1. Leer: [GUIA_RAPIDA.md](SudokuML/MD/GUIA_RAPIDA.md)
2. Ejecutar: `dotnet run` -> opcion `1 -> 1`
3. Leer: [README.md](SudokuML/MD/README.md) (completo)
4. Experimentar con el menu

### Para Desarrolladores
1. Leer: [README.md](SudokuML/MD/README.md) (completo)
2. Leer: [RESUMEN_PROYECTO.md](SudokuML/MD/RESUMEN_PROYECTO.md)
3. Leer: [README_ML_VARIEDAD.md](SudokuML/MD/README_ML_VARIEDAD.md)
4. Experimentar desde codigo

## Tecnologias

- **Lenguaje**: C# 12.0
- **Framework**: .NET 8
- **Algoritmo**: Q-Learning con aproximacion de funciones
- **Estrategias**: Epsilon-Greedy, Softmax, Hibrida

## Empezar

```bash
# 1. Clonar/descargar el proyecto
cd Sudoku

# 2. Ejecutar
dotnet run

# 3. Seleccionar opcion 1 -> 1 (Prueba Rapida)
```

---

**Para documentacion completa, consulta: [SudokuML/MD/](SudokuML/MD/)**
