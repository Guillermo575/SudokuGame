using UnityEngine;
using TMPro;
using System.Linq;
using static Sudoku.SudokuGenerator;
public class SudokuNumberCell : MonoBehaviour
{
    public GameObject sudokuNumberCell;
    public TextMeshPro numberText;
    public int Id = 0;
    public bool Bloqueado = false;
    public Celda objCelda {  get; private set; }
    SudokuBoard sudokuBoard;
    public void Initialize(SudokuBoard sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        if (sudokuNumberCell != null)
        {
            InitializeCell();
        }
    }
    public void InitializeCell()
    {
        Renderer renderer = GetComponent<Renderer>();
        Id = Id == 0 ? sudokuBoard.addLoopId() : Id;
        if (numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshPro>();
            if (numberText == null)
            {
                Debug.LogError("Falta un componente TextMeshPro en SudokuNumberCell");
            }
        }
        var lstCelda = (from x in sudokuBoard.GetLstCeldas() where x.Id == Id select x).ToList();
        if (lstCelda.Count > 0)
        {
            objCelda = lstCelda.First();
            Material material;
            Bloqueado = objCelda.bloqueado;
            material = objCelda.bloqueado ? sudokuBoard.sudokuBoardMaterial.CellDisabled : sudokuBoard.sudokuBoardMaterial.CellNormal;
            SetNumber(objCelda.Valor);
            if (renderer != null)
            {
                renderer.material = material;
            }
        }
    }
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