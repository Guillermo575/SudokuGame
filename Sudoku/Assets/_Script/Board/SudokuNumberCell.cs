using TMPro;
using UnityEngine;
public class SudokuNumberCell : MonoBehaviour
{
    public TextMeshPro numberText;
    public int Id = 0;
    public bool Bloqueado = false;
    public Sudoku.SudokuGenerator.Celda objCelda { get; set; }

    public bool SetNumber(int number)
    {
        if (numberText != null)
        {
            numberText.text = Sudoku.Alphabet.getAlphaChar(number);
            objCelda.Valor = number;
            return true;
        }
        return false;
    }
}