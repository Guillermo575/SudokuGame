using Sudoku;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class MenuNewGame : _Menu
{
    #region Variables
    public TMP_Dropdown dropdownBoard;
    public TMP_Dropdown dropdownDifficult;
    #endregion

    #region Configuration
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
    #endregion

    #region Methods 
    public void OnNewGame()
    {
        var gameManager = GameManager.GetSingleton();
        if (gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null)
        {
            MenuManager.GetSingleton().menuConfirmar.MostrarPantallaConfirmar(EventoNewGame, "The previous game will be deleted, Are you sure?");
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
        string DifficultType = dropdownDifficult.options[dropdownDifficult.value].text.ToUpper().Replace(" ", "");
        var getInterv = CalcularIntervaloOcultamiento(getDiff.First().numberRows, getDiff.First().numberColumns, DifficultType);
        var newgame = GameState.CreateGame(BoardType, DifficultType, getDiff.First().numberRows, getDiff.First().numberColumns, getInterv[0], getInterv[1]);
        gameManager.StartGame(newgame);
        MenuManager.GetSingleton().HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    #endregion
}