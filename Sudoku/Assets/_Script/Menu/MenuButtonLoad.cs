using TMPro;
using System.Linq;
public class MenuButtonLoad : MenuButton
{
    public TextMeshProUGUI txtDate;
    public TextMeshProUGUI txtBoard;
    public TextMeshProUGUI txtDifficult;
    private GameState gameState;
    public void Initialize(GameState gameState)
    {
        this.gameState = gameState;
        txtDate.text = gameState.dateCreate;
        txtBoard.text = gameState.BoardType;
        txtDifficult.text = gameState.Difficult;
    }
    public void OnButtonLoad()
    {
        LoadGame(gameState);
    }
    public void OnButtonDelete()
    {
        if (gameManager == null || gameState == null) return;
        SavePlayerPref savePlayerPref = SavePlayerPref.GetSingleton();
        if (savePlayerPref != null)
        {
            savePlayerPref.DeleteGame(gameState.Id);
        }
        Destroy(gameObject);
    }
}