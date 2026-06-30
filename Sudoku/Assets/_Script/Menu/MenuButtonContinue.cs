public class MenuButtonContinue : MenuButton
{
    void Update()
    {
        SavePlayerPref savePlayerPref = SavePlayerPref.GetSingleton();
        var lastGame = savePlayerPref != null ? savePlayerPref.GetLastGameState() : null;
        button.interactable = lastGame != null && !string.IsNullOrEmpty(lastGame.Id);
    }
}