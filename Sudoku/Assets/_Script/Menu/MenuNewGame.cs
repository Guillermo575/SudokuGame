using Sudoku;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Threading;
using System.Collections;
using UnityEngine.UI;
public class MenuNewGame : _Menu
{
    #region Public
    public TMP_Dropdown dropdownBoard;
    public TMP_Dropdown dropdownDifficult;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public Button cancelButton;
    public List<Button> otherButtons = new List<Button>();
    public List<TMP_Dropdown> otherDropdowns = new List<TMP_Dropdown>();
    #endregion

    #region Private
    private Thread sudokuThread;
    private SudokuGenerator currentSudokuGenerator;
    private bool isCancelled = false;
    private const float GENERATION_TIMEOUT_SECONDS = 60f;
    #endregion

    #region Configuration
    public class Configuration
    {
        public eType etype { get; set; }
        public int numberColumns { get; set; }
        public int numberRows { get; set; }
        public string name { get { return etype.ToString().ToUpper().Replace("CR", ""); } }
        public bool QuickMode { get; set; }
    }
    public List<Configuration> lstConfType = new List<Configuration>
    {
        new Configuration { etype = eType.cr9x9, numberColumns = 3, numberRows = 3, QuickMode = false },
        new Configuration { etype = eType.cr16x16, numberColumns = 4, numberRows = 4, QuickMode = false },
        new Configuration { etype = eType.cr6x6, numberColumns = 3, numberRows = 2, QuickMode = false },
        new Configuration { etype = eType.cr4x4, numberColumns = 4, numberRows = 4, QuickMode = false },
        new Configuration { etype = eType.cr20x20, numberColumns = 5, numberRows = 4, QuickMode = true },
        new Configuration { etype = eType.cr25x25, numberColumns = 5, numberRows = 5, QuickMode = true },
        new Configuration { etype = eType.cr30x30, numberColumns = 6, numberRows = 5, QuickMode = true },
        new Configuration { etype = eType.cr36x36, numberColumns = 6, numberRows = 6, QuickMode = true },
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

    #region Start
    internal override void Start()
    {
        base.Start();
        HideLoadingUI();
        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnCancelGeneration);
    }
    #endregion

    #region Events
    public void OnNewGame()
    {
        var gameManager = GameManager.GetSingleton();
        if (gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null)
        {
            MenuManager.GetSingleton().menuConfirmar.MostrarPantallaConfirmar(EventoNewGameYes, "The previous game will be deleted, Are you sure?");
        }
        else
        {
            EventoNewGame();
        }
    }
    public void EventoNewGameYes()
    {
        menuManager.ShowMenu(menuManager.GetMenu("Menu_pnlNewgame"));
        EventoNewGame();
    }
    public void EventoNewGame()
    {
        var gameManager = GameManager.GetSingleton();
        string BoardType = dropdownBoard.options[dropdownBoard.value].text.ToUpper().Replace(" ", "");
        var getDiff = (from x in lstConfType where x.name == BoardType select x).ToList();
        if (getDiff.Count == 0) return;
        string DifficultType = dropdownDifficult.options[dropdownDifficult.value].text.ToUpper().Replace(" ", "");
        var getInterv = CalcularIntervaloOcultamiento(getDiff.First().numberRows, getDiff.First().numberColumns, DifficultType);
        StartSudokuGeneration(getDiff.First(), BoardType, DifficultType, getInterv);
    }
    #endregion

    #region Sudoku Generation
    private void StartSudokuGeneration(Configuration configuration, string boardType, string difficultType, int[] interval)
    {
        isCancelled = false;
        ShowLoadingUI();
        sudokuThread = new Thread(() => GenerateSudokuThread(configuration, boardType, difficultType, interval))
        {
            IsBackground = true
        };
        sudokuThread.Start();
        StartCoroutine(MonitorProgress());
    }
    private void GenerateSudokuThread(Configuration configuration, string boardType, string difficultType, int[] interval)
    {
        try
        {
            currentSudokuGenerator = new SudokuGenerator(ColumnasX: configuration.numberColumns, ColumnasY: configuration.numberRows, StartNow: false, QuickFunction: configuration.QuickMode);
            currentSudokuGenerator.Start();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error generando Sudoku: {ex.Message}");
        }
    }
    private IEnumerator MonitorProgress()
    {
        while (!isCancelled && (sudokuThread == null || sudokuThread.IsAlive))
        {
            if (currentSudokuGenerator != null)
            {
                float progress = currentSudokuGenerator.progress;
                UpdateLoadingUI(progress);
                if (progress >= 100)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        if (sudokuThread != null && sudokuThread.IsAlive)
        {
            sudokuThread.Join();
        }
        if (!isCancelled && currentSudokuGenerator != null)
        {
            HideLoadingUI();
            StartGame(currentSudokuGenerator);
        }
        else if (isCancelled)
        {
            HideLoadingUI();
            currentSudokuGenerator = null;
        }
    }
    #endregion

    #region Load Bar GUI
    private void UpdateLoadingUI(float progress)
    {
        if (loadingSlider != null)
            loadingSlider.value = progress;
        if (loadingText != null)
            loadingText.text = $"{(int)(progress * 100)}%";
    }
    private void ShowLoadingUI()
    {
        if (loadingSlider != null)
            loadingSlider.gameObject.SetActive(true);
        if (loadingText != null)
            loadingText.gameObject.SetActive(true);
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(true);
        foreach (var button in otherButtons)
        {
            if (button != null)
                button.gameObject.SetActive(false);
        }
        foreach (var dropdown in otherDropdowns)
        {
            if (dropdown != null)
                dropdown.gameObject.SetActive(false);
        }
    }
    private void HideLoadingUI()
    {
        if (loadingSlider != null)
            loadingSlider.gameObject.SetActive(false);
        if (loadingText != null)
            loadingText.gameObject.SetActive(false);
        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);
        foreach (var button in otherButtons)
        {
            if (button != null)
                button.gameObject.SetActive(true);
        }
        foreach (var dropdown in otherDropdowns)
        {
            if (dropdown != null)
                dropdown.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Start & Cancel
    public void OnCancelGeneration()
    {
        isCancelled = true;
        if (sudokuThread != null && sudokuThread.IsAlive)
        {
            sudokuThread.Abort();
        }
        HideLoadingUI();
        currentSudokuGenerator = null;
    }
    public void StartGame(SudokuGenerator sudokuGenerator)
    {
        var gameManager = GameManager.GetSingleton();
        string BoardType = dropdownBoard.options[dropdownBoard.value].text.ToUpper().Replace(" ", "");
        string DifficultType = dropdownDifficult.options[dropdownDifficult.value].text.ToUpper().Replace(" ", "");
        var getDiff = (from x in lstConfType where x.name == BoardType select x).ToList();
        var getInterv = CalcularIntervaloOcultamiento(getDiff.First().numberRows, getDiff.First().numberColumns, DifficultType);
        var newgame = GameState.CreateGame(sudokuGenerator, BoardType, DifficultType, getInterv[0], getInterv[1]);
        gameManager.StartGame(newgame);
        MenuManager.GetSingleton().HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    #endregion
}