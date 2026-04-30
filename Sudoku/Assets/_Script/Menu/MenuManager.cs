using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
public class MenuManager : MonoBehaviour
{

    #region Public
    public GameObject container;
    #endregion

    #region Awake
    void Awake()
    {
        CreateSingleton();
    }
    #endregion

    #region General
    public void OnNewGame()
    {
        var gameManager = GameManager.GetSingleton();
        gameManager.CreateGame();
        gameManager.StartGame();
        HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    public void OnContinueGame()
    {
        var gameManager = GameManager.GetSingleton();
        gameManager.StartGame();
        HideShow(false);
        HUDManager.GetSingleton().HideShow(true);
    }
    public void HideShow(bool Visible)
    {
        container.SetActive(Visible);
    }
    #endregion

    #region Singleton
    private static MenuManager SingletonGameManager;
    private MenuManager() { }
    private void CreateSingleton()
    {
        if (SingletonGameManager == null)
        {
            SingletonGameManager = this;
        }
        else
        {
            Debug.LogError("Ya existe una instancia de esta clase");
        }
    }
    public static MenuManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion
}