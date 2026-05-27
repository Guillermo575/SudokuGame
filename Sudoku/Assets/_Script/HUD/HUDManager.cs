using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    #region Public
    public GameObject hudContainer;
    public float padding = 20f;
    public HUDButtonPanel buttonPanel;
    public HUDSliderZoom sliderZoom;
    public HUDLogControl logControl;
    #endregion

    #region Private
    private RectTransform _rectTransform;
    private CanvasScaler _canvasScaler;
    private GameManager gameManager;
    private GameManager.OrientationMode CurrentOrientation;
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
        _rectTransform = hudContainer.GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            Debug.LogError("HUDManager: El hudContainer no tiene un RectTransform. Asegúrate de que es un elemento UI.");
            enabled = false;
            return;
        }
        _canvasScaler = GetComponentInParent<CanvasScaler>();
        if (_canvasScaler == null)
        {
            Debug.LogWarning("HUDManager: No se encontró CanvasScaler en los padres.  Es posible que la UI no se escale correctamente.");
        }
        UpdateOrientation(CurrentOrientation);
    }
    void Start()
    {
        gameManager = GameManager.GetSingleton();
    }
    void Update()
    {
        var orientation = gameManager.CurrentOrientation;
        if (CurrentOrientation != orientation)
        {
            UpdateOrientation(orientation);
        }
    }
    #endregion

    #region Orientation
    void UpdateOrientation(GameManager.OrientationMode orientationMode)
    {
        CurrentOrientation = orientationMode;
        switch (orientationMode)
        {
            case GameManager.OrientationMode.Vertical: PositionHudVertical(); return;
            case GameManager.OrientationMode.HorizontalLeft: PositionHudHorizontalLeft(); return;
            case GameManager.OrientationMode.HorizontalRight: PositionHudHorizontalRight(); return;
        }
        Debug.LogWarning("HUDManager: Orientación desconocida.  Manteniendo la posición actual del HUD.");
    }
    void PositionHudVertical()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(0.5f, 0);
        _rectTransform.anchorMax = new Vector2(0.5f, 0);
        _rectTransform.pivot = new Vector2(0.5f, 0);
        //_rectTransform.offsetMin = new Vector2(padding, padding);
        //_rectTransform.offsetMax = new Vector2(-padding, _rectTransform.rect.height);
    }
    void PositionHudHorizontalLeft()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(0, 0.5f);
        _rectTransform.anchorMax = new Vector2(0, 0.5f);
        _rectTransform.pivot = new Vector2(0, 0.5f);
        //_rectTransform.offsetMin = new Vector2(padding, padding);
        //_rectTransform.offsetMax = new Vector2(_rectTransform.rect.width, -padding);
    }
    void PositionHudHorizontalRight()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(1, 0.5f);
        _rectTransform.anchorMax = new Vector2(1, 0.5f);
        _rectTransform.pivot = new Vector2(1, 0.5f);
        //_rectTransform.offsetMin = new Vector2(-_rectTransform.rect.width, padding);
        //_rectTransform.offsetMax = new Vector2(-padding, -padding);
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