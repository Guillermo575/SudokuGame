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
    public HUDButtonPanel hUDButtonPanel;
    public GameObject hudObject;
    #endregion

    #region private Variables
    private SudokuNumberCell sudokuNumberCellSelected;
    private int TotalAlphabet { get { return sudokuBoard.numberColumns * sudokuBoard.numberRows; } }
    private GameState gameState { get { return sudokuBoard == null ? null : sudokuBoard.gameState; } }
    private SudokuGenerator sudokuGenerator { get { return gameState == null ? null : gameState.sudokuGenerator; } }
    #endregion

    #region Awake & Start
    void Awake()
    {
        CreateSingleton();
        if (hUDButtonPanel != null)
            hUDButtonPanel.Initialize(Sudoku.Alphabet.masterAlpha.Length - 1, 0);
    }
    #endregion

    #region General
    public void CreateGame()
    {
        saveGameSO.lastGameState = GameState.CreateGame(sudokuBoard.numberColumns, sudokuBoard.numberRows);
    }
    public void StartGame()
    {
        sudokuBoard.gameObject.SetActive(true);
        sudokuBoard.CreateBoard(saveGameSO.lastGameState);
        controller.InitiateCamera();
        if (hUDButtonPanel != null)
            hUDButtonPanel.HideShowButtons(TotalAlphabet);
    }
    public bool setCellSelected(int Valor)
    {
        SudokuNumberCell targetCell = sudokuBoard.allCells.Find(cell => cell.Id == Valor);
        if (targetCell != null)
        {
            setCellSelected(targetCell);
            return true;
        }
        return false;
    }
    public void setCellSelected(SudokuNumberCell sudokuNumberCell)
    {
        Renderer renderer;
        if (sudokuNumberCellSelected != null)
        {
            renderer = sudokuNumberCellSelected.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sudokuBoard.sudokuBoardMaterial.CellNormal;
            }
        }
        if (sudokuNumberCell != null)
        {
            sudokuNumberCellSelected = sudokuNumberCell;
            renderer = sudokuNumberCell.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sudokuBoard.sudokuBoardMaterial.CellSelected;
            }
        }
    }
    public void setCellSelectedValue(int Valor)
    {
        setCellSelectedValue(sudokuNumberCellSelected.Id, Valor);
    }
    public void setCellSelectedValue(int Id, int Valor, bool AddLog = true)
    {
        if (sudokuNumberCellSelected == null) return;
        if (Valor > TotalAlphabet) return;
        int ValorAntes = sudokuNumberCellSelected.objCelda.Valor;
        if (sudokuNumberCellSelected.SetNumber(Valor))
        {
            if (AddLog)
            {
                gameState.LogAdd(Id, Valor, ValorAntes);
            }
            setCellSelected(null);
            CheckWinGame();
        }
    }
    #endregion

    #region Win
    private void AutoResolveGame()
    {
        var lstCeldas = gameState.lstCeldas;
        for (int l = 0; l < lstCeldas.Count; l++)
        {
            lstCeldas[l].Valor = sudokuGenerator.lstCeldas[l].Valor;
        }
    }
    private bool CheckWinGame()
    {
        if (ValidarCeldas(gameState.lstCeldas))
        {
            Debug.Log("COMPLETADO!!");
            return true;
        }
        return false;
    }
    #endregion

    #region Singleton
    private static GameManager SingletonGameManager;
    private GameManager() { }
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
    public static GameManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion

    #region LogMovement
    public void LogBack()
    {
        if (gameState == null) return;
        var objLog = gameState.LogBack();
        if (objLog == null) return;
        if (setCellSelected(objLog.Id))
        {
            setCellSelectedValue(objLog.Id, objLog.ValorAntes, false);
        }
    }
    public void LogBackMax()
    {
        if (gameState == null || gameState.lstBitacoraMovimiento == null) return;
        for (int l = 0; l < gameState.lstBitacoraMovimiento.Count + 1; l++)
        {
            LogBack();
        }
    }
    public void LogForward()
    {
        if (gameState == null) return;
        var objLog = gameState.LogForward();
        if (objLog == null) return;
        if (setCellSelected(objLog.Id))
        {
            setCellSelectedValue(objLog.Id, objLog.Valor, false);
        }
    }
    public void LogForwardMax()
    {
        if (gameState == null || gameState.lstBitacoraMovimiento == null) return;
        for (int l = 0; l < gameState.lstBitacoraMovimiento.Count + 1; l++)
        {
            LogForward();
        }
    }
    #endregion
}