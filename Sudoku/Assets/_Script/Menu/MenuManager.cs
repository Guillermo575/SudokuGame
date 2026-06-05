using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MenuManager : MonoBehaviour
{
    #region Public
    public GameObject container;
    public GameObject buttonContinue;
    public GameObject menuPause;
    public GameObject menuMain;
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
    public void HideShow(bool Visible)
    {
        container.SetActive(Visible);
    }
    #endregion

    #region SalirJuego
    public void QuitApp()
    {
        menuConfirmar.MostrarPantallaConfirmar(EventoQuitApp, "Do you want to exit?");
    }
    private void EventoQuitApp()
    {
        Application.Quit();
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
}