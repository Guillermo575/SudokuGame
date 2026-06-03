using UnityEngine;
using UnityEngine.UI;

public class AutoAdjustOrientation : MonoBehaviour
{
    #region Public
    [Header("Vertical")]
    public Vector2 verticalAnchorMin = new Vector2(0.5f, 0);
    public Vector2 verticalAnchorMax = new Vector2(0.5f, 0);
    public Vector2 verticalPivot = new Vector2(0.5f, 0);
    [Header("HorizontalLeft")]
    public Vector2 horizontalLeftAnchorMin = new Vector2(0, 0.5f);
    public Vector2 horizontalLeftAnchorMax = new Vector2(0, 0.5f);
    public Vector2 horizontalLeftPivot = new Vector2(0, 0.5f);
    [Header("HorizontalRight")]
    public Vector2 horizontalRightAnchorMin = new Vector2(1, 0.5f);
    public Vector2 horizontalRightAnchorMax = new Vector2(1, 0.5f);
    public Vector2 horizontalRightPivot = new Vector2(1, 0.5f);
    #endregion

    #region Private
    private RectTransform _rectTransform;
    private CanvasScaler _canvasScaler;
    private OrientationManager orientationManager;
    private OrientationManager.OrientationMode CurrentOrientation;
    #endregion

    #region Awake
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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

    #region Update
    void Start()
    {
        orientationManager = OrientationManager.GetSingleton();
    }
    void Update()
    {
        var orientation = orientationManager.CurrentOrientation;
        if (CurrentOrientation != orientation)
        {
            UpdateOrientation(orientation);
        }
    }
    #endregion

    #region Orientation
    void UpdateOrientation(OrientationManager.OrientationMode orientationMode)
    {
        if (_rectTransform == null) return;
        CurrentOrientation = orientationMode;
        switch (orientationMode)
        {
            case OrientationManager.OrientationMode.Vertical: PositionHudVertical(); return;
            case OrientationManager.OrientationMode.HorizontalLeft: PositionHudHorizontalLeft(); return;
            case OrientationManager.OrientationMode.HorizontalRight: PositionHudHorizontalRight(); return;
        }
        Debug.LogWarning("HUDManager: Orientación desconocida.  Manteniendo la posición actual del HUD.");
    }
    void PositionHudVertical()
    {
        _rectTransform.anchorMin = verticalAnchorMin;
        _rectTransform.anchorMax = verticalAnchorMax;
        _rectTransform.pivot = verticalPivot;
    }
    void PositionHudHorizontalLeft()
    {
        _rectTransform.anchorMin = horizontalLeftAnchorMin;
        _rectTransform.anchorMax = horizontalLeftAnchorMax;
        _rectTransform.pivot = horizontalLeftPivot;
    }
    void PositionHudHorizontalRight()
    {
        _rectTransform.anchorMin = horizontalRightAnchorMin;
        _rectTransform.anchorMax = horizontalRightAnchorMax;
        _rectTransform.pivot = horizontalRightPivot;
    }
    #endregion
}