using UnityEngine;
using TMPro;
public class SudokuNumberNode : MonoBehaviour
{
    public TextMeshPro numberText;
    public Sudoku.SudokuGenerator.Celda celda;
    public void Initialize()
    {
        if (numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshPro>();
            if (numberText == null)
            {
                Debug.LogError("Falta un componente TextMeshPro en SudokuNumberNode");
            }
        }
        SetNumber(0);
    }
    public void SetNumber(int number)
    {
        if (numberText != null)
        {
            numberText.text = number > 0 ? number.ToString() : "-";
        }
    }
}