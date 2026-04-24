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
    public String Id;
    public String dateUpdate;
    public SudokuGenerator sudokuGenerator;
    public List<Celda> lstCeldas = new List<Celda>();
    public List<LogMove> lstBitacoraMovimiento = new List<LogMove>();
    public int LogIndex = 0;
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