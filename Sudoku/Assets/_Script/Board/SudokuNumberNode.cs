using UnityEngine;
using TMPro;
public class SudokuNumberNode : MonoBehaviour
{
    public SudokuNumberCell sudokuNumberCell;
    public void Initialize()
    {
        if (sudokuNumberCell != null)
        {
            sudokuNumberCell.Initialize();
        }
    }
}