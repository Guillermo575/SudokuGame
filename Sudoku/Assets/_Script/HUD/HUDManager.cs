using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    public enum OrientationMode
    {
        Vertical,
        HorizontalLeft,
        HorizontalRight
    }
    public OrientationMode CurrentOrientation { get; private set; }

    #region Public
    public GameObject hudContainer;
    public float padding = 20f;
    public bool isLeftHanded = false;
    public HUDButtonPanel buttonPanel;
    public HUDSliderZoom sliderZoom;
    public HUDLogControl logControl;
    #endregion

    #region Private
    private RectTransform _rectTransform;
    private CanvasScaler _canvasScaler;
    private bool _isMobilePlatform;
    #endregion

    #region Awake
    void Awake()
    {
        _isMobilePlatform = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
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
    #endregion

    void Update()
    {
        if (_isMobilePlatform)
        {
            var orientation = CheckOrientationChange();
            if (CurrentOrientation != orientation)
            {
                UpdateOrientation(orientation);
            }
        }
    }

    #region Orientation
    public void SwapOrientation()
    {
        CurrentOrientation = CurrentOrientation == OrientationMode.Vertical ? isLeftHanded ? OrientationMode.HorizontalLeft : OrientationMode.HorizontalRight : OrientationMode.Vertical;
        UpdateOrientation(CurrentOrientation);
    }
    private OrientationMode CheckOrientationChange()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            return OrientationMode.Vertical;
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            return (isLeftHanded ? OrientationMode.HorizontalLeft : OrientationMode.HorizontalRight);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            return (isLeftHanded ? OrientationMode.HorizontalRight : OrientationMode.HorizontalLeft);
        }
        return OrientationMode.Vertical;
    }

    void UpdateOrientation(OrientationMode orientationMode)
    {
        CurrentOrientation = orientationMode;
        switch (orientationMode)
        {
            case OrientationMode.Vertical: PositionHudVertical(); return;
            case OrientationMode.HorizontalLeft: PositionHudHorizontalLeft(); return;
            case OrientationMode.HorizontalRight: PositionHudHorizontalRight(); return;
        }
        Debug.LogWarning("HUDManager: Orientación desconocida.  Manteniendo la posición actual del HUD.");
    }

    void PositionHudVertical()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(0.5f, 0);
        _rectTransform.anchorMax = new Vector2(0.5f, 0);
        _rectTransform.pivot = new Vector2(0.5f, 0);
        _rectTransform.offsetMin = new Vector2(padding, padding);
        _rectTransform.offsetMax = new Vector2(-padding, _rectTransform.rect.height);
    }

    void PositionHudHorizontalLeft()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(0, 0.5f);
        _rectTransform.anchorMax = new Vector2(0, 0.5f);
        _rectTransform.pivot = new Vector2(0, 0.5f);
        _rectTransform.offsetMin = new Vector2(padding, padding);
        _rectTransform.offsetMax = new Vector2(_rectTransform.rect.width, -padding);
    }

    void PositionHudHorizontalRight()
    {
        if (_rectTransform == null) return;
        _rectTransform.anchorMin = new Vector2(1, 0.5f);
        _rectTransform.anchorMax = new Vector2(1, 0.5f);
        _rectTransform.pivot = new Vector2(1, 0.5f);
        _rectTransform.offsetMin = new Vector2(-_rectTransform.rect.width, padding);
        _rectTransform.offsetMax = new Vector2(-padding, -padding);
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
