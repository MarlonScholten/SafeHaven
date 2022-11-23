using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private GameObject _pauseMenuInstance;

    public void OnPause()
    {
        if (_pauseMenuInstance)
        {
            UnpauseGame();
            return;
        }

        PauseGame();
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        FindObjectOfType<UnityEngine.InputSystem.PlayerInput>().DeactivateInput();
        
        if (!_pauseMenuInstance) _pauseMenuInstance = Instantiate(_pauseMenu);
    }

    public void UnpauseGame()
    {
        FindObjectOfType<UnityEngine.InputSystem.PlayerInput>().ActivateInput();
        
        Cursor.visible = false;

        if (_pauseMenuInstance)
        {
            Destroy(_pauseMenuInstance);
            _pauseMenuInstance = null;
        }
    }
}
