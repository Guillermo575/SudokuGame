public class MenuButtonContinue : MenuButton
{
    void Update()
    {
        button.interactable = gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null && !string.IsNullOrEmpty(gameManager.saveGameSO.lastGameState.Id);
    }
}