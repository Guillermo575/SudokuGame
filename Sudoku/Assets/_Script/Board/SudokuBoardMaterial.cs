using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "SudokuBoardMaterial", menuName = "ScriptableObjects/SudokuBoardMaterial")]
public class SudokuBoardMaterial : UnityEngine.ScriptableObject
{
    public Material Board;
    public Material SubBoard;
    public Material CellNormal;
    public Material CellDisabled;
    public Material CellSelected;
}