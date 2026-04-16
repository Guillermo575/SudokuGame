using Sudoku;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
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
}