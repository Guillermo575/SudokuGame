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