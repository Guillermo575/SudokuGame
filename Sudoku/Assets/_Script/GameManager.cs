using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static GameState;
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
    private int TotalAlphabet { get { return sudokuBoard.numberColumns * sudokuBoard.numberRows; } }
    private GameState lastGameState { get { return saveGameSO == null ? null : saveGameSO.lastGameState; } }
    private SudokuGenerator sudokuGenerator { get { return lastGameState == null ? null : lastGameState.sudokuGenerator; } }
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
            hUDButtonPanel.Initialize(TotalAlphabet);
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
        if (sudokuNumberCellSelected == null) return;
        setCellSelectedValue(sudokuNumberCellSelected.Id, Valor);
    }
    public void setCellSelectedValue(int Id, int Valor, bool AddLog = true)
    {
        if (Valor > TotalAlphabet) return;
        var lstCelda = lastGameState.lstCeldas;
        var obj = (from x in lstCelda where x.Id == Id select x).ToList();
        if (obj.Count > 0)
        {
            int ValorAntes = obj.First().Valor;
            if (sudokuNumberCellSelected.SetNumber(Valor))
            {
                if (AddLog)
                {
                    obj.First().Valor = Valor;
                    lastGameState.LogAdd(Id, Valor, ValorAntes);
                }
                setCellSelected(null);
                if (ValidarCeldas(lstCelda))
                {
                    Debug.Log("COMPLETADO!!");
                }
            }
        }
    }
    public SudokuGenerator GetSudokuGenerator() 
    { 
        return sudokuGenerator;
    }
    public List<Celda> GetLstCeldas()
    {
        return lastGameState.lstCeldas;
    }
    #endregion

    #region SO
    private void LoadOrCreateGame()
    {
        if (saveGameSO == null || lastGameState == null || sudokuGenerator == null || sudokuGenerator.lstCeldas == null || sudokuGenerator.lstCeldas.Count == 0)
        {
            CreateGame();
        }
        sudokuBoard.numberColumns = sudokuGenerator.ColumnasX;
        sudokuBoard.numberRows = sudokuGenerator.ColumnasY;
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

    #region LogMovement
    public void LogBack()
    {
        if (lastGameState == null) return;
        var objLog = lastGameState.LogBack();
        if (objLog == null) return;
        setCellSelectedValue(objLog.Id, objLog.ValorAntes, false);
    }
    public void LogForward()
    {
        if (lastGameState == null) return;
        var objLog = lastGameState.LogForward();
        if (objLog == null) return;
        setCellSelectedValue(objLog.Id, objLog.Valor, false);
    }
    #endregion
}