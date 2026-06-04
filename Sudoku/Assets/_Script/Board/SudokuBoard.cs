using Sudoku;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Sudoku.SudokuGenerator;
public class SudokuBoard : MonoBehaviour
{
    #region Prefabs
    public GameObject mainBoardPrefab;
    public GameObject subBoardPrefab;
    public GameObject numberNodePrefab;
    public SudokuBoardMaterial sudokuBoardMaterial;
    #endregion

    #region Public
    public float spaceBetweenSubBoards = 0.05f;
    public float spaceBetweenNodes = 0.025f;
    public float sizeNumberNode = 1f;
    public List<SudokuNumberCell> allCells { get; private set; }
    #endregion

    #region Private
    public int numberColumns { get; private set; } = 3;
    public int numberRows { get; private set; } = 3;
    public int LoopId { get; private set; } = 1;
    public GameState gameState { get; private set; }
    private SudokuSubBoard[,] subBoards;
    public bool IsGameActive { get; private set; }
    #endregion

    #region Update
    private void Update()
    {
        UpdatePosition();
    }
    #endregion

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
                GameObject subBoardObj = Instantiate(subBoardPrefab, Vector3.zero, Quaternion.identity, this.transform);
                subBoardObj.name = $"SubBoard_{j}_{i}";
                SudokuSubBoard subBoard = subBoardObj.GetComponent<SudokuSubBoard>();
                if (subBoard == null)
                {
                    subBoard = subBoardObj.AddComponent<SudokuSubBoard>();
                }
                subBoard.Initialize(this);
                subBoards[j, i] = subBoard;
            }
        }
        allCells = FindObjectsByType<SudokuNumberCell>(FindObjectsSortMode.InstanceID).ToList();
        IsGameActive = true;
        UpdatePosition();
    }
    public void UpdatePosition()
    {
        if (gameState == null) return;
        var SizeNumber = sizeNumberNode + spaceBetweenNodes;
        var SizeBoard = numberColumns * numberRows * SizeNumber + (spaceBetweenSubBoards * 4);
        mainBoardPrefab.transform.localScale = new Vector3(SizeBoard, 1f, SizeBoard);
        for (int i = 0; i < numberRows; i++)
        {
            for (int j = 0; j < numberColumns; j++)
            {
                SudokuSubBoard subBoardObj = subBoards[j, i];
                Vector3 position = new Vector3(spaceBetweenSubBoards + (j * ((SizeNumber * numberRows) + spaceBetweenSubBoards)), 0.2f, -spaceBetweenSubBoards + (-i * ((SizeNumber * numberColumns) + spaceBetweenSubBoards)));
                subBoardObj.gameObject.transform.localPosition = position;
                subBoardObj.UpdatePosition();
            }
        }
    }
    public void DestroyBoard()
    {
        Transform[] children = mainBoardPrefab.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != mainBoardPrefab.transform)
            {
                Destroy(child.gameObject);
            }
        }
        LoopId = 1;
        subBoards = null;
        IsGameActive = false;
    }
    public List<Celda> GetLstCeldas()
    {
        return gameState.lstCeldas;
    }
    public int addLoopId()
    {
        return LoopId++;
    }
}