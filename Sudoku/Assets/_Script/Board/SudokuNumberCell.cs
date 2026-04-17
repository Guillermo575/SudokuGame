using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class SudokuNumberCell : MonoBehaviour
{
    public TextMeshPro numberText;
    public int Id = 0;
    public bool Bloqueado = false;
    GameManager gameManager;
    public void Initialize()
    {
        Renderer renderer = GetComponent<Renderer>();
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
            Material material;
            Bloqueado = objCelda.First().bloqueado;
            if (objCelda.First().bloqueado)
            {
                material = gameManager.sudokuBoardMaterial.CellDisabled;
                SetNumber(objCelda.First().Valor);
            }
            else
            {
                //SetNumber(0);
                material = gameManager.sudokuBoardMaterial.CellNormal;
                SetNumber(objCelda.First().Valor);
            }
            if (renderer != null)
            {
                renderer.material = material;
            }
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