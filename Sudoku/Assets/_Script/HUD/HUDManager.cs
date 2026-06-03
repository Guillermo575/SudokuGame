using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    #region Public
    public GameObject hudContainer;
    #endregion

    #region Awake
    void Awake()
    {
        CreateSingleton();
        if (hudContainer == null)
        {
            Debug.LogError("HUDManager: hudContainer no asignado. Por favor, asigna el GameObject que contiene el HUD en el Inspector.");
            enabled = false;
            return;
        }
    }
    #endregion

    #region General
    public void HideShow(bool Visible)
    {
        hudContainer.SetActive(Visible);
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