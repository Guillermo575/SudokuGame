using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Sudoku.SudokuGenerator;
public class GameManager : MonoBehaviour
{
    #region Variables
    public SudokuBoard sudokuBoard;
    public CamaraController controller;
    public SaveGameSO saveGameSO;
    public SudokuBoardMaterial sudokuBoardMaterial;
    public HUDButtonPanel hUDButtonPanel;
    #endregion

    #region private Variables
    public int LoopId { get; private set; } = 1;
    private SudokuNumberCell sudokuNumberCellSelected;
    #endregion

    #region Awake & Start
    void Awake()
    {
        CreateSingleton();
    }
    private void Start()
    {
        LoadOrCreateGame();
        LoopId = 1;
        sudokuBoard.CreateBoard();
        controller.InitiateCamera();
        if (hUDButtonPanel != null)
            hUDButtonPanel.Initialize(sudokuBoard.numberColumns * sudokuBoard.numberRows);
    }
    #endregion

    #region General
    public int addLoopId()
    {
        return LoopId++;
    }
    private void CreateGame()
    {
        saveGameSO.CreateGame(sudokuBoard.numberColumns, sudokuBoard.numberRows);
    }
    public void setCellSelected(SudokuNumberCell sudokuNumberCell)
    {
        Renderer renderer;
        if (sudokuNumberCellSelected != null)
        {
            renderer = sudokuNumberCellSelected.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sudokuBoardMaterial.CellNormal;
            }
        }
        if (sudokuNumberCell != null)
        {
            sudokuNumberCellSelected = sudokuNumberCell;
            renderer = sudokuNumberCell.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sudokuBoardMaterial.CellSelected;
            }
        }
    }
    public void setCellSelectedValue(int Valor)
    {
        setCellSelectedValue(sudokuNumberCellSelected.Id, Valor);
    }
    public void setCellSelectedValue(int Id, int Valor)
    {
        var lstCelda = saveGameSO.lastGameState.lstCeldas;
        var obj = (from x in lstCelda where x.Id == Id select x).ToList();
        if (obj.Count > 0)
        {
            obj.First().Valor = Valor;
            sudokuNumberCellSelected.SetNumber(Valor);
            setCellSelected(null);
            if (ValidarCeldas(lstCelda))
            {
                Debug.Log("COMPLETADO!!");
            }
        }
    }
    public SudokuGenerator GetSudokuGenerator() 
    { 
        return saveGameSO.lastGameState.sudokuGenerator;
    }
    #endregion

    #region SO
    private void LoadOrCreateGame()
    {
        if (saveGameSO == null || saveGameSO.lastGameState == null || saveGameSO.lastGameState.sudokuGenerator == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas.Count == 0)
        {
            CreateGame();
        }
        sudokuBoard.numberColumns = saveGameSO.lastGameState.sudokuGenerator.ColumnasX;
        sudokuBoard.numberRows = saveGameSO.lastGameState.sudokuGenerator.ColumnasY;
    }
    #endregion

    #region Singleton
    /** @hidden*/
    private static GameManager SingletonGameManager;
    /** @hidden*/
    private GameManager()
    {
    }
    /** Aqui se crea el objeto singleton */
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    /** Solo se puede crear un objeto de la clase GameManager, este metodo obtiene el objeto creado */
    public static GameManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion
}