public class MenuButtonLoadControl : MenuButton
{
    void Update()
    {
        button.interactable = gameManager.saveGameSO != null && gameManager.saveGameSO.lstGames != null && gameManager.saveGameSO.lstGames.Count > 0;
    }
}