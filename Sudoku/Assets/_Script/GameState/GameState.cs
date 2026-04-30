using Sudoku;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using static Sudoku.SudokuGenerator;
[Serializable]
public class GameState
{
    #region Public
    public String Id;
    public String dateUpdate;
    public SudokuGenerator sudokuGenerator;
    public List<Celda> lstCeldas = new List<Celda>();
    public List<LogMove> lstBitacoraMovimiento = new List<LogMove>();
    public int LogIndex = 0;
    #endregion

    #region General
    public static GameState CreateGame(int numberColumns, int numberRows, int cicloMin = 2, int cicloMax = 4)
    {
        var sudokuGenerator = new SudokuGenerator(numberColumns, numberRows);
        BlockCells(sudokuGenerator.lstCeldas, cicloMin, cicloMax);
        var gameState = new GameState();
        gameState.Id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        gameState.dateUpdate = gameState.Id;
        gameState.sudokuGenerator = sudokuGenerator;
        var lstCeldas = new List<Celda>();
        foreach (var obj in sudokuGenerator.lstCeldas)
        {
            lstCeldas.Add
            (
                new Celda
                {
                    Id = obj.Id,
                    IdCuadrante = obj.IdCuadrante,
                    CuadranteEjeX = obj.CuadranteEjeX,
                    CuadranteEjeY = obj.CuadranteEjeY,
                    EjeX = obj.EjeX,
                    EjeY = obj.EjeY,
                    bloqueado = obj.bloqueado,
                    Valor = obj.bloqueado ? obj.Valor : 0,
                }
            );
        }
        gameState.lstCeldas = lstCeldas;
        return gameState;
    }
    public static void BlockCells(List<SudokuGenerator.Celda> lstCelda, int cicloMin = 2, int cicloMax = 4)
    {
        foreach (var obj in lstCelda)
        {
            obj.bloqueado = true;
        }
        System.Random rnd = new System.Random();
        int ciclo = rnd.Next(cicloMin, cicloMax);
        for (int i = 0; i < lstCelda.Count; i += ciclo)
        {
            ciclo = rnd.Next(cicloMin, cicloMax);
            int inicio = i;
            int fin = Math.Min(i + ciclo - 1, lstCelda.Count - 1);
            int indiceAleatorio = rnd.Next(inicio, fin + 1);
            lstCelda[indiceAleatorio].bloqueado = false;
        }
    }
    #endregion

    #region Clone
    public GameState Clone()
    {
        GameState clone = CloneShallow(this);
        clone.lstCeldas = new List<Celda>();
        foreach (var obj in this.lstCeldas)
        {
            clone.lstCeldas.Add(CloneShallow(obj));
        }
        clone.lstBitacoraMovimiento = new List<LogMove>();
        foreach (var obj in this.lstBitacoraMovimiento)
        {
            clone.lstBitacoraMovimiento.Add(CloneShallow(obj));
        }
        return clone;
    }
    public T CloneShallow<T>(T original) where T : class, new()
    {
        if (original == null) return null;
        T clone = new T();
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            Type fieldType = field.FieldType;
            if (typeof(ICollection).IsAssignableFrom(fieldType) || fieldType.IsArray) continue;
            if (fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(List<>) || fieldType.GetGenericTypeDefinition() == typeof(IList<>))) continue;
            field.SetValue(clone, field.GetValue(original));
        }
        return clone;
    }
    #endregion

    #region Bitacora
    [Serializable]
    public class LogMove
    {
        public int Id, Valor, ValorAntes;
    }
    public void LogAdd(int Id, int Valor, int ValorAntes)
    {
        var obj = (from x in lstCeldas where x.Id == Id select x).ToList();
        if (obj.Count == 0) return;
        lstBitacoraMovimiento = lstBitacoraMovimiento.Take(LogIndex).ToList();
        lstBitacoraMovimiento.Add(new LogMove { Id = Id, Valor = Valor, ValorAntes = ValorAntes });
        LogIndex = lstBitacoraMovimiento.Count;
    }
    public LogMove LogBack()
    {
        if (LogIndex < 0) return null;
        LogIndex--;
        return (LogIndex < 0) ? null : lstBitacoraMovimiento[LogIndex];
    }
    public LogMove LogForward()
    {
        if (LogIndex >= lstBitacoraMovimiento.Count) return null;
        LogIndex = LogIndex < 0 ? 0 : LogIndex;
        var obj = lstBitacoraMovimiento[LogIndex];
        LogIndex++;
        return obj;
    }
    #endregion
}