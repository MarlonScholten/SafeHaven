using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: Pause manager handles pausing and unpausing the game
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>PauseManager</term>
///         <term>Prefab</term>
///         <term>PauseManager.cs</term>
///         <term>The <c>PauseManager</c> prefab contains everything needed to pause and unpause the game</term>
///     </item>
///     <item>
///         <term>PauseMenu</term>
///         <term>Prefab</term>
///         <term>PauseMenu.cs</term>
///         <term>The <c>PauseMenu</c> prefab contains everything needed to display the pause menu UI</term>
///     </item>
/// </list>
public class PauseManager : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("A reference to the pause menu UI prefab")]
    private GameObject _pauseMenu;

    private GameObject _pauseMenuInstance;

    private void Start()
    {
        InputBehaviour.Instance.OnPause += OnPause;
    }

    /// <summary>
    /// Method <c>OnPause</c> gets called when the user presses the pause button and toggles the pause state
    /// </summary>
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (_pauseMenuInstance)
        {
            UnpauseGame();
            return;
        }

        PauseGame();
    }

    /// <summary>
    /// Method <c>PauseGame</c> pauses the game and spawns the pause menu
    /// </summary>
    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        InputBehaviour.Instance.gameObject.SetActive(false);
        
        if (!_pauseMenuInstance) _pauseMenuInstance = Instantiate(_pauseMenu);
    }

    /// <summary>
    /// Method <c>UnpauseGame</c> unpauses the game and destroys the pause menu
    /// </summary>
    public void UnpauseGame()
    {
        InputBehaviour.Instance.gameObject.SetActive(true);
        
        Cursor.visible = false;

        if (_pauseMenuInstance)
        {
            Destroy(_pauseMenuInstance);
            _pauseMenuInstance = null;
        }
    }
}
