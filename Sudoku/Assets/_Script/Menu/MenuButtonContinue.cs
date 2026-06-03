using UnityEngine;
using UnityEngine.UI;
public class MenuButtonContinue : MonoBehaviour
{
    Button buttonContinue;
    private void Awake()
    {
        buttonContinue = GetComponent<Button>();
    }
    void Update()
    {
        var gameManager = GameManager.GetSingleton();
        buttonContinue.enabled = gameManager.saveGameSO != null && gameManager.saveGameSO.lastGameState != null;
    }
    public void OnContinueGame()
    {
        var gameManager = GameManager.GetSingleton();
        gameManager.StartGame();
        MenuManager.GetSingleton().HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
}