# Guía de Exportación - Sistema Sudoku ML

## ?? Archivos a Incluir en el ZIP

Para usar este sistema en un proyecto nuevo, necesitas incluir estos archivos:

### Archivos Principales (Obligatorios):

```
SudokuML/
??? SudokuGenerator.cs          ? Generador principal
??? SudokuRLAgent.cs           ? Agente de Machine Learning
??? SudokuState.cs             ? Estado del juego
??? SudokuRewardSystem.cs      ? Sistema de recompensas
??? SudokuMLHelper.cs          ? Utilidades y comparaciones
??? Main.cs                    ? Punto de entrada fácil (opcional pero recomendado)
```

### Archivos de Documentación (Opcionales pero Recomendados):

```
SudokuML/
??? README.md                       ? Documentación técnica completa
??? GUIA_RAPIDA.md                 ? Guía de inicio rápido
??? HISTORIAL_IMPLEMENTACION.md    ? Historial de desarrollo
??? CAMBIO_NAMESPACE.md            ? Información sobre namespaces
??? EXPORTACION_PROYECTO.md        ? Este archivo
```

### Archivos de Ejemplo (Opcionales):

```
SudokuML/
??? EjemploUso.cs              ? Programa de ejemplo interactivo
```

---

## ?? Configuración en Proyecto Nuevo

### Paso 1: Crear el Proyecto

#### Para .NET Framework 4.8:
```bash
# Crear proyecto de consola
dotnet new console -n MiSudokuApp -f net48
cd MiSudokuApp
```

#### Para .NET 6/7/8:
```bash
# Crear proyecto de consola
dotnet new console -n MiSudokuApp
cd MiSudokuApp
```

### Paso 2: Extraer el ZIP

Extrae los archivos del ZIP en una carpeta dentro de tu proyecto:

```
MiSudokuApp/
??? Program.cs
??? MiSudokuApp.csproj
??? SudokuML/                  ? Extraer aquí
    ??? SudokuGenerator.cs
    ??? SudokuRLAgent.cs
    ??? SudokuState.cs
    ??? SudokuRewardSystem.cs
    ??? SudokuMLHelper.cs
    ??? Main.cs
    ??? (otros archivos)
```

### Paso 3: Instalar Dependencias

Solo necesitas **Newtonsoft.Json**:

```bash
dotnet add package Newtonsoft.Json
```

O edita el `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
</ItemGroup>
```

### Paso 4: Incluir Archivos en el Proyecto

#### Para .NET Core/5+/6+/7+/8+ (SDK-style):
Los archivos se incluyen automáticamente. ?

#### Para .NET Framework (estilo antiguo):
Edita el `.csproj`:

```xml
<ItemGroup>
  <Compile Include="SudokuML\SudokuGenerator.cs" />
  <Compile Include="SudokuML\SudokuRLAgent.cs" />
  <Compile Include="SudokuML\SudokuState.cs" />
  <Compile Include="SudokuML\SudokuRewardSystem.cs" />
  <Compile Include="SudokuML\SudokuMLHelper.cs" />
  <Compile Include="SudokuML\Main.cs" />
</ItemGroup>
```

### Paso 5: Crear Program.cs

```csharp
using System;
using Sudoku.SudokuML;

namespace MiSudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SUDOKU CON MACHINE LEARNING ===\n");
            
            // Opción 1: Usar la clase Main incluida
            Sudoku.SudokuML.Main.EjecutarTodo();
            
            // Opción 2: Usar directamente
            // var sudoku = new Sudoku.SudokuGenerator(3, 3, usarML: true);
            // Console.WriteLine($"Éxito: {sudoku.Exito}");
            
            Console.ReadKey();
        }
    }
}
```

### Paso 6: Compilar y Ejecutar

```bash
dotnet build
dotnet run
```

---

## ?? Métodos de la Clase Main

La clase `Main.cs` incluye estos métodos estáticos listos para usar:

### 1. Ejemplo Básico
```csharp
Sudoku.SudokuML.Main.EjemploBasico();
```
Genera un sudoku y muestra los resultados.

### 2. Entrenar Modelo
```csharp
Sudoku.SudokuML.Main.EntrenarModelo(episodios: 1000, columnasX: 3, columnasY: 3);
```
Entrena el agente de ML.

### 3. Comparar Rendimiento
```csharp
Sudoku.SudokuML.Main.CompararRendimiento(cantidad: 100, columnasX: 4, columnasY: 4);
```
Compara ML vs método tradicional.

### 4. Ejecutar Todo
```csharp
Sudoku.SudokuML.Main.EjecutarTodo();
```
Ejecuta el flujo completo: entrenar ? generar ? comparar.

---

## ?? Uso Directo (Sin Main)

Si prefieres no usar la clase `Main`, puedes usar directamente:

### Generar Sudoku
```csharp
using Sudoku;

var sudoku = new SudokuGenerator(3, 3, usarML: true, entrenar: false);

if (sudoku.Exito)
{
    Console.WriteLine($"? Sudoku generado");
    Console.WriteLine($"Backtracking: {sudoku.ConteoErrores}");
    Console.WriteLine($"Tiempo: {sudoku.TiempoEjecutado} ms");
    Console.WriteLine(sudoku.ResumenHTML);
}
```

### Entrenar el Agente
```csharp
using Sudoku;

SudokuGenerator.EntrenarAgente(1000, 3, 3);
Console.WriteLine(SudokuGenerator.ObtenerEstadisticasML());
```

### Comparar Rendimiento
```csharp
using Sudoku.SudokuML;

SudokuMLHelper.CompararRendimiento(100, 4, 4);
```

---

## ??? Estructura de Namespaces

```csharp
// Namespace principal
namespace Sudoku
{
    public class SudokuGenerator { }
}

// Namespace de ML
namespace Sudoku.SudokuML
{
    public class SudokuRLAgent { }
    public class SudokuState { }
    public class SudokuRewardSystem { }
    public class SudokuMLHelper { }
    public class Main { }
}
```

### Importar en tu código:

```csharp
using Sudoku;               // Para SudokuGenerator
using Sudoku.SudokuML;      // Para Main, SudokuMLHelper, etc.
```

---

## ?? Verificar que Todo Funciona

Después de importar, ejecuta estos tests:

### Test 1: Generar un Sudoku
```csharp
var sudoku = new Sudoku.SudokuGenerator(3, 3);
Console.WriteLine($"Éxito: {sudoku.Exito}");
```

### Test 2: Usar ML
```csharp
var sudokuML = new Sudoku.SudokuGenerator(3, 3, usarML: true);
Console.WriteLine($"Backtracking: {sudokuML.ConteoErrores}");
```

### Test 3: Entrenar
```csharp
Sudoku.SudokuGenerator.EntrenarAgente(10, 3, 3);
Console.WriteLine(Sudoku.SudokuGenerator.ObtenerEstadisticasML());
```

---

## ?? Troubleshooting

### Error: "No se encuentra el namespace Sudoku"
**Solución**: Asegúrate de que los archivos .cs están en la carpeta del proyecto y se compilan.

```bash
# Verificar que los archivos se incluyen
dotnet build -v detailed
```

### Error: "No se encuentra Newtonsoft.Json"
**Solución**: Instalar el paquete NuGet:

```bash
dotnet add package Newtonsoft.Json
```

### Error: "Archivo de origen no encontrado"
**Solución**: En .NET Framework, agregar explícitamente en el .csproj:

```xml
<Compile Include="SudokuML\*.cs" />
```

---

## ?? Documentación Incluida

Una vez importados, consulta:

1. **README.md** - Documentación técnica completa del sistema
2. **GUIA_RAPIDA.md** - Ejemplos y guía de uso rápido
3. **HISTORIAL_IMPLEMENTACION.md** - Cómo se desarrolló y por qué

---

## ?? Ejemplo de Program.cs Completo

```csharp
using System;
using Sudoku;
using Sudoku.SudokuML;

namespace MiSudokuApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("??????????????????????????????????????????");
            Console.WriteLine("?   GENERADOR DE SUDOKU CON ML           ?");
            Console.WriteLine("??????????????????????????????????????????\n");

            MostrarMenu();
        }

        static void MostrarMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- MENÚ ---");
                Console.WriteLine("1. Generar Sudoku 3x3 con ML");
                Console.WriteLine("2. Entrenar agente (1000 episodios)");
                Console.WriteLine("3. Comparar rendimiento");
                Console.WriteLine("4. Ejecutar todo");
                Console.WriteLine("5. Salir");
                Console.Write("\nOpción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Sudoku.SudokuML.Main.EjemploBasico();
                        break;
                    case "2":
                        Sudoku.SudokuML.Main.EntrenarModelo(1000, 3, 3);
                        break;
                    case "3":
                        Sudoku.SudokuML.Main.CompararRendimiento(50, 3, 3);
                        break;
                    case "4":
                        Sudoku.SudokuML.Main.EjecutarTodo();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
```

---

## ?? Checklist de Exportación

Antes de crear el ZIP, verifica:

- ? Todos los archivos .cs incluidos
- ? Documentación incluida (README, GUIA_RAPIDA, etc.)
- ? Main.cs para facilitar el uso
- ? Sin referencias a SistemaComercial (namespace limpio: Sudoku)
- ? Compatible con .NET Framework 4.8 y .NET 6+
- ? Solo depende de Newtonsoft.Json

---

## ?? Primeros Pasos Recomendados

Después de importar en un proyecto nuevo:

1. **Compilar**: `dotnet build`
2. **Entrenar**: `Sudoku.SudokuML.Main.EntrenarModelo(1000, 3, 3)`
3. **Generar**: `Sudoku.SudokuML.Main.EjemploBasico()`
4. **Comparar**: `Sudoku.SudokuML.Main.CompararRendimiento(100, 3, 3)`

---

## ?? Soporte

Consulta la documentación incluida:
- `README.md` - Documentación completa
- `GUIA_RAPIDA.md` - Ejemplos de uso
- `HISTORIAL_IMPLEMENTACION.md` - Contexto de desarrollo

---

**¡Tu sistema de Sudoku con ML está listo para exportar e importar en cualquier proyecto .NET! ??**
