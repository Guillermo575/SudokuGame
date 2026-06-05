using UnityEngine;
using UnityEngine.UI;
public class MenuButton : MonoBehaviour
{
    #region Private
    internal Button button;
    internal GameManager gameManager;
    internal MenuManager menuManager;
    internal HUDManager hudManager;
    internal CamaraController camaraController;
    #endregion

    #region Awake & Start
    void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        menuManager = MenuManager.GetSingleton();
        hudManager = HUDManager.GetSingleton();
        camaraController = CamaraController.GetSingleton();
    }
    #endregion

    #region Methods
    public void OnContinueGame()
    {
        if (gameManager == null) return;
        gameManager.StartGame();
        RutinaStartGame();
    }
    public void LoadGame(GameState gameState)
    {
        if (gameManager == null || gameState == null) return;
        gameManager.StartGame(gameState);
        RutinaStartGame();
    }
    public void RutinaStartGame()
    {
        menuManager.HideShow(false);
        hudManager.HideShow(true);
        camaraController.CentrarCamara();
    }
    public void PauseGame()
    {
        menuManager.SetActiveCanvas();
        menuManager.lstMenuTree.Clear();
        menuManager.ShowMenu(menuManager.menuPause);
        menuManager.HideShow(true);
        hudManager.HideShow(false);
    }
    public void ExitGame()
    {
        menuManager.menuConfirmar.MostrarPantallaConfirmar(EventExitGame, "Do you want to exit?");
    }
    public void EventExitGame()
    {
        gameManager.DestroyGame();
        menuManager.SetActiveCanvas();
        menuManager.lstMenuTree.Clear();
        menuManager.ShowMenu(menuManager.menuMain);
        menuManager.HideShow(true);
        hudManager.HideShow(false);
    }
    public void ResumeGame()
    {
        menuManager.HideShow(false);
        hudManager.HideShow(true);
    }
    #endregion
}