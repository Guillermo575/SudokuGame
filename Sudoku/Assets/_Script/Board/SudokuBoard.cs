using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class SudokuBoard : MonoBehaviour
{
    #region Prefabs
    [Header("Prefabs")]
    public GameObject mainBoardPrefab;
    public GameObject subBoardPrefab;
    public GameObject numberNodePrefab;
    public SudokuBoardMaterial sudokuBoardMaterial;
    #endregion

    #region Public
    [Header("Transformation")]
    public float spaceBetweenSubBoards = 0.05f;
    public float spaceBetweenNodes = 0.025f;
    public float sizeNumberNode = 1f;
    #endregion

    #region Private
    public List<SudokuNumberCell> allCells { get; private set; }
    public int numberColumns { get; private set; } = 3;
    public int numberRows { get; private set; } = 3;
    public int LoopId { get; private set; } = 1;
    public GameState gameState { get; private set; }
    public bool IsGameActive { get; private set; }
    private SudokuSubBoard[,] subBoards;
    #endregion

    #region Update
    private void Update()
    {
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        if (gameState == null) return;
        var SizeNumber = sizeNumberNode + spaceBetweenNodes;
        var SizeBoard = numberColumns * numberRows * SizeNumber + (spaceBetweenSubBoards * 4);
        var SizeNode = sizeNumberNode + spaceBetweenNodes;
        mainBoardPrefab.transform.localScale = new Vector3(SizeBoard, 1f, SizeBoard);
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                SudokuSubBoard subBoardObj = subBoards[j, i];
                Vector3 position = new Vector3(spaceBetweenSubBoards + (j * ((SizeNumber * numberRows) + spaceBetweenSubBoards)), 0.2f, -spaceBetweenSubBoards + (-i * ((SizeNumber * numberColumns) + spaceBetweenSubBoards)));
                subBoardObj.gameObject.transform.localPosition = position;
                subBoardObj.mainBoardPrefab.transform.localScale = new Vector3(SizeNode * numberRows + spaceBetweenNodes, 1f, SizeNode * numberColumns + spaceBetweenNodes);
                for (int l = 0; l < numberColumns; l++)
                {
                    for (int m = 0; m < numberRows; m++)
                    {
                        Vector3 position2 = new Vector3(spaceBetweenNodes + (m * SizeNode), 0.4f, -spaceBetweenNodes + (-l * SizeNode));
                        subBoardObj.nodes[m, l].gameObject.transform.localPosition = position2;
                    }
                }
            }
        }
    }
    #endregion

    #region Create
    public void CreateBoard(GameState gameState)
    {
        DestroyBoard();
        numberColumns = gameState.sudokuGenerator.ColumnasY;
        numberRows = gameState.sudokuGenerator.ColumnasX;
        this.gameState = gameState;
        subBoards = new SudokuSubBoard[numberColumns, numberRows];
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                subBoards[j, i] = CreateSubBoard(i, j);
            }
        }
        IsGameActive = true;
        UpdatePosition();
    }
    public SudokuSubBoard CreateSubBoard(int row, int col)
    {
        GameObject subBoardObj = Instantiate(subBoardPrefab, Vector3.zero, Quaternion.identity, this.transform);
        subBoardObj.name = $"SubBoard_{col}_{row}";
        SudokuSubBoard subBoard = subBoardObj.GetComponent<SudokuSubBoard>();
        subBoard = subBoard == null ? subBoardObj.AddComponent<SudokuSubBoard>() : subBoard;
        subBoard.nodes = new SudokuNumberCell[numberRows, numberColumns];
        for (int i = 0; i < numberColumns; i++)
        {
            for (int j = 0; j < numberRows; j++)
            {
                subBoard.nodes[j, i] = CreateNumberCell(subBoard.transform, j, i);
                allCells.Add(subBoard.nodes[j, i]);
            }
        }
        return subBoard;
    }
    public SudokuNumberCell CreateNumberCell(Transform transform,int row, int col)
    {
        GameObject NumberNode = Instantiate(numberNodePrefab, Vector3.zero, Quaternion.identity, transform);
        NumberNode.name = $"Node_{col}_{row}";
        SudokuNumberCell subBoardCell = NumberNode.GetComponent<SudokuNumberCell>();
        subBoardCell = subBoardCell == null ? NumberNode.GetComponentInChildren<SudokuNumberCell>() : subBoardCell;
        subBoardCell = subBoardCell == null ? NumberNode.AddComponent<SudokuNumberCell>() : subBoardCell;
        subBoardCell.numberText = subBoardCell.numberText == null ? subBoardCell.GetComponentInChildren<TextMeshPro>() : subBoardCell.numberText;
        subBoardCell.numberText = subBoardCell.numberText == null ? subBoardCell.AddComponent<TextMeshPro>() : subBoardCell.numberText;
        subBoardCell.Id = subBoardCell.Id == 0 ? LoopId++ : subBoardCell.Id;
        var lstCelda = (from x in gameState.lstCeldas where x.Id == subBoardCell.Id select x).ToList();
        if (lstCelda.Count > 0)
        {
            subBoardCell.objCelda = lstCelda.First();
            subBoardCell.Bloqueado = subBoardCell.objCelda.bloqueado;
            subBoardCell.SetNumber(subBoardCell.objCelda.Valor);
            Renderer renderer = subBoardCell.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = subBoardCell.objCelda.bloqueado ? sudokuBoardMaterial.CellDisabled : sudokuBoardMaterial.CellNormal;
            }
        }
        return subBoardCell;
    }
    public void DestroyBoard()
    {
        if (subBoards != null)
        {
            foreach (var child in subBoards)
            {
                Destroy(child.gameObject);
            }
        }
        LoopId = 1;
        subBoards = null;
        IsGameActive = false;
        allCells = new List<SudokuNumberCell>();
    }
    #endregion
}