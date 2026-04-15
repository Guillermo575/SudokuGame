using Sudoku;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Sudoku.SudokuGenerator;
[Serializable]
public class GameState
{
    public SudokuGenerator sudokuGenerator;
    public List<Celda> lstCeldas = new List<Celda>();
    public List<BitacoraMovimiento> lstBitacoraMovimiento = new List<BitacoraMovimiento>();
    public GameState Clone()
    {
        GameState clone = CloneShallow(this);
        clone.lstCeldas = new List<Celda>();
        foreach (var obj in this.lstCeldas)
        {
            clone.lstCeldas.Add(CloneShallow(obj));
        }
        clone.lstBitacoraMovimiento = new List<BitacoraMovimiento>();
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
    #region Bitacora
    [Serializable]
    public class BitacoraMovimiento
    {
        public int CuadranteX, CuadranteY, X, Y;
        public int Valor { get; set; }
    }
    #endregion
}