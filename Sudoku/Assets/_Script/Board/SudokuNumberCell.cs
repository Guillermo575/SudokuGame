using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class SudokuNumberCell : MonoBehaviour
{
    public TextMeshPro numberText;
    public int Id = 0;
    GameManager gameManager;
    public void Initialize()
    {
        gameManager = GameManager.GetSingleton();
        Id = Id == 0 ? gameManager.addLoopId() : Id;
        if (numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshPro>();
            if (numberText == null)
            {
                Debug.LogError("Falta un componente TextMeshPro en SudokuNumberNode");
            }
        }
        var objCelda = (from x in gameManager.sudokuGenerator.lstCeldas where x.Id == Id select x).ToList();
        if (objCelda.Count > 0)
        {
            //if (objCelda.First().bloqueado)
            //{
            //    SetNumber(objCelda.First().Valor);
            //}
            //else
            //{
            //    SetNumber(0);
            //}
            SetNumber(objCelda.First().Valor);
        }
    }
    public void SetNumber(int number)
    {
        if (numberText != null)
        {
            numberText.text = Sudoku.Alphabet.getAlphaChar(number);
        }
    }
}