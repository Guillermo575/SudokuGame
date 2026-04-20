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
    private bool isDragging = false;
    private Vector2 lastMousePosition;
    private float initialPinchDistance;
    private float initialOrthographicSize;
    private bool isPinching = false;
    #endregion

    #region InputAction
    private InputAction moveAction;
    private InputAction zoomScroll;
    private InputAction centerAction;
    private InputAction clickAction;
    #endregion

    #region Start & Update
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
            Debug.LogError("Este script requiere una camara ortografica.");
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
    private void LimitarCamara()
    {
        float minCamX = tablero.transform.position.x - Bounds.x;
        float maxCamX = tablero.transform.position.x + tableroWidth + Bounds.x;
        float minCamZ = tablero.transform.position.z - tableroHeight - Bounds.y;
        float maxCamZ = tablero.transform.position.z + Bounds.y;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minCamX, maxCamX);
        pos.z = Mathf.Clamp(pos.z, minCamZ, maxCamZ);
        transform.position = pos;
    }
    #endregion

    #region Mover Camara
    private void HandleMovement()
    {
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            if (Touchscreen.current.touches.Count == 2)
            {
                HandlePinch();
            }
            else
            {
                HandleTouchDrag();
            }
        }
        else
        {
            HandleMouseDrag();
        }
        Vector3 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 delta = new Vector3(moveInput.x, 0f, moveInput.y) * Time.deltaTime * 10f;
        transform.position += delta;
    }
    private void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (!isDragging)
            {
                isDragging = true;
                lastMousePosition = mousePos;
            }
            else
            {
                Vector2 delta = lastMousePosition - mousePos;
                Vector3 move = new Vector3(delta.x, 0, delta.y) * Time.deltaTime * 10f;
                transform.position += move;
                lastMousePosition = mousePos;
            }
        }
        else
        {
            isDragging = false;
        }
    }
    private void HandleTouchDrag()
    {
        var touches = Touchscreen.current.touches;
        if (touches.Count == 1)
        {
            var touch = touches[0];
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                isDragging = true;
                lastMousePosition = touch.position.ReadValue();
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
            {
                Vector2 currentPos = touch.position.ReadValue();
                Vector2 delta = lastMousePosition - currentPos;
                Vector3 move = new Vector3(delta.x, 0, delta.y) * Time.deltaTime * 10f;
                transform.position += move;
                lastMousePosition = currentPos;
            }
            else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
    #endregion

    #region Zoom
    private void HandleZoom()
    {
        if (zoomScroll.triggered)
        {
            Vector2 scrollValue = zoomScroll.ReadValue<Vector2>();
            cam.orthographicSize += -scrollValue.y;
        }
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 1f, maxOrthographicSize);
    }
    private void HandlePinch()
    {
        var touches = Touchscreen.current.touches;
        if (touches.Count >= 2)
        {
            var touch1 = touches[0];
            var touch2 = touches[1];
            Vector2 pos1 = touch1.position.ReadValue();
            Vector2 pos2 = touch2.position.ReadValue();
            float currentDistance = Vector2.Distance(pos1, pos2);
            if (!isPinching)
            {
                isPinching = true;
                initialPinchDistance = currentDistance;
                initialOrthographicSize = cam.orthographicSize;
            }
            else
            {
                float deltaDistance = currentDistance - initialPinchDistance;
                float zoomFactor = deltaDistance * 0.01f;
                cam.orthographicSize = Mathf.Clamp(initialOrthographicSize - zoomFactor, 1f, maxOrthographicSize);
            }
        }
        else
        {
            isPinching = false;
        }
    }
    #endregion

    #region Centrar Camara
    private void HandleCenter()
    {
        if (centerAction.triggered)
        {
            CentrarCamara(tableroWidth, tableroHeight);
        }
    }
    public void CentrarCamara(float Horizontal, float Vertical)
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float sizeX = Horizontal / 2f;
        float sizeY = Vertical / 2f;
        float newSize = Mathf.Max(sizeX / aspectRatio, sizeY) + ((Bounds.x + Bounds.y) / 2);
        maxOrthographicSize = newSize;
        cam.orthographicSize = newSize;
        float centerX = Horizontal / 2f;
        float centerY = -Vertical / 2f;
        transform.position = new Vector3(centerX, 10f, centerY);
    }
    #endregion

    #region Detect Click
    private void HandleClick()
    {
        if (clickAction.triggered)
        {
            DetectButtonClick();
        }
    }
    private void DetectButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            SudokuNumberCell nodeButton = hit.collider.GetComponent<SudokuNumberCell>();
            if (nodeButton != null && !nodeButton.Bloqueado)
            {
                GameManager.GetSingleton().setCellSelected(nodeButton);
            }
        }
    }
    //private void DetectButtonClick()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        SudokuNumberCell nodeButton = hit.collider.GetComponent<SudokuNumberCell>();
    //        if (nodeButton != null && !nodeButton.Bloqueado)
    //        {
    //            if (!isDragging && !isPinching)
    //            {
    //                GameManager.GetSingleton().setCellSelected(nodeButton);
    //            }
    //        }
    //    }
    //}
    #endregion
    private void DetectKey()
    {
        //foreach (Key key in Keyboard.current.allKeys)
        //{
        //    if (Keyboard.current[key].isPressed)
        //    {
        //        Debug.Log($"Tecla presionada: {key}");
        //        break;
        //    }
        //}
    }
}
