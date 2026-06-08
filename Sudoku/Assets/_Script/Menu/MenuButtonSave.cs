public class MenuButtonSave : MenuButton
{
    public TextDisplayController textDisplayController;
    public void OnSaveGame()
    {
        if (gameManager == null) return;
        gameManager.SaveGame();
        textDisplayController.ShowText("Game Saved!");
    }
}