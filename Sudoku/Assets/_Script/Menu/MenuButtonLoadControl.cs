public class MenuButtonLoadControl : MenuButton
{
    void Update()
    {
        SavePlayerPref savePlayerPref = SavePlayerPref.GetSingleton();
        button.interactable = savePlayerPref != null && savePlayerPref.GetAllGames().Count > 0;
    }
}