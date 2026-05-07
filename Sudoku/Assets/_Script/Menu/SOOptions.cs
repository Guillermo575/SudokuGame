using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[CreateAssetMenu(fileName = "SudokuBoardMaterial", menuName = "ScriptableObjects/Options")]
public class SOOptions : UnityEngine.ScriptableObject
{
    public Color colorBackground = Color.black;
    public Color colorBoard = Color.black;
}