using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: PauseMenu handles all the interactions of the user with the pause menu
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
public class PauseMenu : MonoBehaviour
{
    [Header("Buttons enabled")]
    [SerializeField] 
    [Tooltip("Determines if the continue button can be interacted with in the menu")]
    private bool _continueButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the save button can be interacted with in the menu")]
    private bool _saveButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the settings button can be interacted with in the menu")]
    private bool _settingsButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the main menu button can be interacted with in the menu")]
    private bool _mainMenuButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the exit button can be interacted with in the menu")]
    private bool _exitButtonEnabled = true;
    
    [Header("ButtonReferences")]
    [SerializeField] 
    [Tooltip("A reference to the continue button UI element in the hierarchy")]
    private Button _continueButton;
    
    [SerializeField] 
    [Tooltip("A reference to the save button UI element in the hierarchy")]
    private Button _saveButton;
    
    [SerializeField] 
    [Tooltip("A reference to the settings button UI element in the hierarchy")]
    private Button _settingsButton;
    
    [SerializeField] 
    [Tooltip("A reference to the main menu button UI element in the hierarchy")]
    private Button _mainMenuButton;
    
    [SerializeField] 
    [Tooltip("A reference to the exit button UI element in the hierarchy")]
    private Button _exitButton;

    [Header("Main menu")] 
    [SerializeField] 
    [Tooltip("The id from the scene to load when the main menu button is pressed. The scene needs to be added to the build settings, the id is the place of the scene in that list.")]
    private int _mainMenuSceneId;
    
    [SerializeField] 
    [Tooltip("The prefab that contains the UI for the loading screen")]
    private GameObject _loadingScreen;

    [Header("Menu types")]
    [SerializeField] 
    [Tooltip("A reference to the pause menu UI element in the hierarchy")]
    private GameObject _pauseMenu;
    
    [SerializeField]
    [Tooltip("A reference to the settings menu UI prefab")]
    private GameObject _settingsMenu;

    private bool _wantsToReturnToMainMenu;
    private bool _wantsToExit;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinue);
        _saveButton.onClick.AddListener(OnSave);
        _settingsButton.onClick.AddListener(OnSettings);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
        _exitButton.onClick.AddListener(OnExit);

        _continueButton.interactable = _continueButtonEnabled;
        _saveButton.interactable = _saveButtonEnabled;
        _settingsButton.interactable = _settingsButtonEnabled;
        _mainMenuButton.interactable = _mainMenuButtonEnabled;
        _exitButton.interactable = _exitButtonEnabled;
        
        _continueButton.Select();
    }

    private void OnContinue()
    {
        FindObjectOfType<PauseManager>().UnpauseGame();
    }

    private void OnSave()
    {
    }

    private void OnSettings()
    {
        Instantiate(_settingsMenu, transform).GetComponent<SettingsMenu>().SetPreviousMenu(_pauseMenu);
        
        _pauseMenu.SetActive(false);
    }

    private void OnMainMenu()
    {
        if (_wantsToReturnToMainMenu)
        { 
            Instantiate(_loadingScreen);
        
            SceneManager.LoadSceneAsync(_mainMenuSceneId, LoadSceneMode.Single);
            return;
        }

        var buttonTextComponent = _mainMenuButton.GetComponentInChildren<TMP_Text>();
        buttonTextComponent.SetText("Are you sure?");
        buttonTextComponent.color = Color.red;
        _wantsToReturnToMainMenu = true;

        StartCoroutine(MainMenuConfirmationTimeout());
    }

    private void OnExit()
    {
        if (_wantsToExit)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        
            Application.Quit();
            return;
        }
        
        var buttonTextComponent = _exitButton.GetComponentInChildren<TMP_Text>();
        buttonTextComponent.SetText("Are you sure?");
        buttonTextComponent.color = Color.red;
        _wantsToExit = true;

        StartCoroutine(ExitConfirmationTimeout());
    }

    private IEnumerator MainMenuConfirmationTimeout()
    {
        yield return new WaitForSeconds(2.0f);

        var buttonTextComponent = _mainMenuButton.GetComponentInChildren<TMP_Text>();
        buttonTextComponent.SetText("Main menu");
        buttonTextComponent.color = Color.white;
        _wantsToReturnToMainMenu = false;
    }

    private IEnumerator ExitConfirmationTimeout()
    {
        yield return new WaitForSeconds(2.0f);

        var buttonTextComponent = _exitButton.GetComponentInChildren<TMP_Text>();
        buttonTextComponent.SetText("Exit");
        buttonTextComponent.color = Color.white;
        _wantsToExit = false;
    }
}
