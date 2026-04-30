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
    #region Variables
    public GameState lastGameState;
    public List<GameState> lstGames;
    #endregion

    #region Save
    public void SaveGame(GameState gameState)
    {
        if (lstGames.Count == 0)
        {
            lstGames = new List<GameState>();
            lstGames.Add(gameState);
            return;
        }
        int index = lstGames.FindIndex(p => p.Id == gameState.Id);
        if (index >= 0)
        {
            lstGames[index] = gameState;
        }
        else
        {
            lstGames.Add(gameState);
        }
    }
    #endregion
}