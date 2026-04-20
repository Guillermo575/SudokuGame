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
    #endregion

    #region private Variables
    public int LoopId { get; private set; } = 1;
    public SudokuGenerator sudokuGenerator { get; private set; }
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
    }
    #endregion

    #region General
    public int addLoopId()
    {
        return LoopId++;
    }
    private void CreateGame()
    {
        sudokuGenerator = new SudokuGenerator(sudokuBoard.numberColumns, sudokuBoard.numberRows);
        BlockCells(sudokuGenerator.lstCeldas);
        saveGameSO.lastGameState = new GameState();
        saveGameSO.lastGameState.sudokuGenerator = sudokuGenerator;
        var lstCeldas = new List<Celda>();
        foreach (var obj in sudokuGenerator.lstCeldas)
        {
            lstCeldas.Add
            (
                new Celda
                {
                    Id = obj.Id,
                    IdCuadrante = obj.IdCuadrante,
                    CuadranteEjeX = obj.CuadranteEjeX,
                    CuadranteEjeY = obj.CuadranteEjeY,
                    EjeX = obj.EjeX,
                    EjeY = obj.EjeY,
                    bloqueado = obj.bloqueado,
                    Valor = obj.bloqueado ? obj.Valor : 0,
                }
            );
        }
        saveGameSO.lastGameState.lstCeldas = lstCeldas;
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
    #endregion

    #region SO
    private void LoadOrCreateGame()
    {
        if (saveGameSO == null || saveGameSO.lastGameState == null || saveGameSO.lastGameState.sudokuGenerator == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas == null || saveGameSO.lastGameState.sudokuGenerator.lstCeldas.Count == 0)
        {
            CreateGame();
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
    public void BlockCells(List<SudokuGenerator.Celda> lstCelda, int cicloMin = 2, int cicloMax = 4)
    {
        foreach (var obj in lstCelda)
        {
            obj.bloqueado = true;
        }
        System.Random rnd = new System.Random();
        int ciclo = rnd.Next(cicloMin, cicloMax);
        for (int i = 0; i < lstCelda.Count; i += ciclo)
        {
            ciclo = rnd.Next(cicloMin, cicloMax);
            int inicio = i;
            int fin = Math.Min(i + ciclo - 1, lstCelda.Count - 1);
            int indiceAleatorio = rnd.Next(inicio, fin + 1);
            lstCelda[indiceAleatorio].bloqueado = false;
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