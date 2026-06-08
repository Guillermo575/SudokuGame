using Sudoku;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{
    #region Variables
    public SudokuBoard sudokuBoard;
    public SaveGameSO saveGameSO;
    public HUDButtonPanel hUDButtonPanel;
    public GameObject hudObject;
    #endregion

    #region private Variables
    private SudokuNumberCell sudokuNumberCellSelected;
    private int TotalAlphabet { get { return sudokuBoard.numberColumns * sudokuBoard.numberRows; } }
    private GameState gameState { get { return sudokuBoard == null ? null : sudokuBoard.gameState; } }
    private SudokuGenerator sudokuGenerator { get { return gameState == null ? null : gameState.sudokuGenerator; } }
    public bool IsGameActive { get { return sudokuBoard == null ? false : sudokuBoard.IsGameActive; } }
    public bool IsPause { get { return sudokuBoard == null ? false : sudokuBoard.IsGameActive ? _IsPause : false; } }
    private bool _IsPause;
    public bool IsWin { get { return sudokuBoard == null ? false : sudokuBoard.IsGameActive ? _IsWin : false; } }
    private bool _IsWin;
    public bool ShowWinPanel;
    #endregion

    #region Awake & Start
    void Awake()
    {
        CreateSingleton();
        hUDButtonPanel.Initialize(Sudoku.Alphabet.masterAlpha.Length - 1, 0);
    }
    #endregion

    #region General
    public void StartGame()
    {
        StartGame(saveGameSO.lastGameState);
    }
    public void StartGame(GameState gameState)
    {
        if (gameState == null) return;
        sudokuBoard.gameObject.SetActive(true);
        sudokuBoard.CreateBoard(gameState);
        if (hUDButtonPanel != null)
            hUDButtonPanel.HideShowButtons(TotalAlphabet);
        saveGameSO.lastGameState = gameState;
        _IsPause = false;
        ShowWinPanel = false;
        _IsWin = CheckWinGame();
    }
    public void SaveGame()
    {
        if (gameState == null) return;
        saveGameSO.SaveGame(saveGameSO.lastGameState);
    }
    public void DestroyGame()
    {
        if (sudokuBoard != null)
        {
            sudokuBoard.DestroyBoard();
            sudokuBoard.gameObject.SetActive(false);
        }
        if (hUDButtonPanel != null)
            hUDButtonPanel.HideShowButtons(0);
        _IsPause = false;
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
            if (CheckWinGame())
            {
                _IsWin = true;
            }
        }
    }
    #endregion

    #region Pause
    public void TogglePause()
    {
        _IsPause = !_IsPause;
    }
    public void setPause(bool Pause)
    {
        _IsPause = Pause;
    }
    #endregion

    #region Win
    public void AutoResolveGame()
    {
        var lstCeldas = gameState.lstCeldas;
        for (int l = 0; l < lstCeldas.Count; l++)
        {
            var obj = lstCeldas[l];
            if (!obj.bloqueado)
            {
                var objCelda = (from x in sudokuBoard.allCells where x.Id == obj.Id select x).ToList();
                if (objCelda.Count > 0)
                {
                    sudokuNumberCellSelected = objCelda.First();
                    setCellSelectedValue(sudokuGenerator.lstCeldas[l].Valor);
                    obj.Valor = sudokuGenerator.lstCeldas[l].Valor;
                }
            }
        }
        if (CheckWinGame())
        {
            _IsWin = true;
        }
    }
    private bool CheckWinGame()
    {
        if (Sudoku.SudokuGenerator.ValidarCeldas(gameState.lstCeldas))
        {
            //Debug.Log("COMPLETADO!!");
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