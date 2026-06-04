using UnityEngine.InputSystem;
public class MenuButtonPause : MenuButton
{
    public InputActionAsset inputActions;
    public InputAction pauseAction;
    private void OnEnable()
    {
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.performed += OnPauseTriggered;
        pauseAction.Enable();
    }
    private void OnDisable()
    {
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.performed -= OnPauseTriggered;
        pauseAction.Disable();
    }
    private void OnPauseTriggered(InputAction.CallbackContext context)
    {
        if (!gameManager.IsGameActive) return;
        gameManager.TogglePause();
        if (gameManager.IsPause)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }
}
