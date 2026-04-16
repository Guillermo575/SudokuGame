
using Sudoku;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    #region Variables
    public SudokuBoard sudokuBoard;
    public CamaraController controller;
    public SaveGameSO saveGameSO;
    #endregion

    #region private Variables
    public int LoopId { get; private set; } = 1;
    public SudokuGenerator sudokuGenerator { get; private set; }
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
    }
    #endregion

    #region General
    public int addLoopId()
    {
        return LoopId++;
    }
    #endregion

    #region SO
    private void LoadOrCreateGame()
    {
        if (saveGameSO == null || saveGameSO.lastGameState == null || saveGameSO.lastGameState.sudokuGenerator == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas.Count == 0)
        {
            sudokuGenerator = new SudokuGenerator(sudokuBoard.numberColumns, sudokuBoard.numberRows);
            BlockCells(sudokuGenerator.lstCeldas);
            saveGameSO.lastGameState = new GameState();
            saveGameSO.lastGameState.sudokuGenerator = sudokuGenerator;
        }
        else
        {
            sudokuGenerator = saveGameSO.lastGameState.sudokuGenerator;
        }
        sudokuBoard.numberColumns = saveGameSO.lastGameState.sudokuGenerator.ColumnasX;
        sudokuBoard.numberRows = saveGameSO.lastGameState.sudokuGenerator.ColumnasY;
    }
    #endregion

    #region BlockCells
    public void BlockCells(List<SudokuGenerator.Celda> lstCelda)
    {
        for (int l = 0; l < lstCelda.Count; l++) 
        { 
            if (l % 3 == 0)
            {
                lstCelda[l].bloqueado = true;
            }
        }
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