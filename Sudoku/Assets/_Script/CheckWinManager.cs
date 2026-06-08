using UnityEngine;
public class CheckWinManager : MonoBehaviour
{
    private GameManager gameManager;
    private MenuManager menuManager;
    private HUDManager hudManager;
    void Start()
    {
        gameManager = GameManager.GetSingleton();
        menuManager = MenuManager.GetSingleton();
        hudManager = HUDManager.GetSingleton();
    }
    void Update()
    {
        if (gameManager.IsWin && !gameManager.ShowWinPanel)
        {
            gameManager.ShowWinPanel = true;
            menuManager.ClearAndShowMenu(menuManager.menuWin);
            hudManager.HideShow(false);
        }
    }
}