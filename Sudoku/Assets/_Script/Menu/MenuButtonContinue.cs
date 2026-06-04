public class MenuButtonContinue : MenuButton
{
    void Update()
    {
        button.enabled = gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null;
    }
}