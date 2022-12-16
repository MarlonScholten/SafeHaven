using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class is a container that holds different items that change user settings.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>GameSettingManager</term>
///         <term>Prefab</term>
///         <term>GameSettingManager</term>
///         <term>This prefab contains the scripts that handle applying game settings</term>
///     </item>
///     <item>
///         <term>InputManager</term>
///         <term>Prefab</term>
///         <term>InputManager</term>
///         <term>This prefab contains the components that handle player input</term>
///     </item>
///     <item>
///         <term>SettingsMenu</term>
///         <term>Prefab</term>
///         <term>KeybindingUI.cs</term>
///         <term>This script handles the UI components to change keybindings</term>
///     </item>
/// </list>
public class SettingsMenu : MonoBehaviour
{
    private GameObject _previousMenu;

    [SerializeField]
    [Tooltip("A reference to the button UI element that exits this menu")]
    private Button _exitButton;
    
    [SerializeField]
    [Tooltip("A reference to the button UI element that resets the keybindings")]
    private Button _resetKeybindingsButton;

    private void Start()
    {
        _exitButton.onClick.AddListener(CloseMenu);
        _resetKeybindingsButton.onClick.AddListener(ResetKeybindings);
        
        _exitButton.Select();
    }

    /// <summary>
    /// Sets the menu that spawned this menu, closing this menu reactivates this previous menu
    /// </summary>
    /// <param name="previousMenu">The menu that is reactivated when this menu is closed</param>
    public void SetPreviousMenu(GameObject previousMenu)
    {
        _previousMenu = previousMenu;
    }

    private void CloseMenu()
    {
        _previousMenu.SetActive(true);
        
        Destroy(gameObject);
    }

    private void ResetKeybindings()
    {
        FindObjectOfType<KeybindingSettings>()?.ResetKeybindings();

        foreach (KeybindingUI keybindingUI in GetComponentsInChildren<KeybindingUI>())
        {
            keybindingUI.RefreshRebindingButton();
        }
    }
}
