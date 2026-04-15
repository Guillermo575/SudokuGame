
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

    #region Awake & Start
    void Awake()
    {
        CreateSingleton();
    }
    private void Start()
    {
        LoadOrCreateGame();
        sudokuBoard.CreateBoard();
        controller.InitiateCamera();
    }
    #endregion

    #region SO
    private void LoadOrCreateGame()
    {
        //saveGameSO = AssetDatabase.LoadAssetAtPath<SaveGameSO>(GetAssetPath());
        if (saveGameSO == null || saveGameSO.gameState == null || saveGameSO.gameState.sudokuGenerator == null || saveGameSO.gameState.sudokuGenerator.lstCeldas == null || saveGameSO.gameState.sudokuGenerator.lstCeldas.Count == 0)
        {
            SudokuGenerator sudokuGenerator = new SudokuGenerator(sudokuBoard.numberColumns, sudokuBoard.numberRows);
            //saveGameSO = UnityEngine.ScriptableObject.CreateInstance<SaveGameSO>();
            saveGameSO.gameState = new GameState();
            saveGameSO.gameState.sudokuGenerator = sudokuGenerator;
            //AssetDatabase.CreateAsset(saveGameSO, GetAssetPath());
            //AssetDatabase.SaveAssets();
        }
        sudokuBoard.numberColumns = saveGameSO.gameState.sudokuGenerator.ColumnasX;
        sudokuBoard.numberRows = saveGameSO.gameState.sudokuGenerator.ColumnasY;
    }
    private string GetAssetPath()
    {
        string scriptPath = Directory.GetParent(Application.dataPath).FullName;
        return Path.Combine(scriptPath + "\\Assets\\", "LastGame.asset");
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