using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{

    #region Public
    public GameObject container;
    public MenuConfirmar menuConfirmar;
    public List<GameObject> lstMenus;
    #endregion

    #region Private
    public List<GameObject> lstMenuTree;
    #endregion

    #region Awake
    void Awake()
    {
        CreateSingleton();
        //lstMenuTree = new List<GameObject>();
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

    #region Menus
    public GameObject GetMenu(string name)
    {
        var lst = (from x in lstMenus where x.name.ToUpper() == name.ToUpper() select x).ToList();
        return lst.Count > 0 ? lst.First().gameObject : null;
    }
    public void BackMenu()
    {
        if (lstMenuTree.Count > 1)
        {
            var objBack = lstMenuTree[lstMenuTree.Count - 2];
            var objActual = lstMenuTree.Last();
            lstMenuTree.Remove(objActual);
            objActual.SetActive(false);
            objBack.SetActive(true);
        }
    }
    public void ShowMenu(GameObject objMenu)
    {
        if (objMenu != null)
        {
            SetActiveCanvas();
            lstMenuTree.Add(objMenu);
            objMenu.SetActive(true);
        }
    }
    public void SetActiveCanvas(bool value = false)
    {
        var lst = lstMenuTree;
        foreach (var x in lst)
        {
            if (x.gameObject != null && x.gameObject.activeSelf)
            {
                x.gameObject.SetActive(value);
            }
        }
    }
    public void DeleteMenuTree()
    {
        SetActiveCanvas(false);
        lstMenuTree = new List<GameObject>();
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

    #region SalirJuego
    public void ExitGame()
    {
        menuConfirmar.MostrarPantallaConfirmar(EventoRegresarAPantallaPrincipal, "Do you want to exit?");
    }
    private void EventoRegresarAPantallaPrincipal()
    {
        Application.Quit();
    }
    #endregion
}