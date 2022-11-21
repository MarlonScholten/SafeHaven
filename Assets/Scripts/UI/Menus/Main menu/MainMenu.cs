using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Start scene")]
    
    [SerializeField]
    private int _startSceneId;

    [Header("Loading screen")] 
    
    [SerializeField]
    private GameObject _loadingScreenPrefab;

    [Header("Buttons enabled")]
    
    [SerializeField] 
    private bool _startButtonEnabled = true;
    
    [SerializeField] 
    private bool _loadButtonEnabled = true;
    
    [SerializeField] 
    private bool _settingsButtonEnabled = true;
    
    [SerializeField] 
    private bool _exitButtonEnabled = true;

    [Header("Button references")]
    
    [SerializeField]
    private Button _startButton;

    [SerializeField] 
    private Button _loadButton;
    
    [SerializeField] 
    private Button _settingsButton;
    
    [SerializeField] 
    private Button _exitButton;
    
    [Header("Menu types")]

    [SerializeField] 
    private GameObject _mainMenu;
    
    [SerializeField] 
    private GameObject _settingsMenu;

    private void Start()
    {
        _startButton.Select();
        _startButton.interactable = _startButtonEnabled;
        _loadButton.interactable = _loadButtonEnabled;
        _settingsButton.interactable = _settingsButtonEnabled;
        _exitButton.interactable = _exitButtonEnabled;
        
        _settingsMenu.SetActive(false);
    }

    public void OnStart()
    {
        Instantiate(_loadingScreenPrefab);
        
        SceneManager.LoadSceneAsync(_startSceneId, LoadSceneMode.Single);
    }

    public void OnLoad()
    {
        
    }

    public void OnSettings()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
    }

    public void OnExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
