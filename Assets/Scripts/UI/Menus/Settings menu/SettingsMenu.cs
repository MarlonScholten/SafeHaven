using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private GameObject _previousMenu;

    [SerializeField]
    [Tooltip("A reference to the button UI element that exits this menu")]
    private Button _exitButton;

    private void Start()
    {
        _exitButton.onClick.AddListener(CloseMenu);
        
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
}
