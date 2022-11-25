using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles the interaction of the user with the main menu buttons
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>MainMenu</term>
///         <term>Prefab</term>
///         <term>MainMenu.cs</term>
///         <term>This prefab contains this script and all UI elements that are necessary to display the main menu</term>
///     </item>
/// </list>
public class MainMenu : MonoBehaviour
{
    [Header("Start scene")]
    
    [SerializeField]
    [Tooltip("The id from the scene to load when the start button is pressed. The scene needs to be added to the build settings, the id is the place of the scene in that list.")]
    private int _startSceneId;

    [Header("Loading screen")] 
    
    [SerializeField]
    [Tooltip("The prefab that contains the UI for the loading screen")]
    private GameObject _loadingScreenPrefab;

    [Header("Button settings")]
    
    [SerializeField] 
    [Tooltip("Determines if the start button can be interacted with in the menu")]
    private bool _startButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the load button can be interacted with in the menu")]
    private bool _loadButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the settings button can be interacted with in the menu")]
    private bool _settingsButtonEnabled = true;
    
    [SerializeField] 
    [Tooltip("Determines if the exit button can be interacted with in the menu")]
    private bool _exitButtonEnabled = true;

    [SerializeField] 
    [Tooltip("Determines the amount of time the user has to confirm that they want to exit the game")]
    private float _exitButtonConfirmationTimeout = 2.0f;

    [Header("Button references")]
    
    [SerializeField]
    [Tooltip("A reference to the start button UI element in the hierarchy")]
    private Button _startButton;

    [SerializeField] 
    [Tooltip("A reference to the load button UI element in the hierarchy")]
    private Button _loadButton;
    
    [SerializeField] 
    [Tooltip("A reference to the settings button UI element in the hierarchy")]
    private Button _settingsButton;
    
    [SerializeField] 
    [Tooltip("A reference to the exit button UI element in the hierarchy")]
    private Button _exitButton;
    
    [Header("Menu types")]

    [SerializeField] 
    [Tooltip("A reference to the main menu UI element in the hierarchy")]
    private GameObject _mainMenu;
    
    [SerializeField] 
    [Tooltip("A reference to the settings menu UI prefab")]
    private GameObject _settingsMenu;

    private bool _wantsToLeave;

    private void Start()
    {
        _startButton.Select();
        
        _startButton.onClick.AddListener(OnStart);
        _loadButton.onClick.AddListener(OnLoad);
        _settingsButton.onClick.AddListener(OnSettings);
        _exitButton.onClick.AddListener(OnExit);
        
        _startButton.interactable = _startButtonEnabled;
        _loadButton.interactable = _loadButtonEnabled;
        _settingsButton.interactable = _settingsButtonEnabled;
        _exitButton.interactable = _exitButtonEnabled;
    }

    private void OnStart()
    {
        Instantiate(_loadingScreenPrefab);
        
        SceneManager.LoadSceneAsync(_startSceneId, LoadSceneMode.Single);
    }

    private void OnLoad()
    {
        
    }

    private void OnSettings()
    {
        Instantiate(_settingsMenu, _mainMenu.transform.parent).GetComponent<SettingsMenu>().SetPreviousMenu(_mainMenu);
        
        _mainMenu.SetActive(false);
    }

    private void OnExit()
    {
        if (_wantsToLeave)
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
        _wantsToLeave = true;

        StartCoroutine(ExitConfirmationTimeout());
    }

    private IEnumerator ExitConfirmationTimeout()
    {
        yield return new WaitForSeconds(_exitButtonConfirmationTimeout);

        var buttonTextComponent = _exitButton.GetComponentInChildren<TMP_Text>();
        buttonTextComponent.SetText("Exit");
        buttonTextComponent.color = Color.white;
        _wantsToLeave = false;
    }
}
