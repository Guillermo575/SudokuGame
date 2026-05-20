using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class MenuButtonLoad : MonoBehaviour
{
    public TextMeshProUGUI txtDate;
    public TextMeshProUGUI txtBoard;
    public TextMeshProUGUI txtDifficult;
    private GameState gameState;
    private GameManager gameManager;
    private MenuManager menuManager;

    public void Initialize(GameState gameState)
    {
        gameManager = GameManager.GetSingleton();
        menuManager = MenuManager.GetSingleton();
        this.gameState = gameState;
        txtDate.text = gameState.dateCreate;
        txtBoard.text = gameState.BoardType;
        txtDifficult.text = gameState.Difficult;
    }
    public void OnButtonLoad()
    {
        if (gameManager == null || gameState == null) return;
        gameManager.StartGame(gameState);
    }
    public void OnButtonDelete()
    {
        if (gameManager == null || gameState == null) return;
    }
}