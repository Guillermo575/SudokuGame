using Sudoku;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using static Sudoku.SudokuGenerator;
[Serializable]
[CreateAssetMenu(fileName = "SaveGameSO", menuName = "ScriptableObjects/SaveGameSO")]
public class SaveGameSO : UnityEngine.ScriptableObject
{
    public GameState lastGameState;
    public List<GameState> lstGames;
    public SaveGameSO Clone()
    {
        SaveGameSO clone = new SaveGameSO();
        clone.lastGameState = lastGameState.Clone();
        return clone;
    }
    #region CreateGame
    public void CreateGame(int numberColumns, int numberRows)
    {
        var sudokuGenerator = new SudokuGenerator(numberColumns, numberRows);
        BlockCells(sudokuGenerator.lstCeldas);
        lastGameState = new GameState();
        lastGameState.dateCreation = DateTime.Now;
        lastGameState.Id = lastGameState.dateCreation.ToString("yyyyMMddHHmmssfff");
        lastGameState.dateUpdate = DateTime.Now;
        lastGameState.sudokuGenerator = sudokuGenerator;
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
        lastGameState.lstCeldas = lstCeldas;
    }
    public void BlockCells(List<SudokuGenerator.Celda> lstCelda, int cicloMin = 2, int cicloMax = 4)
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
}