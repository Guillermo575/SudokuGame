using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HUDManager : MonoBehaviour
{
    #region Public
    public GameObject container;
    public HUDButtonPanel buttonPanel;
    public HUDSliderZoom sliderZoom;
    public HUDLogControl logControl;
    #endregion

    #region Awake
    void Awake()
    {
        CreateSingleton();
    }
    #endregion

    #region General
    public void HideShow(bool Visible)
    {
        container.SetActive(Visible);
    }
    #endregion

    #region Singleton
    private static HUDManager SingletonGameManager;
    private HUDManager() { }
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
    public static HUDManager GetSingleton()
    {
        return SingletonGameManager;
    }
    #endregion
}