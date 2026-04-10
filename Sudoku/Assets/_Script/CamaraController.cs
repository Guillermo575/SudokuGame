using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
public class CamaraController : MonoBehaviour
{
    #region Public
    public InputActionAsset inputActions;
    public GameObject tablero;
    public Vector2 Bounds;
    #endregion

    #region Private
    private Camera cam;
    private float defaultOrthographicSize;
    private float maxOrthographicSize;
    private float tableroWidth;
    private float tableroHeight;
    #endregion

    #region InputAction
    private InputAction moveAction;
    private InputAction zoomScroll;
    private InputAction centerAction;
    private InputAction clickAction;
    #endregion

    #region Start & Update
    void Start()
    {
    }
    void Update()
    {
        HandleMovement();
        LimitarCamara();
        HandleZoom();
        HandleCenter();
        HandleClick();
    }
    #endregion

    #region General
    public void InitiateCamera()
    {
        cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic)
        {
            Debug.LogError("Este script requiere una cámara ortográfica.");
            return;
        }
        moveAction = inputActions.FindAction("Move");
        zoomScroll = inputActions.FindAction("Scroll");
        centerAction = inputActions.FindAction("Center");
        clickAction = inputActions.FindAction("Click");
        moveAction.Enable();
        zoomScroll.Enable();
        centerAction.Enable();
        clickAction.Enable();
        defaultOrthographicSize = cam.orthographicSize;
        maxOrthographicSize = defaultOrthographicSize;
        float sizeHorizontal = tablero.transform.localScale.x;
        float sizeVertical = tablero.transform.localScale.z;
        tableroWidth = sizeHorizontal;
        tableroHeight = sizeVertical;
        CentrarCamara(tableroWidth, tableroHeight);
    }
    private void HandleMovement()
    {
        Vector3 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 delta = new Vector3(moveInput.x, 0f, moveInput.y) * Time.deltaTime * 10f;
        transform.position += delta;
    }
    private void HandleZoom()
    {
        if (zoomScroll.triggered)
        {
            Vector2 scrollValue = zoomScroll.ReadValue<Vector2>();
            cam.orthographicSize += scrollValue.y;
        }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, maxOrthographicSize);
    }
    private void HandleCenter()
    {
        if (centerAction.triggered)
        {
            CentrarCamara(tableroWidth, tableroHeight);
        }
    }
    private void HandleClick()
    {
        if (clickAction.triggered)
        {
            DetectButtonClick();
        }
    }
    public void CentrarCamara(float tamañoHorizontal, float tamañoVertical)
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float sizeX = tamañoHorizontal / 2f;
        float sizeY = tamañoVertical / 2f;
        float newSize = Mathf.Max(sizeX / aspectRatio, sizeY) + ((Bounds.x + Bounds.y) / 2);
        maxOrthographicSize = newSize;
        cam.orthographicSize = newSize;
        float centerX = tamañoHorizontal / 2f;
        float centerY = -tamañoVertical / 2f;
        transform.position = new Vector3(centerX, 10f, centerY);
    }
    private void LimitarCamara()
    {
        float minCamX = tablero.transform.position.x - Bounds.x;
        float maxCamX = tablero.transform.position.x + tableroWidth +  Bounds.x;
        float minCamZ = tablero.transform.position.z - tableroHeight - Bounds.y;
        float maxCamZ = tablero.transform.position.z + Bounds.y;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minCamX, maxCamX);
        pos.z = Mathf.Clamp(pos.z, minCamZ, maxCamZ);
        transform.position = pos;
    }
    private void DetectButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);
            SudokuNumberNode nodeButton = hit.collider.GetComponent<SudokuNumberNode>();
            if (nodeButton != null)
            {
                Debug.Log("BOTON OPRIMIDO");
            }
        }
    }
    #endregion
}