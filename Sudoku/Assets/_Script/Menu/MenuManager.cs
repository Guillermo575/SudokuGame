using Sudoku;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{

    #region Public
    public GameObject container;
    public GameObject buttonContinue;
    public MenuConfirmar menuConfirmar;
    public List<GameObject> lstMenus;
    public TMP_Dropdown dropdownBoard;
    public TMP_Dropdown dropdownDifficult;
    #endregion

    #region Private
    public List<GameObject> lstMenuTree;
    #endregion

    #region Awake
    void Awake()
    {
        CreateSingleton();
        //lstMenuTree = new List<GameObject>();
    }
    void OnStart()
    {
        var gameManager = GameManager.GetSingleton();
        buttonContinue.SetActive(gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null);
    }
    #endregion

    public class Configuration
    {
        public eType etype { get; set; }
        public int numberColumns { get; set; }
        public int numberRows { get; set; }
        public string name { get { return etype.ToString().ToUpper().Replace("CR", ""); } }
    }
    public List<Configuration> lstConfType = new List<Configuration>
    {
        new Configuration { etype = eType.cr9x9, numberColumns = 3, numberRows = 3 },
        new Configuration { etype = eType.cr16x16, numberColumns = 4, numberRows = 4 },
        new Configuration { etype = eType.cr6x6, numberColumns = 3, numberRows = 2 },
        new Configuration { etype = eType.cr4x4, numberColumns = 4, numberRows = 4 },
        new Configuration { etype = eType.cr20x20, numberColumns = 5, numberRows = 4 },
        new Configuration { etype = eType.cr25x25, numberColumns = 5, numberRows = 5 },
        new Configuration { etype = eType.cr30x30, numberColumns = 6, numberRows = 5 },
        new Configuration { etype = eType.cr36x36, numberColumns = 6, numberRows = 6 },
    };
    public static int[] CalcularIntervaloOcultamiento(int filas, int columnas, string dificultad = "NORMAL")
    {
        int totalCasillas = filas * columnas;
        float factorDificultad = 0;
        switch (dificultad)
        {
            case "EASY":
                factorDificultad = 0.15f;
                break;
            case "NORMAL":
                factorDificultad = 0.25f;
                break;
            case "HARD":
                factorDificultad = 0.40f;
                break;
            default:
                factorDificultad = 0.25f;
                break;
        }
        int casillasAOcultar = Mathf.RoundToInt(totalCasillas * factorDificultad);
        int intervaloMinimo = Mathf.Clamp(Mathf.RoundToInt(totalCasillas / (casillasAOcultar * 1.5f)), 2, 8);
        int intervaloMaximo = Mathf.Clamp(Mathf.RoundToInt(totalCasillas / casillasAOcultar), intervaloMinimo + 1, 10);
        return new int[] { intervaloMinimo, intervaloMaximo };
    }

    #region General	
    public void HideShow(bool Visible)
    {
        container.SetActive(Visible);
    }
    #endregion

    #region New Game
    public void OnNewGame()
    {
        if (buttonContinue.activeSelf)
        {
            menuConfirmar.MostrarPantallaConfirmar(EventoNewGame, "The previous game will be deleted, Are you sure?");
        }
        else
        {
            EventoNewGame();
        }
    }
    public void EventoNewGame()
    {
        var gameManager = GameManager.GetSingleton();
        string BoardType = dropdownBoard.options[dropdownBoard.value].text.ToUpper().Replace(" ", "");
        var getDiff = (from x in lstConfType where x.name == BoardType select x).ToList();
        if (getDiff.Count == 0) return;
        string DifficultType = dropdownBoard.options[dropdownBoard.value].text.ToUpper().Replace(" ", "");
        var getInterv = CalcularIntervaloOcultamiento(getDiff.First().numberRows, getDiff.First().numberColumns, DifficultType);
        gameManager.CreateGame(getDiff.First().numberRows, getDiff.First().numberColumns, getInterv[0], getInterv[1]);
        gameManager.StartGame();
        HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    #endregion

    #region Continue Game
    public void OnContinueGame()
    {
        var gameManager = GameManager.GetSingleton();
        gameManager.StartGame();
        HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    #endregion

    #region SalirJuego
    public void QuitApp()
    {
        menuConfirmar.MostrarPantallaConfirmar(EventoQuitApp, "Do you want to exit?");
    }
    private void EventoQuitApp()
    {
        Application.Quit();
    }
    #endregion		

    #region Menus
    public GameObject GetMenu(string name)
    {
        var lst = (from x in lstMenus where x.name.ToUpper() == name.ToUpper() select x).ToList();
        return lst.Count > 0 ? lst.First().gameObject : null;
    }
    public void BackMenu()
    {
        if (lstMenuTree.Count > 1)
        {
            var objBack = lstMenuTree[lstMenuTree.Count - 2];
            var objActual = lstMenuTree.Last();
            lstMenuTree.Remove(objActual);
            objActual.SetActive(false);
            objBack.SetActive(true);
        }
    }
    public void ShowMenu(GameObject objMenu)
    {
        if (objMenu != null)
        {
            SetActiveCanvas();
            lstMenuTree.Add(objMenu);
            objMenu.SetActive(true);
        }
    }
    public void SetActiveCanvas(bool value = false)
    {
        var lst = lstMenuTree;
        foreach (var x in lst)
        {
            if (x.gameObject != null && x.gameObject.activeSelf)
            {
                x.gameObject.SetActive(value);
            }
        }
    }
    public void DeleteMenuTree()
    {
        SetActiveCanvas(false);
        lstMenuTree = new List<GameObject>();
    }
    #endregion

    #region Singleton
    private static MenuManager SingletonGameManager;
    private MenuManager() { }
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
    public static MenuManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion
}