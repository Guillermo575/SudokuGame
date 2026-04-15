using Sudoku;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "SaveGameSO", menuName = "ScriptableObjects/Otros")]
public class SaveGameSO : UnityEngine.ScriptableObject
{
    public GameState gameState;
    public SaveGameSO Clone()
    {
        SaveGameSO clone = new SaveGameSO();
        clone.gameState = gameState.Clone();
        return clone;
    }
}